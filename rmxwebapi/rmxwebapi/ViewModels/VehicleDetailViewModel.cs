using rmxwebapi.Models;
using System;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class VehicleDetailViewModel
    {
        //vehicle
        public Guid Oid { get; set; }
        public string WebUniqueId { get; set; }
        public string QuickFindKeyWord { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public Guid? VehicleClass { get; set; }
        public string NameOnWeb { get; set; }
        public string DailyRate { get; set; }
        public string YouTubeID { get; set; }
        public bool? NoTowing { get; set; }
        public int? Belts { get; set; }
        public int? Length { get; set; }
        public bool? NoDogs { get; set; }
        public int? FuelType { get; set; }
        public string Model { get; set; }
        public bool? SmokingAllowed { get; set; }
        public string Make { get; set; }
        public string Year { get; set; }
        public int? Children { get; set; }
        public int? Adolescents { get; set; }
        public int? Adults { get; set; }
        public string WebDescription { get; set; }
        public string OtherCostsDesc { get; set; }
        public long? VehicleSequenceId { get; set; }
        public decimal? WebPrepFee { get; set; }
        public decimal? WebCleaningFee { get; set; }
        public decimal? WebRefundableSecurityDeposit { get; set; }
        public int? WebGeneratorFreeHours { get; set; }
        public int? WebIncludesTheseMilesFreePerDay { get; set; }
        public decimal? HigherRate { get; set; }
        public decimal? WebEventPriceForPickedUp { get; set; }
        public decimal? WebEventPriceForDelivered { get; set; }
        public decimal? WebEventPriceForDelivered2 { get; set; }
        public string WebEventDescription { get; set; }
        public string WebTransportedID { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public int? OldVehicleID { get; set; }
        public Nullable<bool> ShowForSale { get; set; }
        public Nullable<DateTime> ForSaleOn { get; set; }
        public Nullable<DateTime> SoldOn { get; set; }
        public int? SalePrice { get; set; }
        //location
        public bool? isShowCalendarOnWeb { get; set; }
        public bool? IsCalendarWithBookings { get; set; }
        public string PrimaryPhone { get; set; }
        public string DBAName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string WebGoogleMapJavaScriptAPIKey { get; set; }
        public bool? IsLocationBehindOnPaying { get; set; }
        public long? LocationSequenceId { get; set; }
        public Guid? Organization { get; set; }
        public string OrganizationName { get; set; }
        public string OutgoingUserName { get; set; }
        public string OutgoingServerName { get; set; }
        public int? OutgoingServerPort { get; set; }
        public string OutgoingPassword { get; set; }
        public string EmailAddress { get; set; }
        public string WebRegionalName { get; set; }
        public int? MinimumNumberOfTimeInterval { get; set; }
        public bool? CalcByNights { get; set; }
        public string WebQuoteEmailAddress { get; set; }
        public string EmailSupportRequestAddress { get; set; }
        public string FriendlyCompanyName { get; set; }
        // vehicleclass
        public Guid ClassOid { get; set; }
        public string ClassName { get; set; }
        public int? ClassType { get; set; }
        // media list
        public List<VehicleMediaItemModel> MediaList { get; set; }
        // rental list
        public List<RentalModel> RentalList { get; set; }
        // amenity list
        public List<AmenityViewModel> AmenityList { get; set; }
        // sort vehicle list
        public List<VehiclesInLocationViewModel> VehicleList { get; set; }
        // random other vehicle list
        public List<RandOtherFeaturedViewModel> RandOtherVehicleList { get; set; }
    }
}