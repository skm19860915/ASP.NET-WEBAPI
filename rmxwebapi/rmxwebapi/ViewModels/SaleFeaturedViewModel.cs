using System;

namespace rmxwebapi.ViewModels
{
    public class SaleFeaturedViewModel
    {
        public string VehicleKey { get; set; }
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public Guid? VehicleClass { get; set; }
        public double? WebPriceGroup { get; set; }
        public int? WebTopPerformerOrder { get; set; }
        public int? ClassType { get; set; }
        public string ClassName { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public int? SalePrice { get; set; }
        public string ForSaleDesc { get; set; }
        public string WebForSaleSellerName { get; set; }
        public string WebUniqueId { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public bool IsSquarePhoto { get; set; }
    }
}