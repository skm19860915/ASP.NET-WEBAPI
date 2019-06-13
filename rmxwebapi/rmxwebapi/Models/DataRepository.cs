using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Caching;

namespace rmxwebapi.Models
{
    public class DataRepository
    {
        public MemoryCache _cache;
        private CacheItemPolicy _cachePolicy;
        private string _connectionString;

        public DataRepository()
        {
            _cache = MemoryCache.Default;
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.AbsoluteExpiration = DateTime.Now.AddDays(1);
            _connectionString = ConfigurationManager.ConnectionStrings["RMXConnection"].ConnectionString;
        }

        public IQueryable<LocationModel> GetAllLocations()
        {
            if(_cache.Get("LocationCache") != null)
            {
                var listOfCache = _cache.Get("LocationCache") as IEnumerable<LocationModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, OrganizationName, Organization, Name, SequenceId, Company, DBAName, CalcByNights, EmailAddress, OutgoingUserName, " +
                           "OutgoingServerName, OutgoingPassword, OutgoingServerPort, PrimaryPhone, Fax, MobilePhone, SecondaryPhone, PrimaryEmail, SecondaryEmail, Address, " +
                           "City, State, Zip, LocationName, WebRegionalName, WebPrimaryBuildingPhoto, WebMapPhoto, WebTopPerformerOrder, WebMapZoomLevel, " +
                           "WebGoogleMapJavaScriptAPIKey, Longitude, Latitude, isShowCalendarOnWeb, IsCalendarWithBookings, MinimumNumberOfTimeInterval, IsLocationBehindOnPaying, " +
                           "WebStoreHours, WebEmailContactUsToAddress, WebQuoteEmailAddress, FriendlyCompanyName, WebLocationPageHTML, WebLocationPageHTMLBottom, EmailSupportRequestAddress " +
                           "from dbo.Location where IsActive = 1 and GCRecord is null and WebPrimaryBuildingPhoto is not null order by WebTopPerformerOrder";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<LocationModel>();
                    while (reader.Read())
                    {
                        var record = new LocationModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.OrganizationName = reader.GetValue(1).ToString();
                        record.Organization = IsUsableGuidValue(reader, 2);
                        record.Name = reader.GetValue(3).ToString();
                        record.SequenceId = IsUsableLongValue(reader, 4);
                        record.Company = IsUsableGuidValue(reader, 5);
                        record.DBAName = reader.GetValue(6).ToString();
                        record.CalcByNights = IsUsableBoolValue(reader, 7);
                        record.EmailAddress = reader.GetValue(8).ToString();
                        record.OutgoingUserName = reader.GetValue(9).ToString();
                        record.OutgoingServerName = reader.GetValue(10).ToString();
                        record.OutgoingPassword = reader.GetValue(11).ToString();
                        record.OutgoingServerPort = IsUsableIntValue(reader, 12);
                        record.PrimaryPhone = reader.GetValue(13).ToString();
                        record.Fax = reader.GetValue(14).ToString();
                        record.MobilePhone = reader.GetValue(15).ToString();
                        record.SecondaryPhone = reader.GetValue(16).ToString();
                        record.PrimaryEmail = reader.GetValue(17).ToString();
                        record.SecondaryEmail = reader.GetValue(18).ToString();
                        record.Address = reader.GetValue(19).ToString();
                        record.City = reader.GetValue(20).ToString();
                        record.State = reader.GetValue(21).ToString();
                        record.Zip = reader.GetValue(22).ToString();
                        record.LocationName = reader.GetValue(23).ToString();
                        record.WebRegionalName = reader.GetValue(24).ToString();
                        record.WebPrimaryBuildingPhoto = GetCompressPhoto(reader, 25);
                        record.WebMapPhoto = GetCompressPhoto(reader, 26);
                        record.WebTopPerformerOrder = IsUsableIntValue(reader, 27);
                        record.WebMapZoomLevel = IsUsableIntValue(reader, 28);
                        record.WebGoogleMapJavaScriptAPIKey = reader.GetValue(29).ToString();
                        record.Longitude = IsUsableDoubleValue(reader, 30);
                        record.Latitude = IsUsableDoubleValue(reader, 31);
                        record.isShowCalendarOnWeb = IsUsableBoolValue(reader, 32);
                        record.IsCalendarWithBookings = IsUsableBoolValue(reader, 33);
                        record.MinimumNumberOfTimeInterval = IsUsableIntValue(reader, 34);
                        record.IsLocationBehindOnPaying = IsUsableBoolValue(reader, 35);
                        record.WebStoreHours = reader.GetValue(36).ToString();
                        record.WebEmailContactUsToAddress = reader.GetValue(37).ToString();
                        record.WebQuoteEmailAddress = reader.GetValue(38).ToString();
                        record.FriendlyCompanyName = reader.GetValue(39).ToString();
                        record.WebLocationPageHTML = reader.GetValue(40).ToString();
                        record.WebLocationPageHTMLBottom = reader.GetValue(41).ToString();
                        record.EmailSupportRequestAddress = reader.GetValue(42).ToString();

                        list.Add(record);
                    }
                    _cache.Add("LocationCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<VehicleModel> GetAllVehicles()
        {
            if(_cache.Get("VehicleCache") != null)
            {
                var listOfCache = _cache.Get("VehicleCache") as IEnumerable<VehicleModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, LocationName, Tag, AmenityDesc, WebDescription, OtherCostsDesc, DailyRate, YouTubeID, NoTowing, NoDogs, SmokingAllowed, " +
                        "NameOnWeb, ShowForSale, SalePrice, ForSaleDesc, Make, Model, Year, VehicleName, Length, LicensePlate, FuelType, Adults, Belts, Children, InsurancePolicy, Adolescents, VehicleClass, " +
                        "SequenceId, WebUniqueId, QuickFindKeyWord, WebPriceGroup, FeaturedVehicle, WebForSaleSellerName, WebPrepFee, WebCleaningFee, WebRefundableSecurityDeposit, " +
                        "WebGeneratorFreeHours, WebGeneratorAddHoursEach, WebIncludesTheseMilesFreePerDay, WebMinimumNumberOfTimeInterval, " +
                        "WebEventPriceForPickedUp, WebEventPriceForDelivered, WebEventPriceForDelivered2, WebEventDescription, WebEventPriceForDelivered3, WebTransportedID, OldVehicleID, " +
                        "ForSaleOn, SoldOn " +
                        "from dbo.Vehicle where ShowOnWeb = 1 and IsActive = 1 and GCRecord is null and WebUniqueId is not null";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<VehicleModel>();
                    while (reader.Read())
                    {
                        var record = new VehicleModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.LocationName = reader.GetValue(2).ToString();
                        record.Tag = reader.GetValue(3).ToString();
                        record.AmenityDesc = reader.GetValue(4).ToString();
                        record.WebDescription = reader.GetValue(5).ToString();
                        record.OtherCostsDesc = reader.GetValue(6).ToString();
                        record.DailyRate = reader.GetValue(7).ToString();
                        record.YouTubeID = reader.GetValue(8).ToString();
                        record.NoTowing = IsUsableBoolValue(reader, 9);
                        record.NoDogs = IsUsableBoolValue(reader, 10);
                        record.SmokingAllowed = IsUsableBoolValue(reader, 11);
                        record.NameOnWeb = reader.GetValue(12).ToString();
                        record.ShowForSale = IsUsableBoolValue(reader, 13);
                        record.SalePrice = IsUsableIntValue(reader, 14);
                        record.ForSaleDesc = reader.GetValue(15).ToString();
                        record.Make = reader.GetValue(16).ToString();
                        record.Model = reader.GetValue(17).ToString();
                        record.Year = reader.GetValue(18).ToString();
                        record.VehicleName = reader.GetValue(19).ToString();
                        record.Length = IsUsableIntValue(reader, 20);
                        record.LicensePlate = reader.GetValue(21).ToString();
                        record.FuelType = IsUsableIntValue(reader, 22);
                        record.Adults = IsUsableIntValue(reader, 23);
                        record.Belts = IsUsableIntValue(reader, 24);
                        record.Children = IsUsableIntValue(reader, 25);
                        record.InsurancePolicy = IsUsableGuidValue(reader, 26);
                        record.Adolescents = IsUsableIntValue(reader, 27);
                        record.VehicleClass = IsUsableGuidValue(reader, 28);
                        record.SequenceId = IsUsableLongValue(reader, 29);
                        record.WebUniqueId = reader.GetValue(30).ToString();
                        record.QuickFindKeyWord = reader.GetValue(31).ToString();
                        record.WebPriceGroup = IsUsableDoubleValue(reader, 32);
                        record.FeaturedVehicle = IsUsableBoolValue(reader, 33);
                        record.WebForSaleSellerName = reader.GetValue(34).ToString();
                        record.WebPrepFee = IsUsableDecimalValue(reader, 35);
                        record.WebCleaningFee = IsUsableDecimalValue(reader, 36);
                        record.WebRefundableSecurityDeposit = IsUsableDecimalValue(reader, 37);
                        record.WebGeneratorFreeHours = IsUsableIntValue(reader, 38);
                        record.WebGeneratorAddHoursEach = IsUsableIntValue(reader, 39);
                        record.WebIncludesTheseMilesFreePerDay = IsUsableIntValue(reader, 40);
                        record.WebMinimumNumberOfTimeInterval = IsUsableIntValue(reader, 41);
                        record.WebEventPriceForPickedUp = IsUsableDecimalValue(reader, 42);
                        record.WebEventPriceForDelivered = IsUsableDecimalValue(reader, 43);
                        record.WebEventPriceForDelivered2 = IsUsableDecimalValue(reader, 44);
                        record.WebEventDescription = reader.GetValue(45).ToString();
                        record.WebEventPriceForDelivered3 = IsUsableDecimalValue(reader, 46);
                        record.WebTransportedID = reader.GetValue(47).ToString();
                        record.OldVehicleID = IsUsableIntValue(reader, 48);
                        record.ForSaleOn = IsUsableDateTimeValue(reader, 49);
                        record.SoldOn = IsUsableDateTimeValue(reader, 50);

                        list.Add(record);
                    }
                    _cache.Add("VehicleCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<VehicleClassModel> GetAllVehicleClasses()
        {
            if(_cache.Get("VehicleClassCache") != null)
            {
                var listInCache = _cache.Get("VehicleClassCache") as IEnumerable<VehicleClassModel>;
                return listInCache.AsQueryable();
            }

            var queryString = "select Oid, Name, ClassType, WebBannerName, BaseClass, SortOrder from dbo.VehicleClass";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<VehicleClassModel>();
                    while (reader.Read())
                    {
                        var record = new VehicleClassModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Name = reader.GetValue(1).ToString();
                        record.ClassType = IsUsableIntValue(reader, 2);
                        record.WebBannerName = reader.GetValue(3).ToString();
                        record.BaseClass = IsUsableIntValue(reader, 4);
                        record.SortOrder = IsUsableIntValue(reader, 5);
                        list.Add(record);
                    }
                    _cache.Add("VehicleClassCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<CustomVehicleMediaItemViewModel> GetAllVehicleMediaItems()
        {
            if (_cache.Get("VehicleMediaItemCache") != null)
            {
                var listInCache = _cache.Get("VehicleMediaItemCache") as IEnumerable<CustomVehicleMediaItemViewModel>;
                return listInCache.AsQueryable();
            }

            var queryString = "select Vehicle, Photo from dbo.VehicleMediaItem where PhotoCode = 0 and Photo is not null and Vehicle is not null and GCRecord is null and IsActive = 1";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<CustomVehicleMediaItemViewModel>();
                    while (reader.Read())
                    {
                        var record = new CustomVehicleMediaItemViewModel();
                        record.Vehicle = IsUsableGuidValue(reader, 0);
                        record.PhotoInfo = GetCompressPhotoInfo(reader, 1);
                        list.Add(record);
                    }
                    _cache.Add("VehicleMediaItemCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<VehicleMediaItemModel> GetAllMediaList()
        {
            if(_cache.Get("MediaListCache") != null)
            {
                var listOfCache = _cache.Get("MediaListCache") as IEnumerable<VehicleMediaItemModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Vehicle, Photo, PhotoCode from dbo.VehicleMediaItem where Photo is not null and Vehicle is not null and GCRecord is null and IsActive = 1 order by PhotoCode";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<VehicleMediaItemModel>();
                    while (reader.Read())
                    {
                        var record = new VehicleMediaItemModel();
                        record.Vehicle = IsUsableGuidValue(reader, 0);
                        record.Photo = GetCompressPhoto(reader, 1);
                        record.PhotoCode = IsUsableIntValue(reader, 2);
                        list.Add(record);
                    }
                    _cache.Add("MediaListCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<WebCityStaticPageModel> GetAllWebCityStaticPages()
        {
            if(_cache.Get("WebCityStaticPageCache") != null)
            {
                var listOfCache = _cache.Get("WebCityStaticPageCache") as IEnumerable<WebCityStaticPageModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Location, AnsweringPageURL, MetaTitle, HTMLPageContent, IsPointOfInterest, IsEvent " +
                            "from dbo.WebCityStaticPage where GCRecord is null and IsActive = 1 and AnsweringPageURL is not null order by SortOrder";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<WebCityStaticPageModel>();
                    while (reader.Read())
                    {
                        var record = new WebCityStaticPageModel();
                        record.Location = IsUsableGuidValue(reader, 0);
                        record.AnsweringPageURL = reader.GetValue(1).ToString();
                        record.MetaTitle = reader.GetValue(2).ToString();
                        record.HTMLPageContent = reader.GetValue(3).ToString();
                        record.IsPointOfInterest = IsUsableBoolValue(reader, 4);
                        record.IsEvent = IsUsableBoolValue(reader, 5);
                        list.Add(record);
                    }
                    _cache.Add("WebCityStaticPageCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<CompanyModel> GetAllCompanies()
        {
            if(_cache.Get("CompanyCache") != null)
            {
                var listOfCache = _cache.Get("CompanyCache") as IEnumerable<CompanyModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select WebfooterCopyright, IsAllowSmartyStreets, WebAllLocationsHTML from dbo.Company";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<CompanyModel>();
                    while (reader.Read())
                    {
                        var record = new CompanyModel();
                        record.WebfooterCopyright = reader.GetValue(0).ToString();
                        record.IsAllowSmartyStreets = IsUsableBoolValue(reader, 1);
                        record.WebAllLocationsHTML = reader.GetValue(2).ToString();
                        list.Add(record);
                    }
                    _cache.Add("CompanyCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<RentalModel> GetAllRentals()
        {
            if(_cache.Get("RentalCache") != null)
            {
                var listOfCache = _cache.Get("RentalCache") as IEnumerable<RentalModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Location, LeaveOn, ReturnOn, Destination, Vehicle, IsOwnerRenting, BookedOn, ReferenceEstimateSequenceId from dbo.Rental where GCRecord is null";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<RentalModel>();
                    while (reader.Read())
                    {
                        var record = new RentalModel();
                        record.Location = IsUsableGuidValue(reader, 0);
                        record.LeaveOn = IsUsableDateTimeValue(reader, 1);
                        record.ReturnOn = IsUsableDateTimeValue(reader, 2);
                        record.Destination = reader.GetValue(3).ToString();
                        record.Vehicle = IsUsableGuidValue(reader, 4);
                        record.IsOwnerRenting = IsUsableBoolValue(reader, 5);
                        record.BookedOn = IsUsableDateTimeValue(reader, 6);
                        record.ReferenceEstimateSequenceId = IsUsableIntValue(reader, 7);
                        list.Add(record);
                    }
                    _cache.Add("RentalCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<AmenityModel> GetAllAmenities()
        {
            if(_cache.Get("AmenityCache") != null)
            {
                var listOfCache = _cache.Get("AmenityCache") as IEnumerable<AmenityModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Name from dbo.Amenity";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<AmenityModel>();
                    while (reader.Read())
                    {
                        var record = new AmenityModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Name = reader.GetValue(1).ToString();
                        list.Add(record);
                    }
                    _cache.Add("AmenityCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<VehicleAmenityModel> GetAllVehiclesAmenities()
        {
            if(_cache.Get("VehicleAmenityCache") != null)
            {
                var listOfCache = _cache.Get("VehicleAmenityCache") as IEnumerable<VehicleAmenityModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Amenities, Vehicles from dbo.VehicleVehicles_AmenityAmenities";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<VehicleAmenityModel>();
                    while (reader.Read())
                    {
                        var record = new VehicleAmenityModel();
                        record.Amenities = IsUsableGuidValue(reader, 0);
                        record.Vehicles = IsUsableGuidValue(reader, 1);
                        list.Add(record);
                    }
                    _cache.Add("VehicleAmenityCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<EquipmentTypeModel> GetAllEquipmentTypes()
        {
            if(_cache.Get("EquipmentTypeCache") != null)
            {
                var listOfCache = _cache.Get("EquipmentTypeCache") as IEnumerable<EquipmentTypeModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, Name, WebPrice, WebDescription, Tag from dbo.EquipmentType where IsShownOnWeb = 1 and IsActive = 1 and Name is not null order by Name";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<EquipmentTypeModel>();
                    while (reader.Read())
                    {
                        var record = new EquipmentTypeModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.Name = reader.GetValue(2).ToString();
                        record.WebPrice = reader.GetValue(3).ToString();
                        record.WebDescription = reader.GetValue(4).ToString();
                        record.Tag = reader.GetValue(5).ToString();
                        list.Add(record);
                    }
                    _cache.Add("EquipmentTypeCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<FeeModel> GetAllFees()
        {
            if(_cache.Get("FeeCache") != null)
            {
                var listOfCache = _cache.Get("FeeCache") as IEnumerable<FeeModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, Name, WebPrice, WebDescription, Tag, SequenceId from dbo.Fee where IsShownOnWeb = 1 and IsActive = 1 and Name is not null order by Name";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<FeeModel>();
                    while (reader.Read())
                    {
                        var record = new FeeModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.Name = reader.GetValue(2).ToString();
                        record.WebPrice = reader.GetValue(3).ToString();
                        record.WebDescription = reader.GetValue(4).ToString();
                        record.Tag = reader.GetValue(5).ToString();
                        record.SequenceId = IsUsableLongValue(reader, 6);
                        list.Add(record);
                    }
                    _cache.Add("FeeCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<LocationInsuranceCompanyModel> GetAllLocationInsuranceCompanies()
        {
            if(_cache.Get("LocationInsuranceCompanyCache") != null)
            {
                var listOfCache = _cache.Get("LocationInsuranceCompanyCache") as IEnumerable<LocationInsuranceCompanyModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, InsuranceCompanyName from dbo.LocationInsuranceCompany where ShowOnWeb = 1 and IsActive = 1 and " +
                            "InsuranceCompanyName is not null and GCRecord is null order by InsuranceCompanyName";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<LocationInsuranceCompanyModel>();
                    while (reader.Read())
                    {
                        var record = new LocationInsuranceCompanyModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.InsuranceCompanyName = reader.GetValue(2).ToString();
                        list.Add(record);
                    }
                    _cache.Add("LocationInsuranceCompanyCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<LeadSourceModel> GetAllLeadSources()
        {
            if(_cache.Get("LeadSourceCache") != null)
            {
                var listOfCache = _cache.Get("LeadSourceCache") as IEnumerable<LeadSourceModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, Name, IsSystemObject, ShowOnWeb from dbo.LeadSource where IsActive = 1 and GCRecord is null and Name is not null order by Name";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<LeadSourceModel>();
                    while (reader.Read())
                    {
                        var record = new LeadSourceModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.Name = reader.GetValue(2).ToString();
                        record.IsSystemObject = IsUsableBoolValue(reader, 3);
                        record.ShowOnWeb = IsUsableBoolValue(reader, 4);
                        list.Add(record);
                    }
                    _cache.Add("LeadSourceCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<CampgroundModel> GetAllCampgrounds()
        {
            if(_cache.Get("CampgroundCache") != null)
            {
                var listOfCache = _cache.Get("CampgroundCache") as IEnumerable<CampgroundModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select * from dbo.Campground";
            var connectionString = ConfigurationManager.ConnectionStrings["CampgroundConnection"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<CampgroundModel>();
                    while (reader.Read())
                    {
                        var record = new CampgroundModel();
                        record.Id = IsUsableDoubleValue(reader, 0);
                        record.Lat = IsUsableDoubleValue(reader, 1);
                        record.Lng = IsUsableDoubleValue(reader, 2);
                        record.Name = reader.GetValue(3).ToString();
                        record.State = reader.GetValue(4).ToString();
                        record.Type = reader.GetValue(5).ToString();
                        record.City = reader.GetValue(6).ToString();
                        record.Street = reader.GetValue(7).ToString();
                        record.Website = reader.GetValue(8).ToString();
                        record.Zip = IsUsableDoubleValue(reader, 9);
                        record.Email = reader.GetValue(10).ToString();
                        record.Phone = reader.GetValue(11).ToString();
                        record.DeleteFlag = (bool)reader.GetValue(12);
                        record.DeleteDate = reader.GetValue(13).ToString();
                        list.Add(record);
                    }
                    _cache.Add("CampgroundCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<EventVehicleModel> GetAllEventVehicles()
        {
            if(_cache.Get("EventVehicleCache") != null)
            {
                var listOfCache = _cache.Get("EventVehicleCache") as IEnumerable<EventVehicleModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select Oid, Location, Tag, YouTubeID, VehicleClass, QuickFindKeyWord, WebEventPriceForPickedUp, WebEventPriceForDelivered, WebEventPriceForDelivered2, WebEventPriceForDelivered3, DailyRate " +
                            "from dbo.Vehicle where IsShowOnWebEventFlyer = 1 and IsActive = 1 and GCRecord is null";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<EventVehicleModel>();
                    while (reader.Read())
                    {
                        var record = new EventVehicleModel();
                        record.Oid = (Guid)reader.GetValue(0);
                        record.Location = IsUsableGuidValue(reader, 1);
                        record.Tag = reader.GetValue(2).ToString();
                        record.YouTubeID = reader.GetValue(3).ToString();
                        record.VehicleClass = IsUsableGuidValue(reader, 4);
                        record.QuickFindKeyWord = reader.GetValue(5).ToString();
                        record.WebEventPriceForPickedUp = IsUsableDecimalValue(reader, 6);
                        record.WebEventPriceForDelivered = IsUsableDecimalValue(reader, 7);
                        record.WebEventPriceForDelivered2 = IsUsableDecimalValue(reader, 8);
                        record.WebEventPriceForDelivered3 = IsUsableDecimalValue(reader, 9);
                        record.DailyRate = reader.GetValue(10).ToString();
                        list.Add(record);
                    }
                    _cache.Add("EventVehicleCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public IQueryable<MatchVehicleViewModel> GetAllMatchVehicles()
        {
            if(_cache.Get("MatchVehicleCache") != null)
            {
                var listOfCache = _cache.Get("MatchVehicleCache") as IEnumerable<MatchVehicleViewModel>;
                return listOfCache.AsQueryable();
            }

            var queryString = "select WebUniqueId, OldVehicleID, IsActive, Location, VehicleClass, ShowOnWeb from dbo.Vehicle where WebUniqueId is not null " +
                            "and OldVehicleID is not null and OldVehicleID != 0 and GCRecord is null and Location is not null order by OldVehicleID";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    var list = new List<MatchVehicleViewModel>();
                    while (reader.Read())
                    {
                        var record = new MatchVehicleViewModel();
                        record.WebUniqueId = reader.GetValue(0).ToString();
                        record.OldVehicleID = reader.GetInt32(1);
                        record.IsActive = IsUsableBoolValue(reader, 2);
                        record.Location = IsUsableGuidValue(reader, 3);
                        record.VehicleClass = IsUsableGuidValue(reader, 4);
                        record.ShowOnWeb = IsUsableBoolValue(reader, 5);
                        list.Add(record);
                    }
                    _cache.Add("MatchVehicleCache", list, _cachePolicy);
                    return list.AsQueryable();
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public bool SaveWorkSpaceWebQuote(WorkspaceWebQuoteModel model)
        {
            var queryString = "insert into dbo.WorkspaceWebQuote (Oid, IsSystemObject, Organization, Location, OrganizationName, LocationName, IsActive, CreatedOn, Name, First, Last, " +
                            "Address, City, State, Zip, MobilePhonePrimary, HomePhoneSecondary, EmailAddress, LeaveOn, ReturnOn, Destination, Distance, " +
                            "Adults, Children, WebUserSelectedLocationName, WebUserSelectedClassName, WebUserComments, NickName, LeadSource, PostalCode, WebUserSelectedClass, ClassOid, " +
                            "VehicleName, VehicleId, InsuranceCompanyName, LocationInsuranceCompanyOid, Comments, WebUserSelectedCountry) " +
                            "values (@Oid, @IsSystemObject, @Organization, @Location, @OrganizationName, @LocationName, @IsActive, @CreatedOn, @Name, @First, @Last, " +
                            "@Address, @City, @State, @Zip, @MobilePhonePrimary, @HomePhoneSecondary, @EmailAddress, @LeaveOn, @ReturnOn, @Destination, @Distance, " +
                            "@Adults, @Children, @WebUserSelectedLocationName, @WebUserSelectedClassName, @WebUserComments, @NickName, @LeadSource, @PostalCode, @WebUserSelectedClass, @ClassOid, " +
                            "@VehicleName, @VehicleId, @InsuranceCompanyName, @LocationInsuranceCompanyOid, @Comments, @WebUserSelectedCountry)";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@Oid", model.Oid);
                    command.Parameters.AddWithValue("@IsSystemObject", model.IsSystemObject);
                    command.Parameters.AddWithValue("@Organization", IsSetDBNull(model.Organization));
                    command.Parameters.AddWithValue("@Location", IsSetDBNull(model.Location));
                    command.Parameters.AddWithValue("@OrganizationName", model.OrganizationName);
                    command.Parameters.AddWithValue("@LocationName", model.LocationName);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);
                    command.Parameters.AddWithValue("@CreatedOn", model.CreatedOn);
                    command.Parameters.AddWithValue("@Name", IsSetDBNullFromString(model.Name));
                    command.Parameters.AddWithValue("@First", IsSetDBNullFromString(model.First));
                    command.Parameters.AddWithValue("@Last", IsSetDBNullFromString(model.Last));
                    command.Parameters.AddWithValue("@Address", IsSetDBNullFromString(model.Address));
                    command.Parameters.AddWithValue("@City", IsSetDBNullFromString(model.City));
                    command.Parameters.AddWithValue("@State", IsSetDBNullFromString(model.State));
                    command.Parameters.AddWithValue("@Zip", IsSetDBNullFromString(model.Zip));
                    command.Parameters.AddWithValue("@MobilePhonePrimary", IsSetDBNullFromString(model.MobilePhonePrimary));
                    command.Parameters.AddWithValue("@HomePhoneSecondary", IsSetDBNullFromString(model.HomePhoneSecondary));
                    command.Parameters.AddWithValue("@EmailAddress", IsSetDBNullFromString(model.EmailAddress));
                    command.Parameters.AddWithValue("@LeaveOn", model.LeaveOn);
                    command.Parameters.AddWithValue("@ReturnOn", model.ReturnOn);
                    command.Parameters.AddWithValue("@Destination", IsSetDBNullFromString(model.Destination));
                    command.Parameters.AddWithValue("@Distance", IsSetDBNullFromString(model.Distance));
                    command.Parameters.AddWithValue("@Adults", IsSetDBNullFromInt(model.Adults));
                    command.Parameters.AddWithValue("@Children", IsSetDBNullFromInt(model.Children));
                    command.Parameters.AddWithValue("@WebUserSelectedLocationName", model.WebUserSelectedLocationName);
                    command.Parameters.AddWithValue("@WebUserSelectedClassName", model.WebUserSelectedClassName);
                    command.Parameters.AddWithValue("@WebUserComments", IsSetDBNullFromString(model.WebUserComments));
                    command.Parameters.AddWithValue("@NickName", IsSetDBNullFromString(model.NickName));
                    command.Parameters.AddWithValue("@LeadSource", IsSetDBNull(model.LeadSource));
                    command.Parameters.AddWithValue("@PostalCode", IsSetDBNullFromString(model.PostalCode));
                    command.Parameters.AddWithValue("@WebUserSelectedClass", model.WebUserSelectedClass);
                    command.Parameters.AddWithValue("@ClassOid", IsSetDBNull(model.ClassOid));
                    command.Parameters.AddWithValue("@VehicleName", model.VehicleName);
                    command.Parameters.AddWithValue("@VehicleId", IsSetDBNull(model.VehicleId));
                    command.Parameters.AddWithValue("@InsuranceCompanyName", IsSetDBNullFromString(model.InsuranceCompanyName));
                    command.Parameters.AddWithValue("@LocationInsuranceCompanyOid", IsSetDBNull(model.LocationInsuranceCompanyOid));
                    command.Parameters.AddWithValue("@Comments", IsSetDBNullFromString(model.Comments));
                    command.Parameters.AddWithValue("@WebUserSelectedCountry", IsSetDBNullFromString(model.WebUserSelectedCountry));

                    int id = command.ExecuteNonQuery();
                    if (id > 0)
                        return true;

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SaveWorkSpaceFeesAndEquipment(WorkspaceFeesAndEquipmentModel model)
        {
            var queryString = "insert into dbo.WorkspaceFeesAndEquipment (Oid, Location, LocationName, Quantity, Description, Price, ExtendedPrice, FeeItemOid) " +
                            "values (@Oid, @Location, @LocationName, @Quantity, @Description, @Price, @ExtendedPrice, @FeeItemOid)";

            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@Oid", model.Oid);
                    command.Parameters.AddWithValue("@Location", model.Location);
                    command.Parameters.AddWithValue("@LocationName", model.LocationName);
                    command.Parameters.AddWithValue("@Quantity", model.Quantity);
                    command.Parameters.AddWithValue("@Description", IsSetDBNullFromString(model.Description));
                    command.Parameters.AddWithValue("@Price", IsSetDBNullFromDecimal(model.Price));
                    command.Parameters.AddWithValue("@ExtendedPrice", IsSetDBNullFromDecimal(model.ExtendedPrice));
                    command.Parameters.AddWithValue("@FeeItemOid", model.FeeItemOid);

                    int id = command.ExecuteNonQuery();
                    if (id > 0)
                        return true;

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
    
        public object IsSetDBNull(Guid? id)
        {
            if (id == null)
                return DBNull.Value;

            return id;
        }

        private object IsSetDBNullFromInt(int? val)
        {
            if (val == null)
                return DBNull.Value;

            return val;
        }

        private object IsSetDBNullFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return DBNull.Value;

            return str;
        }

        private object IsSetDBNullFromDecimal(decimal? value)
        {
            if (value == null)
                return DBNull.Value;

            return value;
        }

        public DateTime? IsUsableDateTimeValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetDateTime(index);

            return null;
        }

        public decimal? IsUsableDecimalValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetDecimal(index);

            return null;
        }

        public int? IsUsableIntValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetInt32(index);

            return null;
        }

        public long? IsUsableLongValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetInt64(index);

            return null;
        }

        public bool? IsUsableBoolValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetBoolean(index);

            return null;
        }

        public double? IsUsableDoubleValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetDouble(index);

            return null;
        }

        public Guid? IsUsableGuidValue(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetGuid(index);

            return null;
        }

        private byte[] GetCompressPhoto(SqlDataReader reader, int index)
        {
            int quality = 70;
            Image image;
            byte[] photo = null;

            if (reader.IsDBNull(index))
                return null;

            photo = (byte[])reader.GetValue(index);
            var size = photo.Length / 1024 / 1024;
            if (size > 1.5)
                return new byte[0];

            if (photo == null || photo.Count() == 0)
                return null;

            using (var inputStream = new MemoryStream(photo))
            {
                image = Image.FromStream(inputStream);
                var encoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                Byte[] output;
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, encoder, encoderParameters);
                    output = outputStream.ToArray();
                }
                return output;
            }
        }

        private PhotoInfoViewModel GetCompressPhotoInfo(SqlDataReader reader, int index)
        {
            var quality = 70;
            Image image;
            var isSquare = false;

            byte[] photo = null;

            if (reader.IsDBNull(index))
                return null;

            photo = (byte[])reader.GetValue(index);
            var size = photo.Length / 1024 / 1024;
            if (size > 1.5)
            {
                var record = new PhotoInfoViewModel
                {
                    Photo = new byte[0],
                    IsSquarePhoto = isSquare
                };
                return record;
            }

            if (photo == null || photo.Count() == 0)
            {
                var record = new PhotoInfoViewModel
                {
                    Photo = null,
                    IsSquarePhoto = isSquare,
                };
                return record;
            }

            using (var inputStream = new MemoryStream(photo))
            {
                image = Image.FromStream(inputStream);
                var encoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                Byte[] output;
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, encoder, encoderParameters);
                    output = outputStream.ToArray();
                }

                var height = image.Height;
                var width = image.Width;
                if (width == height)
                    isSquare = true;

                var record = new PhotoInfoViewModel
                {
                    Photo = output,
                    IsSquarePhoto = isSquare
                };

                return record;
            }
        }
    }
}