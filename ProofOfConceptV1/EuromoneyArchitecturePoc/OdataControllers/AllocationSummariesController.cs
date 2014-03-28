using System.Linq;
using System.Web.Http.OData;
using EuromoneyBca.Domain.Allocations.Data;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyArchitecturePoc.Controllers
{

    public class AllocationSummariesController : EntitySetController<AllocationSummary, int>
    {
        private readonly IBcaAllocationsDataService _bcaAllocationsDataService;

        public AllocationSummariesController()
        {
            _bcaAllocationsDataService = new BcaAllocationsDataService();

        }
        protected override AllocationSummary GetEntityByKey(int key)
        {
            return _bcaAllocationsDataService.Query<AllocationSummary>().FirstOrDefault(p => p.Id == key);
        }

        // We need this cos of the really long filter....
        // $filter=Portfolio_Id+eq+1+or+Portfolio_Id+eq+2+or+Portfolio_Id+eq+3+or+Portfolio_Id+eq+4+or+Portfolio_Id+eq+5+or+Portfolio_Id+eq+6+or+Portfolio_Id+eq+7+or+Portfolio_Id+eq+8+or+Portfolio_Id+eq+9+or+Portfolio_Id+eq+10+or+Portfolio_Id+eq+11+or+Portfolio_Id+eq+12+or+Portfolio_Id+eq+13+or+Portfolio_Id+eq+14+or+Portfolio_Id+eq+15+or+Portfolio_Id+eq+16+or+Portfolio_Id+eq+17+or+Portfolio_Id+eq+18+or+Portfolio_Id+eq+19+or+Portfolio_Id+eq+20
        // Or we get this error:
        // MaxNodeCount > 100
        // Need better solution
        
        public override IQueryable<AllocationSummary> Get()
        {
            return _bcaAllocationsDataService.Query<AllocationSummary>();

        }

        protected override int GetKey(AllocationSummary entity)
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
