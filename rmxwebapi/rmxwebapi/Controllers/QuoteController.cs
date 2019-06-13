using rmxwebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class QuoteController : ApiController
    {
        private DataRepository _repo;

        public QuoteController()
        {
            _repo = new DataRepository();
        }

        [HttpPost]
        public IHttpActionResult Post(QuoteModel quote)
        {
            var oid = quote.Oid;
            if (oid == null)
                return null;

            var organizationOid = GetGuidFromString(quote.Organization);
            var locationOid = GetGuidFromString(quote.Location);
            var organizationName = quote.OrganizationName;
            var locationName = quote.LocationName;
            var name = quote.Name;
            var firstName = quote.FirstName;
            var lastName = quote.LastName;
            var address = quote.Address;
            var city = quote.City;
            var state = quote.State;
            var zip = quote.Zip;
            var primaryPhone = quote.MobilePhonePrimary;
            var secondaryPhone = quote.HomePhoneSecondary;
            var emailAddress = quote.EmailAddress;
            var leaveOn = quote.LeaveOn;
            var returnOn = quote.ReturnOn;
            var destination = quote.Destination;
            var distance = quote.Distance;
            var adults = quote.Adults;
            var children = quote.Children;
            var webUserComments = quote.WebUserComments;
            var leadSourceOid = GetGuidFromString(quote.LeadSourceOid);
            var webUserSelectedClass = quote.WebUserSelectedClass;
            var classOid = GetGuidFromString(quote.ClassOid);
            var vehicleName = quote.VehicleName;
            var vehicleOid = GetGuidFromString(quote.VehicleId);
            var comments = quote.Comments;
            var country = quote.WebUserSelectedCountry;
            var equipments = quote.EquipmentTypeOids;
            var fees = quote.FeeOids;
            var insuranceCompanyName = GetValueFromString(quote.LocationInsuranceCompanyOidsAndNames);
            var insuranceCompanyOid = GetOidFromString(quote.LocationInsuranceCompanyOidsAndNames);

            var model = new WorkspaceWebQuoteModel();
            model.Oid = oid;
            model.IsSystemObject = false;
            model.Organization = organizationOid;
            model.Location = locationOid;
            model.OrganizationName = organizationName;
            model.LocationName = locationName;
            model.IsActive = true;
            model.CreatedOn = DateTime.Now;
            model.Name = name;
            model.First = firstName;
            model.Last = lastName;
            model.Address = address;
            model.City = city;
            model.State = state;
            model.Zip = zip;
            model.MobilePhonePrimary = primaryPhone;
            model.HomePhoneSecondary = secondaryPhone;
            model.EmailAddress = emailAddress;
            model.LeaveOn = Convert.ToDateTime(leaveOn);
            model.ReturnOn = Convert.ToDateTime(returnOn);
            model.Destination = destination;
            model.Distance = distance;
            model.Adults = GetIntFromString(adults);
            model.Children = GetIntFromString(children);
            model.WebUserSelectedLocationName = locationName;
            model.WebUserSelectedClassName = webUserSelectedClass;
            model.WebUserComments = webUserComments;
            model.NickName = firstName;
            model.LeadSource = leadSourceOid;
            model.PostalCode = zip;
            model.WebUserSelectedClass = webUserSelectedClass;
            model.ClassOid = classOid;
            model.VehicleName = vehicleName;
            model.VehicleId = vehicleOid;
            model.InsuranceCompanyName = insuranceCompanyName;
            model.LocationInsuranceCompanyOid = insuranceCompanyOid;
            model.Comments = comments;
            model.WebUserSelectedCountry = country;

            var success = _repo.SaveWorkSpaceWebQuote(model);

            if (success)
            {
                if (!string.IsNullOrEmpty(equipments))
                {
                    var list = _repo.GetAllEquipmentTypes();
                    var equipmentOids = GetGuidList(equipments);
                    foreach (var equipmentOid in equipmentOids)
                    {
                        var equipment = list.Where(x => x.Oid == equipmentOid).FirstOrDefault();
                        var equipmentRecord = new WorkspaceFeesAndEquipmentModel()
                        {
                            Oid = Guid.NewGuid(),
                            Location = locationOid,
                            LocationName = locationName,
                            Price = GetDecimalFromCurrency(equipment.WebPrice),
                            Description = equipment.WebDescription,
                            Quantity = 1,
                            ExtendedPrice = GetDecimalFromCurrency(equipment.WebPrice),
                            FeeItemOid = equipment.Oid
                        };
                        success = _repo.SaveWorkSpaceFeesAndEquipment(equipmentRecord);
                    }
                }

                if (success)
                {
                    if (!string.IsNullOrEmpty(fees))
                    {
                        var list = _repo.GetAllFees();
                        var feeOids = GetGuidList(fees);
                        foreach (var feeOid in feeOids)
                        {
                            var fee = list.Where(x => x.Oid == feeOid).FirstOrDefault();
                            var feeRecord = new WorkspaceFeesAndEquipmentModel()
                            {
                                Oid = Guid.NewGuid(),
                                Location = locationOid,
                                LocationName = locationName,
                                Price = GetDecimalFromCurrency(fee.WebPrice),
                                Description = fee.WebDescription,
                                Quantity = 1,
                                ExtendedPrice = GetDecimalFromCurrency(fee.WebPrice),
                                FeeItemOid = fee.Oid
                            };
                            success = _repo.SaveWorkSpaceFeesAndEquipment(feeRecord);
                        }
                    }
                }
            }

            if (success)
                return Ok();

            return null;
        }

        private List<Guid?> GetGuidList(string str)
        {
            var guids = new List<Guid?>();
            var guidArray = str.Split(',').ToList();
            foreach (var item in guidArray)
            {
                guids.Add(new Guid(item));
            }

            return guids;
        }

        private Guid? GetOidFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            var oid = str.Split(':').FirstOrDefault();
            return new Guid(oid);
        }

        private string GetValueFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            var name = str.Split(':').LastOrDefault();
            return name;
        }

        private int? GetIntFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return Convert.ToInt32(str);
        }

        private Guid? GetGuidFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return new Guid(str);
        }

        private decimal? GetDecimalFromCurrency(string price)
        {
            if (string.IsNullOrEmpty(price))
                return null;

            return decimal.Parse(Regex.Replace(price, @"[^\d.]", ""));
        }
    }
}
