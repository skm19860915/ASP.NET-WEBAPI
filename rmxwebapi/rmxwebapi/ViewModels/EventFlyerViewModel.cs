using rmxwebapi.Models;
using System;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class EventFlyerViewModel
    {
        // vehicle
        public Guid Vehicle { get; set; }
        public string Tag { get; set; }
        public string QuickFindKeyWord { get; set; }
        public decimal? WebEventPriceForPickedUp { get; set; }
        public decimal? WebEventPriceForDelivered { get; set; }
        public decimal? WebEventPriceForDelivered2 { get; set; }
        public decimal? WebEventPriceForDelivered3 { get; set; }
        public string YouTubeID { get; set; }
        public string DailyRate { get; set; }
        // vehicle class
        public Guid VehicleClass { get; set; }
        public string ClassName { get; set; }
        public int? ClassType { get; set; }
        // vehicle media
        public List<VehicleMediaItemModel> MediaList { get; set; }
        // location
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public string PrimaryPhone { get; set; }
        public string MobilePhone { get; set; }
        public string OutgoingUserName { get; set; }
        public string OutgoingServerName { get; set; }
        public string OutgoingPassword { get; set; }
        public int? OutgoingServerPort { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string DBAName { get; set; }
        public string WebRegionalName { get; set; }
        // rental
        public DateTime? LeaveOn { get; set; }
        public DateTime? ReturnOn { get; set; }
    }
}