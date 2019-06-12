using System;

namespace rmxwebapi.Models
{
    public class VehicleClassModel
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public int? ClassType { get; set; }
        public string WebBannerName { get; set; }
        public int? BaseClass { get; set; }
        public int? SortOrder { get; set; }
    }
}