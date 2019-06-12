using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class CacheController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;
        private int _errorStatus = 200;

        public CacheController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<int> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                _errorStatus = 301;

            var list = _repo._cache.ToList();
            foreach (var item in list)
            {
                _repo._cache.Remove(item.Key);
            }

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                _errorStatus = 1;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                _errorStatus = 2;

            var vehicleClasses = await Task.FromResult(_repo.GetAllVehicleClasses());
            if (vehicleClasses == null || vehicleClasses.Count() < 1)
                _errorStatus = 3;

            var vehicleMediaItems = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            if (vehicleMediaItems == null || vehicleMediaItems.Count() < 1)
                _errorStatus = 4;

            var mediaList = await Task.FromResult(_repo.GetAllMediaList());
            if (mediaList == null || mediaList.Count() < 1)
                _errorStatus = 5;

            var webCityStaticPages = await Task.FromResult(_repo.GetAllWebCityStaticPages());
            if (webCityStaticPages == null || webCityStaticPages.Count() < 1)
                _errorStatus = 6;

            var companies = await Task.FromResult(_repo.GetAllCompanies());
            if (companies == null || companies.Count() < 1)
                _errorStatus = 7;

            var rentals = await Task.FromResult(_repo.GetAllRentals());
            if (rentals == null || rentals.Count() < 1)
                _errorStatus = 8;

            var amenities = await Task.FromResult(_repo.GetAllAmenities());
            if (amenities == null || amenities.Count() < 1)
                _errorStatus = 9;

            var vehicleAmenities = await Task.FromResult(_repo.GetAllVehiclesAmenities());
            if (vehicleAmenities == null || vehicleAmenities.Count() < 1)
                _errorStatus = 10;

            var equipmentTypes = await Task.FromResult(_repo.GetAllEquipmentTypes());
            if (equipmentTypes == null || equipmentTypes.Count() < 1)
                _errorStatus = 11;

            var fees = await Task.FromResult(_repo.GetAllFees());
            if (fees == null || fees.Count() < 1)
                _errorStatus = 12;

            var locationInsuranceCompanies = await Task.FromResult(_repo.GetAllLocationInsuranceCompanies());
            if (locationInsuranceCompanies == null || locationInsuranceCompanies.Count() < 1)
                _errorStatus = 13;

            var leadSources = await Task.FromResult(_repo.GetAllLeadSources());
            if (leadSources == null || leadSources.Count() < 1)
                _errorStatus = 14;

            var campgrounds = await Task.FromResult(_repo.GetAllCampgrounds());
            if (campgrounds == null || campgrounds.Count() < 1)
                _errorStatus = 15;

            var eventVehicles = await Task.FromResult(_repo.GetAllEventVehicles());
            if (eventVehicles == null || eventVehicles.Count() < 1)
                _errorStatus = 16;

            var matchVehicles = await Task.FromResult(_repo.GetAllMatchVehicles());
            if (matchVehicles == null || matchVehicles.Count() < 1)
                _errorStatus = 17;

            return await Task.FromResult(_errorStatus);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
