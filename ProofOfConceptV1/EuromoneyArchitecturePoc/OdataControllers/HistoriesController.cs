using System.Linq;
using System.Web.Http.OData;
using EuromoneyBca.Domain.Allocations.Data;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyArchitecturePoc.OdataControllers
{

    public class HistoriesController : EntitySetController<History, int>
    {
        private readonly IBcaAllocationsDataService _bcaAllocationsDataService;

        public HistoriesController()
        {
            _bcaAllocationsDataService = new BcaAllocationsDataService();
        }

        protected override History GetEntityByKey(int key)
        {
            return _bcaAllocationsDataService.Query<History>().FirstOrDefault(a => a.Id == key);
        }

        public override IQueryable<History> Get()
        {
            return _bcaAllocationsDataService.Query<History>();

        }

        protected override int GetKey(History entity)
        {
            return entity.Id;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _bcaAllocationsDataService.Dispose();
        }

    }
}
