using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    // both sharemycoach and affiliate system
    public class DetailController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public DetailController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        // affiliate system
        public async Task<VehicleDetailViewModel> Get(string id, long seq, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            var location = locations.FirstOrDefault(x => x.SequenceId == seq);
            if (location == null)
                return null;
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            var vehicle = vehicles.FirstOrDefault(x => x.QuickFindKeyWord == id);
            if (vehicle == null)
                return null;
            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            var galleries = await Task.FromResult(_repo.GetAllMediaList());
            var rentals = await Task.FromResult(_repo.GetAllRentals());
            var amenities = await Task.FromResult(_repo.GetAllAmenities());
            var vehiclesAmenities = await Task.FromResult(_repo.GetAllVehiclesAmenities());
            
            var vehicleClass = classes.FirstOrDefault(x => x.Oid == vehicle.VehicleClass);
            var gallery = galleries.Where(x => x.Vehicle == vehicle.Oid).ToList();
            var targetRentals = rentals.Where(x => x.Vehicle == vehicle.Oid && x.Location == location.Oid).ToList();
            var targetVehiclesAmenities = vehiclesAmenities.Where(x => x.Vehicles == vehicle.Oid).ToList();

            var amenitiesOfVehicle = (from v in targetVehiclesAmenities
                                        join a in amenities on v.Amenities equals a.Oid
                                        select new AmenityViewModel
                                        {
                                            AmenityName = a.Name
                                        }).ToList();
            var vehiclesInLocation = vehicles.Where(x => x.Location == location.Oid && x.NameOnWeb != null && x.VehicleClass != null);
            var vehiclesInLocationWithClass = from v in vehiclesInLocation
                                                  join c in classes on v.VehicleClass equals c.Oid
                                                  select new
                                                  {
                                                      Oid = v.Oid,
                                                      Name = v.NameOnWeb,
                                                      VehicleKey = v.QuickFindKeyWord,  
                                                      LocationName = v.LocationName,
                                                      VehicleSequenceId = v.SequenceId,
                                                      Price = v.DailyRate,
                                                      WebUniqueId = v.WebUniqueId,
                                                      InsurancePolicy = v.InsurancePolicy,
                                                      ClassType = c.ClassType ?? 0,
                                                      ClassName = c.Name
                                                  };
            var vehiclesWithMedia = from s in vehiclesInLocationWithClass
                                      join m in medias on s.Oid equals m.Vehicle
                                      select new
                                      {
                                          Name = s.Name,
                                          VehicleKey = s.VehicleKey,
                                          LocationName = s.LocationName,
                                          VehicleSequenceId = s.VehicleSequenceId,
                                          Price = s.Price,
                                          WebUniqueId = s.WebUniqueId,
                                          InsurancePolicy = s.InsurancePolicy,
                                          ClassType = s.ClassType,
                                          ClassName = s.ClassName,
                                          Photo = m.PhotoInfo.Photo
                                      };
            var validVehicles = new List<VehiclesInLocationViewModel>();
            var randomOtherVehicles = new List<RandOtherFeaturedViewModel>();
            foreach (var item in vehiclesWithMedia)
            {
                if (item.ClassType == vehicleClass.ClassType && item.VehicleKey != vehicle.QuickFindKeyWord)
                {
                    var randomOtherVehicleRecord = new RandOtherFeaturedViewModel()
                    {
                        Name = item.Name,
                        LocationName = item.LocationName,
                        Photo = item.Photo,
                        Price = item.Price,
                        VehicleSequenceId = item.VehicleSequenceId,
                        InsurancePolicy = item.InsurancePolicy,
                        WebUniqueId = item.WebUniqueId
                    };
                    randomOtherVehicles.Add(randomOtherVehicleRecord);
                }

                var vehicleRecord = new VehiclesInLocationViewModel()
                {
                    Name = item.Name,
                    VehicleKey = item.VehicleKey,
                    ClassType = item.ClassType,
                    ClassName = item.ClassName,
                    WebUniqueId = item.WebUniqueId,
                    InsurancePolicy = item.InsurancePolicy,
                };

                validVehicles.Add(vehicleRecord);
            }

            var model = new VehicleDetailViewModel()
            {
                Oid = vehicle.Oid,
                WebUniqueId = vehicle.WebUniqueId,
                QuickFindKeyWord = vehicle.QuickFindKeyWord,
                Location = vehicle.Location,
                LocationName = vehicle.LocationName,
                VehicleClass = vehicle.VehicleClass,
                NameOnWeb = vehicle.NameOnWeb,
                DailyRate = vehicle.DailyRate,
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
                VehicleSequenceId = vehicle.SequenceId,
                WebPrepFee = vehicle.WebPrepFee,
                WebCleaningFee = vehicle.WebCleaningFee,
                WebRefundableSecurityDeposit = vehicle.WebRefundableSecurityDeposit,
                WebGeneratorFreeHours = vehicle.WebGeneratorFreeHours,
                WebIncludesTheseMilesFreePerDay = vehicle.WebIncludesTheseMilesFreePerDay,
                HigherRate = GenerateHigherRate(vehicle.DailyRate),
                WebEventPriceForPickedUp = vehicle.WebEventPriceForPickedUp,
                WebEventPriceForDelivered = vehicle.WebEventPriceForDelivered,
                WebEventPriceForDelivered2 = vehicle.WebEventPriceForDelivered2,
                WebEventDescription = vehicle.WebEventDescription,
                WebTransportedID = vehicle.WebTransportedID,
                InsurancePolicy = vehicle.InsurancePolicy,
                isShowCalendarOnWeb = location.isShowCalendarOnWeb,
                IsCalendarWithBookings = location.IsCalendarWithBookings,
                PrimaryPhone = location.PrimaryPhone,
                DBAName = location.DBAName,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Zip = location.Zip,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                WebGoogleMapJavaScriptAPIKey = location.WebGoogleMapJavaScriptAPIKey,
                IsLocationBehindOnPaying = location.IsLocationBehindOnPaying,
                LocationSequenceId = location.SequenceId,
                Organization = location.Organization,
                OrganizationName = location.OrganizationName,
                OutgoingUserName = location.OutgoingUserName,
                OutgoingServerName = location.OutgoingServerName,
                OutgoingServerPort = location.OutgoingServerPort,
                OutgoingPassword = location.OutgoingPassword,
                EmailAddress = location.EmailAddress,
                WebRegionalName = location.WebRegionalName,
                MinimumNumberOfTimeInterval = location.MinimumNumberOfTimeInterval,
                CalcByNights = location.CalcByNights,
                WebQuoteEmailAddress = location.WebQuoteEmailAddress,
                ClassOid = vehicleClass.Oid,
                ClassName = vehicleClass.Name,
                ClassType = vehicleClass.ClassType,
                MediaList = gallery,
                RentalList = targetRentals,
                AmenityList = amenitiesOfVehicle,
                VehicleList = validVehicles.OrderBy(x => x.ClassName).ToList(),
                RandOtherVehicleList = GetRandOtherFeaturedList(randomOtherVehicles)
            };

            return await Task.FromResult(model);
        }

        // sharemycoach
        public async Task<VehicleDetailViewModel> Get(string id, bool isChanged, string token)
        {
            VehicleModel vehicle = null;

            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if(isChanged)
                vehicle = vehicles.FirstOrDefault(x => x.WebUniqueId == getMatchId(x.WebUniqueId, id));
            else
                vehicle = vehicles.FirstOrDefault(x => x.WebUniqueId == id);

            if (vehicle == null)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            var galleries = await Task.FromResult(_repo.GetAllMediaList());
            var rentals = await Task.FromResult(_repo.GetAllRentals());
            var amenities = await Task.FromResult(_repo.GetAllAmenities());
            var vehiclesAmenities = await Task.FromResult(_repo.GetAllVehiclesAmenities());

            var location = locations.FirstOrDefault(x => x.Oid == vehicle.Location);
            var vehicleClass = classes.FirstOrDefault(x => x.Oid == vehicle.VehicleClass);
            var gallery = galleries.Where(x => x.Vehicle == vehicle.Oid).ToList();
            var targetRentals = rentals.Where(x => x.Vehicle == vehicle.Oid && x.Location == location.Oid).ToList();
            var targetVehiclesAmenities = vehiclesAmenities.Where(x => x.Vehicles == vehicle.Oid).ToList();

            var amenitiesOfVehicle = (from v in targetVehiclesAmenities
                                        join a in amenities on v.Amenities equals a.Oid
                                        select new AmenityViewModel
                                        {
                                            AmenityName = a.Name
                                        }).ToList();
            var vehiclesInLocation = vehicles.Where(x => x.Location == location.Oid && x.NameOnWeb != null && x.VehicleClass != null);
            var vehiclesInLocationWithClass = from v in vehiclesInLocation
                                                  join c in classes on v.VehicleClass equals c.Oid
                                                  select new
                                                  {
                                                      Oid = v.Oid,
                                                      Name = v.NameOnWeb,
                                                      VehicleKey = v.QuickFindKeyWord,
                                                      LocationName = v.LocationName,
                                                      VehicleSequenceId = v.SequenceId,
                                                      Price = v.DailyRate,
                                                      WebUniqueId = v.WebUniqueId,
                                                      InsurancePolicy = v.InsurancePolicy,
                                                      ClassType = c.ClassType ?? 0,
                                                      ClassName = c.Name
                                                  };
            var vehiclesWithMedia = from s in vehiclesInLocationWithClass
                                      join m in medias on s.Oid equals m.Vehicle
                                      select new
                                      {
                                          Name = s.Name,
                                          VehicleKey = s.VehicleKey,
                                          LocationName = s.LocationName,
                                          VehicleSequenceId = s.VehicleSequenceId,
                                          Price = s.Price,
                                          WebUniqueId = s.WebUniqueId,
                                          InsurancePolicy = s.InsurancePolicy,
                                          ClassType = s.ClassType,
                                          ClassName = s.ClassName,
                                          Photo = m.PhotoInfo.Photo,
                                          IsSquarePhoto = m.PhotoInfo.IsSquarePhoto
                                      };
            var validVehicles = new List<VehiclesInLocationViewModel>();
            var randomOtherVehicles = new List<RandOtherFeaturedViewModel>();
            foreach (var item in vehiclesWithMedia)
            {
                if (item.ClassType == vehicleClass.ClassType && item.VehicleKey != vehicle.QuickFindKeyWord)
                {
                    var randomOtherVehicleRecord = new RandOtherFeaturedViewModel()
                    {
                        Name = item.Name,
                        LocationName = item.LocationName,
                        Photo = item.Photo,
                        Price = item.Price,
                        VehicleSequenceId = item.VehicleSequenceId,
                        InsurancePolicy = item.InsurancePolicy,
                        WebUniqueId = item.WebUniqueId,
                        IsSquarePhoto = item.IsSquarePhoto
                    };
                    randomOtherVehicles.Add(randomOtherVehicleRecord);
                }

                var vehicleRecord = new VehiclesInLocationViewModel()
                {
                    Name = item.Name,
                    VehicleKey = item.VehicleKey,
                    ClassType = item.ClassType,
                    ClassName = item.ClassName,
                    WebUniqueId = item.WebUniqueId,
                    InsurancePolicy = item.InsurancePolicy
                };

                validVehicles.Add(vehicleRecord);
            }

            var model = new VehicleDetailViewModel()
            {
                Oid = vehicle.Oid,
                WebUniqueId = vehicle.WebUniqueId,
                QuickFindKeyWord = vehicle.QuickFindKeyWord,
                Location = vehicle.Location,
                LocationName = vehicle.LocationName,
                VehicleClass = vehicle.VehicleClass,
                NameOnWeb = vehicle.NameOnWeb,
                DailyRate = vehicle.DailyRate,
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
                VehicleSequenceId = vehicle.SequenceId,
                WebPrepFee = vehicle.WebPrepFee,
                WebCleaningFee = vehicle.WebCleaningFee,
                WebRefundableSecurityDeposit = vehicle.WebRefundableSecurityDeposit,
                WebGeneratorFreeHours = vehicle.WebGeneratorFreeHours,
                WebIncludesTheseMilesFreePerDay = vehicle.WebIncludesTheseMilesFreePerDay,
                HigherRate = GenerateHigherRate(vehicle.DailyRate),
                WebEventPriceForPickedUp = vehicle.WebEventPriceForPickedUp,
                WebEventPriceForDelivered = vehicle.WebEventPriceForDelivered,
                WebEventPriceForDelivered2 = vehicle.WebEventPriceForDelivered2,
                WebEventDescription = vehicle.WebEventDescription,
                WebTransportedID = vehicle.WebTransportedID,
                InsurancePolicy = vehicle.InsurancePolicy,
                OldVehicleID = vehicle.OldVehicleID,
                ShowForSale = vehicle.ShowForSale,
                ForSaleOn = vehicle.ForSaleOn,
                SoldOn = vehicle.SoldOn,
                SalePrice = vehicle.SalePrice,
                isShowCalendarOnWeb = location.isShowCalendarOnWeb,
                IsCalendarWithBookings = location.IsCalendarWithBookings,
                PrimaryPhone = location.PrimaryPhone,
                DBAName = location.DBAName,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Zip = location.Zip,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                WebGoogleMapJavaScriptAPIKey = location.WebGoogleMapJavaScriptAPIKey,
                IsLocationBehindOnPaying = location.IsLocationBehindOnPaying,
                LocationSequenceId = location.SequenceId,
                Organization = location.Organization,
                OrganizationName = location.OrganizationName,
                OutgoingUserName = location.OutgoingUserName,
                OutgoingServerName = location.OutgoingServerName,
                OutgoingServerPort = location.OutgoingServerPort,
                OutgoingPassword = location.OutgoingPassword,
                EmailAddress = location.EmailAddress,
                WebRegionalName = location.WebRegionalName,
                MinimumNumberOfTimeInterval = location.MinimumNumberOfTimeInterval,
                CalcByNights = location.CalcByNights,
                WebQuoteEmailAddress = location.WebQuoteEmailAddress,
                EmailSupportRequestAddress = location.EmailSupportRequestAddress,
                FriendlyCompanyName = location.FriendlyCompanyName,
                ClassOid = vehicleClass.Oid,
                ClassName = vehicleClass.Name,
                ClassType = vehicleClass.ClassType,
                MediaList = gallery,
                RentalList = targetRentals,
                AmenityList = amenitiesOfVehicle,
                VehicleList = validVehicles.OrderBy(x => x.ClassName).ToList(),
                RandOtherVehicleList = GetRandOtherFeaturedList(randomOtherVehicles)
            };

            return await Task.FromResult(model);
        }

        private string getMatchId(string webUniqueId, string id)
        {
            var strFirstArray = webUniqueId.Split('_');
            var str1 = strFirstArray.First().Replace("-", string.Empty);

            var strSecondArray = id.Split('_');
            var str2 = strSecondArray.First().Replace("-", string.Empty);

            int isEqual = string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
            if (isEqual == 0)
                return webUniqueId;

            return string.Empty;
        }

        public string Get(string id, long seq)
        {
            return _utility._noAuthorizationMessage;
        }

        public string Get(string id, bool isChanged)
        {
            return _utility._noAuthorizationMessage;
        }

        private List<RandOtherFeaturedViewModel> GetRandOtherFeaturedList(List<RandOtherFeaturedViewModel> list)
        {
            if (list.Count() > 4)
            {
                var rand = new Random();
                var count = list.Count();
                while (count > 1)
                {
                    count--;
                    var x = rand.Next(count + 1);
                    RandOtherFeaturedViewModel model = list[x];
                    list[x] = list[count];
                    list[count] = model;
                }
                
                var result = list.Take(4);
                return result.ToList();
            }
            return list;
        }

        private decimal? GenerateHigherRate(string dailyRate)
        {
            if (string.IsNullOrEmpty(dailyRate))
                return 0;

            var split = dailyRate.Split('-').ToArray();
            string element = null;
            if (split.Count() == 1)
                element = split[0];
            else
                element = split[1];

            var result = Regex.Match(element, @"\d+");
            try
            {
                var value = Convert.ToDecimal(result.ToString());
                return value;
            }
            catch
            {
                return 0;
            }
        }
    }
}
