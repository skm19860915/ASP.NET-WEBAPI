using rmxwebapi.Models;
using System;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class VehicleSaleDetailViewModel
    {
        //vehicle
        public Guid Oid { get; set; }
        public string QuickFindKeyWord { get; set; }
        public string NameOnWeb { get; set; }
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
        public string ForSaleDesc { get; set; }
        public int? SalePrice { get; set; }
        public string WebUniqueId { get; set; }
        public Guid? InsurancePolicy { get; set; }
        //location
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        // vehicleclass
        public string ClassName { get; set; }
        // media list
        public List<VehicleMediaItemModel> MediaList { get; set; }
        // amenity list
        public List<AmenityViewModel> AmenityList { get; set; }
    }
}