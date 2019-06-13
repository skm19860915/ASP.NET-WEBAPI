using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class FeaturedVehicleController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;
        public FeaturedVehicleController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<IEnumerable<FeaturedVehicleViewModel>> Get(long id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var targetLocation = locations.FirstOrDefault(x => x.SequenceId == id);
            if (targetLocation == null)
                return null;

            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            if (classes == null || classes.Count() < 1)
                return null;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            if (medias == null || medias.Count() < 1)
                return null;

            var rentals = await Task.FromResult(_repo.GetAllRentals());

            var vehiclesInTargetLocation = vehicles.Where(x => x.Location == targetLocation.Oid);
            if (vehiclesInTargetLocation == null || vehiclesInTargetLocation.Count() < 1)
                return null;

            var mediaJoinVehicle = from m in medias
                                   join v in vehiclesInTargetLocation on m.Vehicle equals v.Oid
                                   select new FeaturedVehicleViewModel
                                   {
                                       VehicleOid = v.Oid,
                                       VehicleKey = v.QuickFindKeyWord,
                                       Name = v.NameOnWeb,
                                       Price = v.DailyRate,
                                       VehicleClass = v.VehicleClass,
                                       WebPriceGroup = v.WebPriceGroup,
                                       VehicleSequenceId = v.SequenceId,
                                       Adolescents = v.Adolescents,
                                       Adults = v.Adults,
                                       Children = v.Children,
                                       WebCleaningFee = v.WebCleaningFee,
                                       WebPrepFee = v.WebPrepFee,
                                       WebRefundableSecurityDeposit = v.WebRefundableSecurityDeposit,
                                       WebGeneratorFreeHours = v.WebGeneratorFreeHours,
                                       WebIncludesTheseMilesFreePerDay = v.WebIncludesTheseMilesFreePerDay,
                                       WebGeneratorAddHoursEach = v.WebGeneratorAddHoursEach,
                                       WebMinimumNumberOfTimeInterval = v.WebMinimumNumberOfTimeInterval,
                                       FeaturedVehicle = v.FeaturedVehicle,
                                       HigherRate = GenerateHigherRate(v.DailyRate),
                                       Make = v.Make,
                                       Model = v.Model,
                                       WebDescription = v.WebDescription,
                                       WebEventPriceForPickedUp = v.WebEventPriceForPickedUp,
                                       WebEventPriceForDelivered = v.WebEventPriceForDelivered,
                                       WebEventPriceForDelivered2 = v.WebEventPriceForDelivered2,
                                       WebEventDescription = v.WebEventDescription,
                                       WebUniqueId = v.WebUniqueId,
                                       InsurancePolicy = v.InsurancePolicy,
                                       ShowForSale = v.ShowForSale,
                                       ForSaleOn = v.ForSaleOn,
                                       SoldOn = v.SoldOn,
                                       SalePrice = v.SalePrice,
                                       Photo = GetAvailablePhoto(m.PhotoInfo.Photo),
                                       IsSquarePhoto = m.PhotoInfo.IsSquarePhoto,
                                   };

            var vehicleJoinClass = from m in mediaJoinVehicle
                                   join c in classes on m.VehicleClass equals c.Oid
                                   select new FeaturedVehicleViewModel
                                   {
                                       VehicleOid = m.VehicleOid,
                                       VehicleKey = m.VehicleKey,
                                       Name = m.Name,
                                       Price = m.Price,
                                       VehicleClass = m.VehicleClass,
                                       WebPriceGroup = m.WebPriceGroup,
                                       VehicleSequenceId = m.VehicleSequenceId,
                                       Adolescents = m.Adolescents,
                                       Adults = m.Adults,
                                       Children = m.Children,
                                       WebCleaningFee = m.WebCleaningFee,
                                       WebPrepFee = m.WebPrepFee,
                                       WebRefundableSecurityDeposit = m.WebRefundableSecurityDeposit,
                                       WebGeneratorFreeHours = m.WebGeneratorFreeHours,
                                       WebGeneratorAddHoursEach = m.WebGeneratorAddHoursEach,
                                       WebIncludesTheseMilesFreePerDay = m.WebIncludesTheseMilesFreePerDay,
                                       WebMinimumNumberOfTimeInterval = m.WebMinimumNumberOfTimeInterval,
                                       FeaturedVehicle = m.FeaturedVehicle,
                                       HigherRate = m.HigherRate,
                                       Make = m.Make,
                                       Model = m.Model,
                                       WebDescription = m.WebDescription,
                                       WebEventPriceForPickedUp = m.WebEventPriceForPickedUp,
                                       WebEventPriceForDelivered = m.WebEventPriceForDelivered,
                                       WebEventPriceForDelivered2 = m.WebEventPriceForDelivered2,
                                       WebEventDescription = m.WebEventDescription,
                                       WebUniqueId = m.WebUniqueId,
                                       InsurancePolicy = m.InsurancePolicy,
                                       ShowForSale = m.ShowForSale,
                                       ForSaleOn = m.ForSaleOn,
                                       SoldOn = m.SoldOn,
                                       SalePrice = m.SalePrice,
                                       Photo = m.Photo,
                                       IsSquarePhoto = m.IsSquarePhoto,
                                       ClassType = c.ClassType,
                                       ClassName = c.Name,
                                       Location = targetLocation.Oid,
                                       LocationName = targetLocation.LocationName,
                                       WebLocationPageHTML = targetLocation.WebLocationPageHTML,
                                       WebLocationPageHTMLBottom = targetLocation.WebLocationPageHTMLBottom,
                                       WebRegionalName = targetLocation.WebRegionalName,
                                       CalcByNights = targetLocation.CalcByNights,
                                       MinimumNumberOfTimeInterval = targetLocation.MinimumNumberOfTimeInterval,
                                       IsLocationBehindOnPaying = targetLocation.IsLocationBehindOnPaying
                                   };

            var list = new List<FeaturedVehicleViewModel>();
            foreach (var item in vehicleJoinClass)
            {
                var record = new FeaturedVehicleViewModel()
                {
                    VehicleOid = item.VehicleOid,
                    VehicleKey = item.VehicleKey,
                    Name = item.Name,
                    Price = item.Price,
                    VehicleClass = item.VehicleClass,
                    WebPriceGroup = item.WebPriceGroup,
                    VehicleSequenceId = item.VehicleSequenceId,
                    Adolescents = item.Adolescents,
                    Adults = item.Adults,
                    Children = item.Children,
                    WebCleaningFee = item.WebCleaningFee,
                    WebPrepFee = item.WebPrepFee,
                    WebRefundableSecurityDeposit = item.WebRefundableSecurityDeposit,
                    WebGeneratorFreeHours = item.WebGeneratorFreeHours,
                    WebGeneratorAddHoursEach = item.WebGeneratorAddHoursEach,
                    WebIncludesTheseMilesFreePerDay = item.WebIncludesTheseMilesFreePerDay,
                    WebMinimumNumberOfTimeInterval = item.WebMinimumNumberOfTimeInterval,
                    FeaturedVehicle = item.FeaturedVehicle,
                    HigherRate = item.HigherRate,
                    Make = item.Make,
                    Model = item.Model,
                    WebDescription = item.WebDescription,
                    WebEventPriceForPickedUp = item.WebEventPriceForPickedUp,
                    WebEventPriceForDelivered = item.WebEventPriceForDelivered,
                    WebEventPriceForDelivered2 = item.WebEventPriceForDelivered2,
                    WebEventDescription = item.WebEventDescription,
                    WebUniqueId = item.WebUniqueId,
                    InsurancePolicy = item.InsurancePolicy,
                    ShowForSale = item.ShowForSale,
                    ForSaleOn = item.ForSaleOn,
                    SoldOn = item.SoldOn,
                    SalePrice = item.SalePrice,
                    Photo = item.Photo,
                    IsSquarePhoto = item.IsSquarePhoto,
                    ClassType = item.ClassType,
                    ClassName = item.ClassName,
                    Location = item.Location,
                    LocationName = item.LocationName,
                    WebLocationPageHTML = item.WebLocationPageHTML,
                    WebLocationPageHTMLBottom = item.WebLocationPageHTMLBottom,
                    WebRegionalName = item.WebRegionalName,
                    CalcByNights = item.CalcByNights,
                    MinimumNumberOfTimeInterval = item.MinimumNumberOfTimeInterval,
                    IsLocationBehindOnPaying = item.IsLocationBehindOnPaying,
                    DateList = GetAllDateArray(rentals == null || rentals.Count() < 1 ? null : rentals.Where(x => x.Vehicle == item.VehicleOid).ToList())
                };
                list.Add(record);
            }

            var result = list.OrderByDescending(x => x.VehicleKey);

            return await Task.FromResult(result);
        }

        public string Get(long id)
        {
            return _utility._noAuthorizationMessage;
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

        private string GetAvailablePhoto(byte[] photo)
        {
            if (photo == null)
                return null;

            if (photo.Length == 0) // new byte[0]
                return "big";

            var validPhoto = string.Format("{0}", Convert.ToBase64String(photo));
            return validPhoto;
        }

        private List<string> GetAllDateArray(List<RentalModel> rentals)
        {
            if (rentals == null)
                return null;

            var dateList = new List<DateTime>();
            foreach (var item in rentals)
            {
                dateList.AddRange(GetDateList(item.LeaveOn ?? DateTime.Now, item.ReturnOn ?? DateTime.Now));
            }
            var result = CustomizeAllDateArray(dateList);
            return result;
        }

        private List<DateTime> GetDateList(DateTime startDate, DateTime endDate)
        {
            var days = (endDate - startDate).Days + 1;
            var range = Enumerable.Range(0, days).Select(i => startDate.AddDays(i)).ToList();
            return range;
        }

        private List<string> CustomizeAllDateArray(List<DateTime> dateList)
        {
            var stringList = new List<string>();
            foreach (var item in dateList)
            {
                stringList.Add(string.Format("{0:yyyy-MM-dd}", item));
            }
            return stringList;
        }
    }
}
