using System;

namespace rmxwebapi.Models
{
    public class VehicleModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public string Tag { get; set; }
        public string AmenityDesc { get; set; }
        public string WebDescription { get; set; }
        public string OtherCostsDesc { get; set; }
        public string DailyRate { get; set; }
        public string YouTubeID { get; set; }
        public bool? NoTowing { get; set; }
        public bool? NoDogs { get; set; }
        public bool? SmokingAllowed { get; set; }
        public string NameOnWeb { get; set; }

        // sale
        public bool? ShowForSale { get; set; }
        public int? SalePrice { get; set; }
        public string ForSaleDesc { get; set; }
        public string WebForSaleSellerName { get; set; }
        public DateTime? ForSaleOn { get; set; }
        public DateTime? SoldOn { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string VehicleName { get; set; }
        public int? Length { get; set; }
        public string LicensePlate { get; set; }
        public int? FuelType { get; set; }
        public int? Adults { get; set; }
        public int? Belts { get; set; }
        public int? Children { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public int? Adolescents { get; set; }
        public Guid? VehicleClass { get; set; }
        public long? SequenceId { get; set; }
        public string WebUniqueId { get; set; }
        public string QuickFindKeyWord { get; set; }
        public double? WebPriceGroup { get; set; }
        public bool? FeaturedVehicle { get; set; }
        public decimal? WebPrepFee { get; set; }
        public decimal? WebCleaningFee { get; set; }
        public decimal? WebRefundableSecurityDeposit { get; set; }
        public int? WebGeneratorFreeHours { get; set; }
        public int? WebGeneratorAddHoursEach { get; set; }
        public int? WebIncludesTheseMilesFreePerDay { get; set; }
        public int? WebMinimumNumberOfTimeInterval { get; set; }
        public decimal? WebEventPriceForPickedUp { get; set; }
        public decimal? WebEventPriceForDelivered { get; set; }
        public decimal? WebEventPriceForDelivered2 { get; set; }
        public string WebEventDescription { get; set; }
        public decimal? WebEventPriceForDelivered3 { get; set; }
        public string WebTransportedID { get; set; }
        public int? OldVehicleID { get; set; }
    }
}