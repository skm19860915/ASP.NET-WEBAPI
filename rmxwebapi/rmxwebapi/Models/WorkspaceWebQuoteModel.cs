using System;

namespace rmxwebapi.Models
{
    public class WorkspaceWebQuoteModel
    {
        public Guid Oid { get; set; }
        public bool? IsSystemObject { get; set; }
        public Guid? Organization { get; set; }
        public Guid? Location { get; set; }
        public string OrganizationName { get; set; }
        public string LocationName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Name { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MobilePhonePrimary { get; set; }
        public string HomePhoneSecondary { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? LeaveOn { get; set; }
        public DateTime? ReturnOn { get; set; }
        public string Destination { get; set; }
        public string Distance { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public string WebUserSelectedLocationName { get; set; }
        public string WebUserSelectedClassName { get; set; }
        public string WebUserComments { get; set; }
        public string NickName { get; set; }
        public Guid? LeadSource { get; set; }
        public string PostalCode { get; set; }
        public string WebUserSelectedClass { get; set; }
        public Guid? ClassOid { get; set; }
        public string VehicleName { get; set; }
        public Guid? VehicleId { get; set; }
        public string InsuranceCompanyName { get; set; }
        public Guid? LocationInsuranceCompanyOid { get; set; }
        public string Comments { get; set; }
        public string WebUserSelectedCountry { get; set; }
    }
}