using System;

namespace rmxwebapi.ViewModels
{
    public class RandOtherFeaturedViewModel
    {
        public long? VehicleSequenceId { get; set; }
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string Price { get; set; }
        public Guid? InsurancePolicy { get; set; }
        public string WebUniqueId { get; set; }
        public bool IsSquarePhoto { get; set; }
    }
}