using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMIE.Core.Data
{
    public interface IGenericRepository
    {
        T First<T>(ISpecification criteria);
        Task<T> FirstAsync<T>(ISpecification criteria);

        IEnumerable<dynamic> All(ISpecification criteria);
        Task<IEnumerable<dynamic>> AllAsync(ISpecification criteria);

        IEnumerable<T> All<T>(ISpecification criteria);
        Task<IEnumerable<T>> AllAsync<T>(ISpecification criteria);

        IEnumerable<T> All<T>(ISpecification criteria, Func<IDisposable, IEnumerable<T>> read);
        Task<IEnumerable<T>> AllAsync<T>(ISpecification criteria, Func<IDisposable, Task<IEnumerable<T>>> read);

        int Execute(ISpecification criteria);
        Task<int> ExecuteAsync(ISpecification criteria);

        T Multiple<T>(ISpecification criteria, Func<IDisposable, T> read);
        Task<T> MultipleAsync<T>(ISpecification criteria, Func<IDisposable, Task<T>> read);

        IEnumerable<T> Read<T>(IDisposable gridReader);
        Task<IEnumerable<T>> ReadAsync<T>(IDisposable gridReader);
    }
}
