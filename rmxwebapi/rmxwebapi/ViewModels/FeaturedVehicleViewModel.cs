using System;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class FeaturedVehicleViewModel
    {
        public Guid VehicleOid { get; set; }
        public string VehicleKey { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public Guid? VehicleClass { get; set; }
        public double? WebPriceGroup { get; set; }
        public long? VehicleSequenceId { get; set; }
        public int? Adolescents { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public decimal? WebCleaningFee { get; set; }
        public decimal? WebPrepFee { get; set; }
        public decimal? WebRefundableSecurityDeposit { get; set; }
        public int? WebGeneratorFreeHours { get; set; }
        public int? WebGeneratorAddHoursEach { get; set; }
        public int? WebIncludesTheseMilesFreePerDay { get; set; }
        public int? WebMinimumNumberOfTimeInterval { get; set; }
        public bool? FeaturedVehicle { get; set; }
        public decimal? HigherRate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string WebDescription { get; set; }
        public decimal? WebEventPriceForPickedUp { get; set; }
        public decimal? WebEventPriceForDelivered { get; set; }
        public decimal? WebEventPriceForDelivered2 { get; set; }
        public string WebEventDescription { get; set; }
        public string WebUniqueId { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public string Photo { get; set; }
        public int? ClassType { get; set; }
        public string ClassName { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public string WebLocationPageHTML { get; set; }
        public string WebLocationPageHTMLBottom { get; set; }
        public string WebRegionalName { get; set; }
        public bool? CalcByNights { get; set; }
        public int? MinimumNumberOfTimeInterval { get; set; }
        public bool? IsLocationBehindOnPaying { get; set; }
        public List<string> DateList { get; set; }
        public bool IsSquarePhoto { get; set; }
        public Nullable<bool> ShowForSale { get; set; }
        public Nullable<DateTime> ForSaleOn { get; set; }
        public Nullable<DateTime> SoldOn { get; set; }
        public int? SalePrice { get; set; }
    }
}