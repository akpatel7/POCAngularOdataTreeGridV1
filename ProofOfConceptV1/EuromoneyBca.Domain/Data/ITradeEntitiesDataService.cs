using System;
using System.Linq;

namespace EuromoneyBca.Domain.Data
{
    public interface ITradeEntitiesDataService : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void SaveChanges();
    }
}
