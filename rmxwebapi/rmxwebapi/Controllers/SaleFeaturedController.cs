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
    public class SaleFeaturedController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public SaleFeaturedController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }
        public async Task<IEnumerable<SaleFeaturedViewModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            var locations = await Task.FromResult(_repo.GetAllLocations());

            var filterVehicles = vehicles.Where(x => x.ShowForSale == true && x.SoldOn == null && x.ForSaleOn <= DateTime.Now);

            var saleVehicles = from m in medias
                                join v in filterVehicles on m.Vehicle equals v.Oid
                                select new SaleFeaturedViewModel
                                {
                                    VehicleKey = v.QuickFindKeyWord,
                                    Name = v.NameOnWeb,
                                    Location = v.Location,
                                    LocationName = v.LocationName,
                                    SalePrice = v.SalePrice,
                                    ForSaleDesc = v.ForSaleDesc,
                                    VehicleClass = v.VehicleClass,
                                    WebPriceGroup = v.WebPriceGroup,
                                    WebForSaleSellerName = v.WebForSaleSellerName,
                                    WebUniqueId = v.WebUniqueId,
                                    InsurancePolicy = v.InsurancePolicy,
                                    Photo = m.PhotoInfo.Photo,
                                    IsSquarePhoto = m.PhotoInfo.IsSquarePhoto
                                };

            var sortedSaleVehicles = (from s in saleVehicles
                                        join l in locations on s.Location equals l.Oid
                                        select new SaleFeaturedViewModel
                                        {
                                            VehicleKey = s.VehicleKey,
                                            Name = s.Name,
                                            Location = s.Location,
                                            LocationName = s.LocationName,
                                            SalePrice = s.SalePrice,
                                            ForSaleDesc = s.ForSaleDesc,
                                            VehicleClass = s.VehicleClass,
                                            WebPriceGroup = s.WebPriceGroup,
                                            WebForSaleSellerName = s.WebForSaleSellerName,
                                            WebUniqueId = s.WebUniqueId,
                                            InsurancePolicy = s.InsurancePolicy,
                                            Photo = s.Photo,
                                            IsSquarePhoto = s.IsSquarePhoto,
                                            PrimaryPhone = l.PrimaryPhone,
                                            PrimaryEmail = l.PrimaryEmail,
                                            WebTopPerformerOrder = l.WebTopPerformerOrder
                                        }).OrderBy(x => x.WebTopPerformerOrder);

            var result = (from s in sortedSaleVehicles
                          join c in classes on s.VehicleClass equals c.Oid
                          select new SaleFeaturedViewModel
                          {
                              VehicleKey = s.VehicleKey,
                              Name = s.Name,
                              Location = s.Location,
                              LocationName = s.LocationName,
                              SalePrice = s.SalePrice,
                              ForSaleDesc = s.ForSaleDesc,
                              VehicleClass = s.VehicleClass,
                              WebPriceGroup = s.WebPriceGroup,
                              WebForSaleSellerName = s.WebForSaleSellerName,
                              WebUniqueId = s.WebUniqueId,
                              InsurancePolicy = s.InsurancePolicy,
                              Photo = s.Photo,
                              IsSquarePhoto = s.IsSquarePhoto,
                              PrimaryPhone = s.PrimaryPhone,
                              PrimaryEmail = s.PrimaryEmail,
                              WebTopPerformerOrder = s.WebTopPerformerOrder,
                              ClassType = c.ClassType,
                              ClassName = c.Name,
                          });

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<SaleFeaturedViewModel>> Get(long id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var medias = await Task.FromResult(_repo.GetAllVehicleMediaItems());
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var filterVehicles = vehicles.Where(x => x.ShowForSale == true && x.SoldOn == null && x.ForSaleOn <= DateTime.Now);

            var locationOid = locations.FirstOrDefault(x => x.SequenceId == id).Oid;

            var saleVehicles = from m in medias
                                join v in filterVehicles on m.Vehicle equals v.Oid
                                select new SaleFeaturedViewModel
                                {
                                    VehicleKey = v.QuickFindKeyWord,
                                    Name = v.NameOnWeb,
                                    Location = v.Location,
                                    LocationName = v.LocationName,
                                    SalePrice = v.SalePrice,
                                    ForSaleDesc = v.ForSaleDesc,
                                    VehicleClass = v.VehicleClass,
                                    WebPriceGroup = v.WebPriceGroup,
                                    WebForSaleSellerName = v.WebForSaleSellerName,
                                    WebUniqueId = v.WebUniqueId,
                                    InsurancePolicy = v.InsurancePolicy,
                                    Photo = m.PhotoInfo.Photo,
                                    IsSquarePhoto = m.PhotoInfo.IsSquarePhoto
                                };

            var sortedSaleVehicles = (from s in saleVehicles
                                        join l in locations on s.Location equals l.Oid
                                        select new SaleFeaturedViewModel
                                        {
                                            VehicleKey = s.VehicleKey,
                                            Name = s.Name,
                                            Location = s.Location,
                                            LocationName = s.LocationName,
                                            SalePrice = s.SalePrice,
                                            ForSaleDesc = s.ForSaleDesc,
                                            VehicleClass = s.VehicleClass,
                                            WebPriceGroup = s.WebPriceGroup,
                                            WebForSaleSellerName = s.WebForSaleSellerName,
                                            WebUniqueId = s.WebUniqueId,
                                            InsurancePolicy = s.InsurancePolicy,
                                            Photo = s.Photo,
                                            IsSquarePhoto = s.IsSquarePhoto,
                                            PrimaryPhone = l.PrimaryPhone,
                                            PrimaryEmail = l.PrimaryEmail,
                                            WebTopPerformerOrder = l.WebTopPerformerOrder
                                        }).OrderBy(x => x.WebTopPerformerOrder);

            var result = (from s in sortedSaleVehicles
                          join c in classes on s.VehicleClass equals c.Oid
                          select new SaleFeaturedViewModel
                          {
                              VehicleKey = s.VehicleKey,
                              Name = s.Name,
                              Location = s.Location,
                              LocationName = s.LocationName,
                              SalePrice = s.SalePrice,
                              ForSaleDesc = s.ForSaleDesc,
                              VehicleClass = s.VehicleClass,
                              WebPriceGroup = s.WebPriceGroup,
                              WebForSaleSellerName = s.WebForSaleSellerName,
                              WebUniqueId = s.WebUniqueId,
                              InsurancePolicy = s.InsurancePolicy,
                              Photo = s.Photo,
                              IsSquarePhoto = s.IsSquarePhoto,
                              PrimaryPhone = s.PrimaryPhone,
                              PrimaryEmail = s.PrimaryEmail,
                              WebTopPerformerOrder = s.WebTopPerformerOrder,
                              ClassType = c.ClassType,
                              ClassName = c.Name,
                          });
            var list = result.Where(x => x.Location == locationOid).ToList();

            return await Task.FromResult(list);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        public string Get(long id)
        {
            return _utility._noAuthorizationMessage;
        }

        private List<string> GetAvailableDateTime(List<DateTime> list)
        {
            var stringList = new List<string>();
            foreach (var item in list)
            {
                stringList.Add(string.Format("{0:yyyy-MM-dd}", item));
            }
            return stringList;
        }
    }
}
