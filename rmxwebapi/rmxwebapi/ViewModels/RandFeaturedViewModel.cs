using System;

namespace rmxwebapi.ViewModels
{
    public class RandFeaturedViewModel
    {
        public Guid VehicleOid { get; set; }
        public string VehicleKey { get; set; }
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public string Price { get; set; }
        public Guid? VehicleClass { get; set; }
        public double? WebPriceGroup { get; set; }
        public int? WebTopPerformerOrder { get; set; }
        public int? ClassType { get; set; }
        public string ClassName { get; set; }
        public long? VehicleSequenceId { get; set; }
        public long? LocationSequenceId { get; set; }
        public string WebLocationPageHTML { get; set; }
        public string WebLocationPageHTMLBottom { get; set; }
        public string WebRegionalName { get; set; }
        public string WebBannerName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool? IsLocationBehindOnPaying { get; set; }
        public string WebUniqueId { get; set; }
        public int? SortOrder { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public bool IsSquarePhoto { get; set; }
    }
}