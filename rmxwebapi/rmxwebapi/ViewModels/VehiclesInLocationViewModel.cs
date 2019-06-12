using System;

namespace rmxwebapi.ViewModels
{
    public class VehiclesInLocationViewModel
    {
        public string Name { get; set; }
        public string VehicleKey { get; set; }
        public int ClassType { get; set; }
        public string ClassName { get; set; }
        public string WebUniqueId { get; set; }
        public Guid? InsurancePolicy { get; set; }
    }
}