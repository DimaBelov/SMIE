using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMIE.Core.Data.Dapper;

namespace SMIE.Core.Data
{
    public abstract class AppService
    {
        readonly IGenericRepository _repository;

        protected AppService(IGenericRepository repository)
        {
            _repository = repository;
        }

        protected virtual T Get<T>(ISpecification criteria)
        {
            return _repository.First<T>(criteria);
        }

        protected virtual Task<T> GetAsync<T>(ISpecification criteria)
        {
            return _repository.FirstAsync<T>(criteria);
        }

        protected virtual T Get<TDto, T>(ISpecification criteria)
            where TDto : IDapperDto<T>
            where T : class
        {
            var repositoryItem = _repository.First<TDto>(criteria);
            return repositoryItem != null ? repositoryItem.GetEntity() : null;
        }

        protected virtual async Task<T> GetAsync<TDto, T>(ISpecification criteria)
            where TDto : IDapperDto<T>
            where T : class
        {
            var repositoryItem = await _repository.FirstAsync<TDto>(criteria);
            return repositoryItem != null ? repositoryItem.GetEntity() : null;
        }

        protected virtual IEnumerable<dynamic> GetAll(ISpecification criteria)
        {
            return _repository.All(criteria);
        }

        protected virtual Task<IEnumerable<dynamic>> GetAllAsync(ISpecification criteria)
        {
            return _repository.AllAsync(criteria);
        }

        protected virtual T Get<T>(ISpecification criteria, Func<IDisposable, T> read)
        {
            return _repository.Multiple(criteria, read);
        }

        protected virtual Task<T> GetAsync<T>(ISpecification criteria, Func<IDisposable, Task<T>> read)
        {
            return _repository.MultipleAsync(criteria, read);
        }

        protected virtual IEnumerable<T> GetAll<T>(ISpecification criteria)
        {
            return _repository.All<T>(criteria);
        }

        protected virtual Task<IEnumerable<T>> GetAllAsync<T>(ISpecification criteria)
        {
            return _repository.AllAsync<T>(criteria);
        }

        protected virtual IEnumerable<T> GetAll<TDto, T>(ISpecification criteria)
            where TDto : IDapperDto<T>
            where T : class
        {
            return _repository.All<TDto>(criteria).Select(x => x.GetEntity());
        }

        protected virtual async Task<IEnumerable<T>> GetAllAsync<TDto, T>(ISpecification criteria)
            where TDto : IDapperDto<T>
            where T : class
        {
            return (await GetAllAsync<TDto>(criteria)).Select(x => x.GetEntity());
        }

        protected virtual int Execute(ISpecification criteria)
        {
            return _repository.Execute(criteria);
        }

        protected virtual Task<int> ExecuteAsync(ISpecification criteria)
        {
            return _repository.ExecuteAsync(criteria);
        }

        protected virtual IEnumerable<T> Read<T>(IDisposable gridReader)
        {
            return _repository.Read<T>(gridReader);
        }

        protected virtual Task<IEnumerable<T>> ReadAsync<T>(IDisposable gridReader)
        {
            return _repository.ReadAsync<T>(gridReader);
        }

        protected virtual IEnumerable<T> Read<TDto, T>(IDisposable gridReader)
            where TDto : IDapperDto<T>
            where T : class
        {
            return _repository.Read<TDto>(gridReader).
                Select(x => x.GetEntity());
        }

        protected virtual async Task<IEnumerable<T>> ReadAsync<TDto, T>(IDisposable gridReader)
            where TDto : IDapperDto<T>
            where T : class
        {
            return (await ReadAsync<TDto>(gridReader)).Select(x => x.GetEntity());
        }
    }
}
