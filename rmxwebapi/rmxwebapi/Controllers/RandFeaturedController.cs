using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class RandFeaturedController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public RandFeaturedController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }
        public async Task<IEnumerable<RandFeaturedViewModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            if (classes == null || classes.Count() < 1)
                return null;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var featuredVehicles = vehicles.Where(x => x.FeaturedVehicle == true && x.Location != null);
            if (featuredVehicles == null || featuredVehicles.Count() < 1)
                return null;

            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            if (medias == null || medias.Count() < 1)
                return null;

            var random = new Random();

            var mediaJoinVehicle = (from m in medias
                                    join f in featuredVehicles on m.Vehicle equals f.Oid
                                    select new RandFeaturedViewModel
                                    {
                                        VehicleOid = f.Oid,
                                        VehicleKey = f.QuickFindKeyWord,
                                        Name = f.NameOnWeb,
                                        Location = f.Location,
                                        LocationName = f.LocationName,
                                        Price = f.DailyRate,
                                        VehicleClass = f.VehicleClass,
                                        WebPriceGroup = f.WebPriceGroup,
                                        VehicleSequenceId = f.SequenceId,
                                        WebUniqueId = f.WebUniqueId,
                                        InsurancePolicy = f.InsurancePolicy,
                                        Photo = m.PhotoInfo.Photo,
                                        IsSquarePhoto = m.PhotoInfo.IsSquarePhoto
                                    }).GroupBy(x => x.VehicleClass).Select(y => y.ElementAt(random.Next(0, y.Count())));

            var vehicleJoinLocation = (from m in mediaJoinVehicle
                                      join l in locations on m.Location equals l.Oid
                                      select new RandFeaturedViewModel
                                      {
                                          VehicleOid = m.VehicleOid,
                                          VehicleKey = m.VehicleKey,
                                          Name = m.Name,
                                          Location = m.Location,
                                          LocationName = m.LocationName,
                                          Price = m.Price,
                                          VehicleClass = m.VehicleClass,
                                          WebPriceGroup = m.WebPriceGroup,
                                          VehicleSequenceId = m.VehicleSequenceId,
                                          WebUniqueId = m.WebUniqueId,
                                          InsurancePolicy = m.InsurancePolicy,
                                          Photo = m.Photo,
                                          IsSquarePhoto = m.IsSquarePhoto,
                                          WebLocationPageHTML = l.WebLocationPageHTML,
                                          WebLocationPageHTMLBottom = l.WebLocationPageHTMLBottom,
                                          WebTopPerformerOrder = l.WebTopPerformerOrder,
                                          LocationSequenceId = l.SequenceId,
                                          WebRegionalName = l.WebRegionalName,
                                          IsLocationBehindOnPaying = l.IsLocationBehindOnPaying,
                                          City = l.City,
                                          State = l.State
                                      }).OrderBy(x => x.WebTopPerformerOrder);

            var result = from s in vehicleJoinLocation
                         join c in classes on s.VehicleClass equals c.Oid
                         select new RandFeaturedViewModel
                         {
                             VehicleOid = s.VehicleOid,
                             VehicleKey = s.VehicleKey,
                             Name = s.Name,
                             Location = s.Location,
                             LocationName = s.LocationName,
                             Price = s.Price,
                             VehicleClass = s.VehicleClass,
                             WebPriceGroup = s.WebPriceGroup,
                             VehicleSequenceId = s.VehicleSequenceId,
                             WebUniqueId = s.WebUniqueId,
                             InsurancePolicy = s.InsurancePolicy,
                             Photo = s.Photo,
                             IsSquarePhoto = s.IsSquarePhoto,
                             WebTopPerformerOrder = s.WebTopPerformerOrder,
                             LocationSequenceId = s.LocationSequenceId,
                             WebLocationPageHTML = s.WebLocationPageHTML,
                             WebLocationPageHTMLBottom = s.WebLocationPageHTMLBottom,
                             WebRegionalName = s.WebRegionalName,
                             IsLocationBehindOnPaying = s.IsLocationBehindOnPaying,
                             City = s.City,
                             State = s.State,
                             ClassType = c.ClassType,
                             ClassName = c.Name,
                             WebBannerName = c.WebBannerName,
                             SortOrder = c.SortOrder
                         };

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<RandFeaturedViewModel>> Get(long id, string token)
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

            var random = new Random();
            
            var featuredVehicles = vehicles.Where(x => x.FeaturedVehicle == true && x.Location == targetLocation.Oid);
            if (featuredVehicles == null || featuredVehicles.Count() < 1)
                return null;

            var mediaJoinVehicle = (from m in medias
                                   join f in featuredVehicles on m.Vehicle equals f.Oid
                                   select new RandFeaturedViewModel
                                   {
                                       VehicleOid = f.Oid,
                                       VehicleKey = f.QuickFindKeyWord,
                                       Name = f.NameOnWeb,
                                       Price = f.DailyRate,
                                       VehicleClass = f.VehicleClass,
                                       WebPriceGroup = f.WebPriceGroup,
                                       VehicleSequenceId = f.SequenceId,
                                       WebUniqueId = f.WebUniqueId,
                                       InsurancePolicy = f.InsurancePolicy,
                                       Photo = m.PhotoInfo.Photo,
                                       IsSquarePhoto = m.PhotoInfo.IsSquarePhoto
                                   }).GroupBy(x => x.VehicleClass).Select(y => y.ElementAt(random.Next(0, y.Count()))).OrderBy(z => z.WebTopPerformerOrder);

            var result = (from m in mediaJoinVehicle
                         join c in classes on m.VehicleClass equals c.Oid
                         select new RandFeaturedViewModel
                         {
                             VehicleOid = m.VehicleOid,
                             VehicleKey = m.VehicleKey,
                             Name = m.Name,
                             Price = m.Price,
                             VehicleClass = m.VehicleClass,
                             WebPriceGroup = m.WebPriceGroup,
                             VehicleSequenceId = m.VehicleSequenceId,
                             WebUniqueId = m.WebUniqueId,
                             InsurancePolicy = m.InsurancePolicy,
                             Photo = m.Photo,
                             IsSquarePhoto = m.IsSquarePhoto,
                             ClassType = c.ClassType,
                             ClassName = c.Name,
                             WebBannerName = c.WebBannerName,
                             SortOrder = c.SortOrder,
                             Location = targetLocation.Oid,
                             LocationName = targetLocation.LocationName,
                             WebLocationPageHTML = targetLocation.WebLocationPageHTML,
                             WebLocationPageHTMLBottom = targetLocation.WebLocationPageHTMLBottom,
                             City = targetLocation.City,
                             IsLocationBehindOnPaying = targetLocation.IsLocationBehindOnPaying,
                             LocationSequenceId = targetLocation.SequenceId,
                             State = targetLocation.State,
                             WebRegionalName = targetLocation.WebRegionalName,
                             WebTopPerformerOrder = targetLocation.WebTopPerformerOrder
                         }).OrderBy(x => x.SortOrder);

            return await Task.FromResult(result);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        public string Get(long id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
