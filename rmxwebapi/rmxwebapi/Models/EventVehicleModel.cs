using System;

namespace rmxwebapi.Models
{
    public class EventVehicleModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string Tag { get; set; }
        public string YouTubeID { get; set; }
        public Guid? VehicleClass { get; set; }
        public string QuickFindKeyWord { get; set; }
        public decimal? WebEventPriceForPickedUp { get; set; }
        public decimal? WebEventPriceForDelivered { get; set; }
        public decimal? WebEventPriceForDelivered2 { get; set; }
        public decimal? WebEventPriceForDelivered3 { get; set; }
        public string DailyRate { get; set; }
    }
}