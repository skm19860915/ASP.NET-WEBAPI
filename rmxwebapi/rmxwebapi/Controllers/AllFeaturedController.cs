using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class AllFeaturedController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public AllFeaturedController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<string>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());

            var vehicleIncludingMedia = from m in medias
                            join v in vehicles on m.Vehicle equals v.Oid
                            select new
                            {
                                Location = v.Location,
                                WebUniqueId = v.WebUniqueId,
                                VehicleClass = v.VehicleClass
                            };

            var vehicleOnLocation = from v in vehicleIncludingMedia
                          join l in locations on v.Location equals l.Oid
                          select new
                          {
                              WebUniqueId = v.WebUniqueId,
                              VehicleClass = v.VehicleClass
                          };

            var result = from v in vehicleOnLocation
                          join c in classes on v.VehicleClass equals c.Oid
                          select v;

            var list = result.Select(x => x.WebUniqueId).ToList();
            return await Task.FromResult(list);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
