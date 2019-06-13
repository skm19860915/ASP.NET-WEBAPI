using System;

namespace rmxwebapi.Models
{
    public class LocationModel
    {
        public Guid Oid { get; set; }
        public string OrganizationName { get; set; }
        public Guid? Organization { get; set; }
        public string Name { get; set; }
        public long? SequenceId { get; set; }
        public Guid? Company { get; set; }
        public string DBAName { get; set; }
        public bool? CalcByNights { get; set; }
        public string EmailAddress { get; set; }
        public string OutgoingUserName { get; set; }
        public string OutgoingServerName { get; set; }
        public string OutgoingPassword { get; set; }
        public int? OutgoingServerPort { get; set; }
        public string PrimaryPhone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LocationName { get; set; }
        public string WebRegionalName { get; set; }
        public byte[] WebPrimaryBuildingPhoto { get; set; }
        public byte[] WebMapPhoto { get; set; }
        public int? WebTopPerformerOrder { get; set; }
        public int? WebMapZoomLevel { get; set; }
        public string WebGoogleMapJavaScriptAPIKey { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public bool? isShowCalendarOnWeb { get; set; }
        public bool? IsCalendarWithBookings { get; set; }
        public int? MinimumNumberOfTimeInterval { get; set; }
        public bool? IsLocationBehindOnPaying { get; set; }
        public string WebLocationPageHTML { get; set; }
        public string WebLocationPageHTMLBottom { get; set; }
        public string WebStoreHours { get; set; }
        public string WebEmailContactUsToAddress { get; set; }
        public string WebQuoteEmailAddress { get; set; }
        public string FriendlyCompanyName { get; set; }
        public string EmailSupportRequestAddress { get; set; }
    }
}