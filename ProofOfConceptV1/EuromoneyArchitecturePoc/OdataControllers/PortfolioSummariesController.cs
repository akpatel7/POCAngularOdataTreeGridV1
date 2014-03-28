using System.Linq;
using System.Web.Http.OData;
using EuromoneyBca.Domain.Allocations.Data;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyArchitecturePoc.OdataControllers
{

    public class PortfolioSummariesController : EntitySetController<PortfolioSummary, int>
    {
        private readonly IBcaAllocationsDataService _bcaAllocationsDataService;

        public PortfolioSummariesController()
        {
            _bcaAllocationsDataService = new BcaAllocationsDataService();

        }

        [ApiAuthorize]
        protected override PortfolioSummary GetEntityByKey(int key)
        {
            return _bcaAllocationsDataService.Query<PortfolioSummary>().FirstOrDefault(p => p.Id == key);
        }

        [ApiAuthorize]
        public override IQueryable<PortfolioSummary> Get()
        {
            return _bcaAllocationsDataService.Query<PortfolioSummary>();

        }

        [ApiAuthorize]
        protected override int GetKey(PortfolioSummary entity)
        {
            return entity.Id;
        }

        [ApiAuthorize]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _bcaAllocationsDataService.Dispose();
        }

    }
}
