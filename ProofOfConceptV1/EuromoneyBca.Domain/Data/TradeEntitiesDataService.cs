using System.Data.Entity;
using System.Linq;
using EuromoneyBca.Domain.Data.Models;

namespace EuromoneyBca.Domain.Data
{
    public class TradeEntitiesDataService : DbContext, ITradeEntitiesDataService
    {
        public TradeEntitiesDataService()
            : base("name=BCATradeEntities")
        {}
        
        IQueryable<T> ITradeEntitiesDataService.Query<T>()
        {
            return Set<T>();
        }

        void ITradeEntitiesDataService.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void ITradeEntitiesDataService.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void ITradeEntitiesDataService.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void ITradeEntitiesDataService.SaveChanges()
        {
            SaveChanges();
        }
    }
}
