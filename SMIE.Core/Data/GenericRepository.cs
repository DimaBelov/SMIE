using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SMIE.Core.CrossCutting.Exceptions;
using SMIE.Core.CrossCutting.Extensions;
using SMIE.Core.Data.Dapper;

namespace SMIE.Core.Data
{
    public class GenericRepository : IGenericRepository
    {
        const int CONNECTION_TRY_COUNT = 3;
        const int DEADLOCK_TRY_COUNT = 5;

        protected readonly IDbConnectionsManager _connectionsManager;
        protected readonly string ServerName;

        public GenericRepository(IDbConnectionsManager connectionsManager, string serverName)
        {
            ServerName = serverName;
            _connectionsManager = connectionsManager;
        }

        public T First<T>(ISpecification criteria)
        {
            return Execute<T>(criteria).FirstOrDefault();
        }

        public async Task<T> FirstAsync<T>(ISpecification criteria)
        {
            return (await ExecuteAsync<T>(criteria)).FirstOrDefault();
        }

        public IEnumerable<dynamic> All(ISpecification criteria)
        {
            return Execute(x => x.Query(criteria.GetCommandText(),
                    criteria.GetParameters(),
                    commandType: criteria.GetCommandType(),
                    commandTimeout: criteria.CommandTimeOut));
        }

        public Task<IEnumerable<dynamic>> AllAsync(ISpecification criteria)
        {
            return ExecuteAsync(x => x.QueryAsync(criteria.GetCommandText(),
                    criteria.GetParameters(),
                    commandType: criteria.GetCommandType(),
                    commandTimeout: criteria.CommandTimeOut));
        }

        public IEnumerable<T> All<T>(ISpecification criteria)
        {
            return Execute<T>(criteria);
        }

        public Task<IEnumerable<T>> AllAsync<T>(ISpecification criteria)
        {
            return ExecuteAsync<T>(criteria);
        }

        public IEnumerable<T> All<T>(ISpecification criteria, Func<IDisposable, IEnumerable<T>> read)
        {
            return Multiple(criteria, read);
        }

        public Task<IEnumerable<T>> AllAsync<T>(ISpecification criteria, Func<IDisposable, Task<IEnumerable<T>>> read)
        {
            return MultipleAsync(criteria, read);
        }

        public int Execute(ISpecification criteria)
        {
            try
            {
                Execute(
                    x => x.Execute(
                        criteria.GetCommandText(),
                        criteria.GetParameters(),
                        commandType: criteria.GetCommandType(),
                        commandTimeout: criteria.CommandTimeOut));
                return criteria.Output();
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        public async Task<int> ExecuteAsync(ISpecification criteria)
        {
            try
            {
                await ExecuteAsync(
                    x => x.ExecuteAsync(
                        criteria.GetCommandText(),
                        criteria.GetParameters(),
                        commandType: criteria.GetCommandType(),
                        commandTimeout: criteria.CommandTimeOut));
                return criteria.Output();
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        public T Multiple<T>(ISpecification criteria, Func<IDisposable, T> read)
        {
            try
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
                return Execute(
                    c =>
                    {
                        using (var reader = c.QueryMultiple(
                            criteria.GetCommandText(),
                            criteria.GetParameters(),
                            commandType: criteria.GetCommandType(),
                            commandTimeout: criteria.CommandTimeOut))
                        {
                            return read(reader);
                        }
                    });
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        public async Task<T> MultipleAsync<T>(ISpecification criteria, Func<IDisposable, Task<T>> read)
        {
            try
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
                return await ExecuteAsync(
                    async c =>
                    {
                        using (var reader = await c.QueryMultipleAsync(
                            criteria.GetCommandText(),
                            criteria.GetParameters(),
                            commandType: criteria.GetCommandType(),
                            commandTimeout: criteria.CommandTimeOut))
                        {
                            return await read(reader);
                        }
                    });
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        public IEnumerable<T> Read<T>(IDisposable gridReader)
        {
            SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
            return ((SqlMapper.GridReader) gridReader).Read<T>();
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IDisposable gridReader)
        {
            SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
            return ((SqlMapper.GridReader) gridReader).ReadAsync<T>();
        }

        IEnumerable<T> Execute<T>(ISpecification criteria)
        {
            try
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
                return Execute(x => x.Query<T>(criteria.GetCommandText(),
                        criteria.GetParameters(),
                        commandType: criteria.GetCommandType(),
                        commandTimeout: criteria.CommandTimeOut));
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        async Task<IEnumerable<T>> ExecuteAsync<T>(ISpecification criteria)
        {
            try
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
                return await ExecuteAsync(
                    x => x.QueryAsync<T>(
                        criteria.GetCommandText(),
                        criteria.GetParameters(),
                        commandType: criteria.GetCommandType(),
                        commandTimeout: criteria.CommandTimeOut));
            }
            catch (SqlException e)
            {
                throw new ErpSqlException(criteria.GetCommandText(), e);
            }
        }

        protected virtual T Execute<T>(Func<IDbConnection, T> func)
        {
            var tryCount = CONNECTION_TRY_COUNT;
            IDbConnection connection = null;
            do
            {
                try
                {
                    using (connection = _connectionsManager.CreateConnection(ServerName))
                    {
                        connection.Open();
                        var connection1 = connection;
                        return ExecuteWithHandleDeadlock(() => func(connection1));
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.IsDefinedError())
                        throw;

                    if (--tryCount < 0)
                        throw;
                }
            } while (true);
        }

        async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            var tryCount = CONNECTION_TRY_COUNT;
            DbConnection connection = null;
            do
            {
                try
                {
                    using (connection = (DbConnection) _connectionsManager.CreateConnection(ServerName))
                    {
                        await connection.OpenAsync();
                        var connection1 = connection;
                        return await ExecuteWithHandleDeadlock(() => func(connection1));
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.IsDefinedError())
                        throw;

                    if (--tryCount < 0)
                        throw;
                }
            } while (true);
        }

        protected T ExecuteWithHandleDeadlock<T>(Func<T> func)
        {
            var tryCount = DEADLOCK_TRY_COUNT;
            do
            {
                try
                {
                    return func();
                }
                catch (SqlException sqlException)
                {
                    if (sqlException.Number != 1205 || --tryCount < 0)
                        throw;
                    Thread.Sleep(10);
                }
            } while (true);
        }
    }
}