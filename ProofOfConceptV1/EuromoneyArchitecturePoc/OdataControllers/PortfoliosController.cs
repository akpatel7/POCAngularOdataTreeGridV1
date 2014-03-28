using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Mvc;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using EuromoneyBca.Domain.Allocations.Data;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyArchitecturePoc.OdataControllers
{
    public class PortfoliosController : EntitySetController<Portfolio, int>
    {
        private readonly IBcaAllocationsDataService _bcaAllocationsDataService;

        public PortfoliosController()
        {
            _bcaAllocationsDataService = new BcaAllocationsDataService();

        }

        [ApiAuthorize]
        protected override Portfolio GetEntityByKey(int key)
        {
            return _bcaAllocationsDataService.Query<Portfolio>().FirstOrDefault(p => p.Id == key);
        }

        [ApiAuthorize]
        public override IQueryable<Portfolio> Get()
        {
            return _bcaAllocationsDataService.Query<Portfolio>();

        }

        [ApiAuthorize]
        protected override int GetKey(Portfolio entity)
        {
            return entity.Id;
        }

        [ApiAuthorize]
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

                var portfolio = _bcaAllocationsDataService.Query<Portfolio>().Single(p => p.Id == key);
                //Thread.Sleep(3000);
                return Request.CreateResponse(HttpStatusCode.OK, portfolio.History);
                // var enrollment = _repository.GetOne(NormalizeKey(key));
                // return Request.CreateResponse(HttpStatusCode.OK, enrollment.Status);
            }
            return base.HandleUnmappedRequest(odataPath);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _bcaAllocationsDataService.Dispose();
        }

    }
}
