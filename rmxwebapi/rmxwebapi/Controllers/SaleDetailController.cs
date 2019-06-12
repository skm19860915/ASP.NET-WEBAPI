using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class SaleDetailController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public SaleDetailController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<VehicleSaleDetailViewModel> Get(string id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            if (classes == null || classes.Count() < 1)
                return null;

            var vehicle = vehicles.FirstOrDefault(x => x.WebUniqueId == id && x.ShowForSale == true && x.SoldOn == null);
            if (vehicle == null)
                return null;

            var location = locations.FirstOrDefault(x => x.Oid == vehicle.Location);
            var vehicleClass = classes.FirstOrDefault(x => x.Oid == vehicle.VehicleClass);
            var medias = await Task.FromResult(_repo.GetAllMediaList().Where(x => x.Vehicle == vehicle.Oid).ToList());
            var amenities = await Task.FromResult(_repo.GetAllAmenities());
            var vehiclesAmenities = await Task.FromResult(_repo.GetAllVehiclesAmenities().Where(x => x.Vehicles == vehicle.Oid));

            var amenitiesOfVehicle = (from v in vehiclesAmenities
                                        join a in amenities on v.Amenities equals a.Oid
                                        select new AmenityViewModel
                                        {
                                            AmenityName = a.Name
                                        }).ToList();

            var model = new VehicleSaleDetailViewModel()
            {
                Oid = vehicle.Oid,
                QuickFindKeyWord = vehicle.QuickFindKeyWord,
                NameOnWeb = vehicle.NameOnWeb,
                YouTubeID = vehicle.YouTubeID,
                NoTowing = vehicle.NoTowing,
                Belts = vehicle.Belts,
                Length = vehicle.Length,
                NoDogs = vehicle.NoDogs,
                FuelType = vehicle.FuelType,
                Model = vehicle.Model,
                SmokingAllowed = vehicle.SmokingAllowed,
                Make = vehicle.Make,
                Year = vehicle.Year,
                Children = vehicle.Children,
                Adolescents = vehicle.Adolescents,
                Adults = vehicle.Adults,
                WebDescription = vehicle.WebDescription,
                OtherCostsDesc = vehicle.OtherCostsDesc,
                ForSaleDesc = vehicle.ForSaleDesc,
                SalePrice = vehicle.SalePrice,
                WebUniqueId = vehicle.WebUniqueId,
                InsurancePolicy = vehicle.InsurancePolicy,
                PrimaryPhone = location.PrimaryPhone,
                PrimaryEmail = location.PrimaryEmail,
                ClassName = vehicleClass.Name,
                MediaList = medias,
                AmenityList = amenitiesOfVehicle,
            };

            return await Task.FromResult(model);
        }

        public string Get(string id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
