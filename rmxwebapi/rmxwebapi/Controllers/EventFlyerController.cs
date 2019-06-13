using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    // eventflyer of sharemycoach
    public class EventFlyerController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public EventFlyerController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        // get tiger's eventflyer values
        public async Task<IEnumerable<EventFlyerViewModel>> Get(string id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            if (string.IsNullOrEmpty(id))
                return null;

            var param = GetUsuableParameters(id);

            var vehicles = await Task.FromResult(_repo.GetAllEventVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            if (classes == null || classes.Count() < 1)
                return null;

            var medias = await Task.FromResult(_repo.GetAllMediaList());
            if (medias == null || medias.Count() < 1)
                return null;

            var validMedias = medias.Where(x => x.PhotoCode == 0 || x.PhotoCode == 1 || x.PhotoCode == 5).ToList();
            var vehiclesWithTag = vehicles.Where(x => x.Tag.Contains(param[0]));

            var targetVehicleInfo = GetTargetVehicleInfo(param[1], vehicles.ToList());
            if (targetVehicleInfo == null)
                return null;

            var eventClasses = classes.FirstOrDefault(x => x.Oid == targetVehicleInfo.VehicleClass);
            var eventLocation = locations.FirstOrDefault(x => x.Oid == targetVehicleInfo.Location);
            var eventVehicles = vehiclesWithTag.Where(x => x.VehicleClass == targetVehicleInfo.VehicleClass && x.Location == targetVehicleInfo.Location).ToList();

            var list = new List<EventFlyerViewModel>();

            foreach (var item in eventVehicles)
            {
                if(string.Compare(targetVehicleInfo.Tag, item.Tag) == 0)
                {
                    var record = new EventFlyerViewModel()
                    {
                        Vehicle = item.Oid,
                        Tag = item.Tag,
                        QuickFindKeyWord = item.QuickFindKeyWord,
                        WebEventPriceForPickedUp = getEventPrice(item.Tag, 1, param[0]),
                        WebEventPriceForDelivered = getEventPrice(item.Tag, 2, param[0]),
                        WebEventPriceForDelivered2 = getEventPrice(item.Tag, 3, param[0]),
                        WebEventPriceForDelivered3 = getEventPrice(item.Tag, 4, param[0]),
                        YouTubeID = item.YouTubeID,
                        DailyRate = item.DailyRate,
                        VehicleClass = eventClasses.Oid,
                        ClassName = eventClasses.Name,
                        ClassType = eventClasses.ClassType,
                        MediaList = GetMediaList(validMedias.Where(x => x.Vehicle == item.Oid).ToList(), medias.ToList(), item.Oid),
                        Location = eventLocation.Oid,
                        LocationName = eventLocation.Name,
                        PrimaryPhone = eventLocation.PrimaryPhone,
                        MobilePhone = eventLocation.MobilePhone,
                        OutgoingUserName = eventLocation.OutgoingUserName,
                        OutgoingServerName = eventLocation.OutgoingServerName,
                        OutgoingPassword = eventLocation.OutgoingPassword,
                        OutgoingServerPort = eventLocation.OutgoingServerPort,
                        PrimaryEmail = eventLocation.PrimaryEmail,
                        SecondaryEmail = eventLocation.SecondaryEmail,
                        DBAName = eventLocation.DBAName,
                        WebRegionalName = eventLocation.WebRegionalName
                    };
                    list.Add(record);
                }
            }

            var result = list.OrderBy(x => x.QuickFindKeyWord);
            return result;
        }

        private decimal? getEventPrice(string tag, int index, string targetDirectory)
        {
            decimal price = 0;

            if (string.IsNullOrEmpty(tag))
                return 0;

            var infos = tag.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var info in infos)
            {
                var parameters = info.Split(',');
                if(string.Equals(parameters[0], targetDirectory))
                {
                    if(parameters.Count() > index)
                    {
                        price = decimal.Parse(parameters[index]);
                        break;
                    }
                }
            }
            return price;
        }

        public string Get(string id)
        {
            return _utility._noAuthorizationMessage;
        }

        private List<VehicleMediaItemModel> GetMediaList(List<VehicleMediaItemModel> validMedias, List<VehicleMediaItemModel> allMedias, Guid vehicleOid)
        {
            if(validMedias == null || validMedias.Count() < 1)
            {
                var otherMedias = allMedias.Where(x => x.Vehicle == vehicleOid).Take(3).ToList();
                return otherMedias;
            }
            return validMedias;
        }

        private EventVehicleModel GetTargetVehicleInfo(string oidToken, List<EventVehicleModel> vehicles)
        {
            EventVehicleModel record = null;
            if (vehicles == null)
                return null;

            if (string.IsNullOrEmpty(oidToken))
                return null;

            foreach(var vehicle in vehicles)
            {
                var targetToken = vehicle.Oid.ToString().Substring(0, 3);
                if(string.Equals(targetToken, oidToken, StringComparison.OrdinalIgnoreCase))
                {
                    record = new EventVehicleModel()
                    {
                        Oid = vehicle.Oid,
                        Location = vehicle.Location,
                        Tag = vehicle.Tag,
                        YouTubeID = vehicle.YouTubeID,
                        VehicleClass = vehicle.VehicleClass,
                        QuickFindKeyWord = vehicle.QuickFindKeyWord,
                        WebEventPriceForPickedUp = vehicle.WebEventPriceForPickedUp,
                        WebEventPriceForDelivered = vehicle.WebEventPriceForDelivered,
                        WebEventPriceForDelivered2 = vehicle.WebEventPriceForDelivered2,
                        WebEventPriceForDelivered3 = vehicle.WebEventPriceForDelivered3
                    };
                    break;
                }
            }
            if(record == null)
            {
                var allVehicles = _repo.GetAllVehicles();
                foreach(var v in allVehicles)
                {
                    var validToken = v.Oid.ToString().Substring(0, 3);
                    if(string.Equals(validToken, oidToken, StringComparison.OrdinalIgnoreCase))
                    {
                        record = new EventVehicleModel()
                        {
                            Oid = v.Oid,
                            Location = v.Location,
                            Tag = v.Tag,
                            YouTubeID = v.YouTubeID,
                            VehicleClass = v.VehicleClass,
                            QuickFindKeyWord = v.QuickFindKeyWord,
                            WebEventPriceForPickedUp = v.WebEventPriceForPickedUp,
                            WebEventPriceForDelivered = v.WebEventPriceForDelivered,
                            WebEventPriceForDelivered2 = v.WebEventPriceForDelivered2,
                            WebEventPriceForDelivered3 = v.WebEventPriceForDelivered3
                        };
                        break;
                    }
                }
            }
            return record;
        }

        private List<RentalModel> GetNoBookedRentals(string year, string startMonth, string startDay, string endMonth, string endDay, List<RentalModel> list)
        {
            var validStartDate = new DateTime(2000 + Convert.ToInt32(year), Convert.ToInt32(startMonth), Convert.ToInt32(startDay), 0, 0, 0);
            var validEndDate = new DateTime(2000 + Convert.ToInt32(year), Convert.ToInt32(endMonth), Convert.ToInt32(endDay), 0, 0, 0);

            var arr = new List<RentalModel>();

            foreach(var item in list)
            {
                var startDate = GetCustomDateTime(item.LeaveOn);
                var endDate = GetCustomDateTime(item.ReturnOn);

                var startResult = DateTime.Compare(validStartDate, startDate);
                var endResult = DateTime.Compare(validEndDate, endDate);

                if(!(startResult <= 0 && endResult >= 0))
                {
                    var record = new RentalModel()
                    {
                        Destination = item.Destination,
                        IsOwnerRenting = item.IsOwnerRenting,
                        LeaveOn = item.LeaveOn,
                        Location = item.Location,
                        ReturnOn = item.ReturnOn,
                        Vehicle = item.Vehicle
                    };
                    arr.Add(record);
                }
            }

            var result = arr.GroupBy(x => x.Vehicle).Select(y => y.FirstOrDefault()).ToList();
            return result;
        }

        private DateTime GetCustomDateTime(DateTime? dateTime)
        {
            var year = dateTime.Value.Year;
            var month = dateTime.Value.Month;
            var day = dateTime.Value.Day;
            var date = new DateTime(year, month, day, 0, 0, 0);
            return date;
        }

        private string[] GetUsuableParameters(string id)
        {
            var param = new string[7];
            if (!string.IsNullOrEmpty(id))
            {
                param[0] = id.Substring(0, 4); // directory name
                param[1] = id.Substring(4, 3); // vehicle_oid_token
                param[2] = id.Substring(7, 2); // year 
                param[3] = id.Substring(9, 2); // start month  
                param[4] = id.Substring(11, 2); // start date 
                param[5] = id.Substring(15, 2); // end month 
                param[6] = id.Substring(17, 2); // end date 

                return param;
            }
            return null;
        }
    }
}
