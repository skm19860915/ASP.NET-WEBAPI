using System;

namespace rmxwebapi.ViewModels
{
    public class MatchVehicleViewModel
    {
        public int OldVehicleID { get; set; }
        public string WebUniqueId { get; set; }
        public bool? IsActive { get; set; }
        public Guid? Location { get; set; }
        public Guid? VehicleClass { get; set; }
        public bool? ShowOnWeb { get; set; }
    }
}