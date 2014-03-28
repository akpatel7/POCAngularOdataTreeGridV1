using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using EuromoneyBca.Domain.Allocations.Data;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyArchitecturePoc.OdataControllers
{

    public class AllocationsController : EntitySetController<Allocation, int>
    {
        private readonly IBcaAllocationsDataService _bcaAllocationsDataService;

        public AllocationsController()
        {
            _bcaAllocationsDataService = new BcaAllocationsDataService();

        }

        protected override Allocation GetEntityByKey(int key)
        {
            return _bcaAllocationsDataService.Query<Allocation>().FirstOrDefault(a => a.Id == key);
        }

        public override IQueryable<Allocation> Get()
        {
            return _bcaAllocationsDataService.Query<Allocation>();

        }

        protected override int GetKey(Allocation entity)
        {
            return entity.Id;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _bcaAllocationsDataService.Dispose();
        }

        public override HttpResponseMessage HandleUnmappedRequest(ODataPath odataPath)
        {
            // This syntax:
            //      Portfolios(461)?$expand=History
            // is not supported by web api yet.
            // This is a valid alternative:
            //      Portfolios(461)/History
            // However the default routing will not allow this.
            // The best solution is suggested here:
            //      http://aspnetwebstack.codeplex.com/discussions/435031
            // Our implementation below:

            if (odataPath.PathTemplate == "~/entityset/key/navigation")
            {
                var keyValuePathSegment = odataPath.Segments[1] as KeyValuePathSegment;
                if (keyValuePathSegment == null) throw new ArgumentException("Key cannot be null");
                int key;
                if (!int.TryParse(keyValuePathSegment.Value, out key)) throw new ArgumentException("Key must be an integer");

                var propertyNameSegment = odataPath.Segments[2] as NavigationPathSegment;
                if (propertyNameSegment == null) throw new ArgumentException("Property cannot be null");

                if (propertyNameSegment.NavigationPropertyName != "History") throw new ArgumentException("Only History can be queried via the key");

                var allocation = _bcaAllocationsDataService.Query<Allocation>().Single(p => p.Id == key);
                //Thread.Sleep(3000);
                return Request.CreateResponse(HttpStatusCode.OK, allocation.History);
                // var enrollment = _repository.GetOne(NormalizeKey(key));
                // return Request.CreateResponse(HttpStatusCode.OK, enrollment.Status);
            }
            return base.HandleUnmappedRequest(odataPath);
        }

    }
}
