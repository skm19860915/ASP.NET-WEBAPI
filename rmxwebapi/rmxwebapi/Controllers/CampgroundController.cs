using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class CampgroundController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public CampgroundController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<IEnumerable<CampgroundModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var campgrounds = await Task.FromResult(_repo.GetAllCampgrounds());
            return await Task.FromResult(campgrounds);
        }

        public async Task<LocationCampViewModel> Get(long id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var campgrounds = await Task.FromResult(_repo.GetAllCampgrounds());
            var locations = await Task.FromResult(_repo.GetAllLocations());
            var primaryLocation = locations.FirstOrDefault(x => x.SequenceId == id);

            var list = GetCampgroundInfoInLocation(campgrounds.ToList(), primaryLocation.Latitude ?? 0, primaryLocation.Longitude ?? 0);

            var record = new LocationCampViewModel()
            {
                WebGoogleMapJavaScriptAPIKey = primaryLocation.WebGoogleMapJavaScriptAPIKey,
                Latitude = primaryLocation.Latitude,
                Longitude = primaryLocation.Longitude,
                MapList = list
            };
            return record;
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        public string Get(long id)
        {
            return _utility._noAuthorizationMessage;
        }

        private List<CampgroundModel> GetCampgroundInfoInLocation(List<CampgroundModel> list, double targetLat, double targetLng)
        {
            var arr = new List<CampgroundModel>();
            foreach(var item in list)
            {
                if((item.Lat >= targetLat - 2 && item.Lat <= targetLat + 2) &&(item.Lng >= targetLng - 3 && item.Lng <= targetLng + 3))
                {
                    arr.Add(item);
                }
            }
            return arr;
        }
    }
}
