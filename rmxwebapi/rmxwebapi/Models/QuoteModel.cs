using System;

namespace rmxwebapi.Models
{
    public class QuoteModel
    {
        public Guid Oid { get; set; }
        public string Organization { get; set; }
        public string Location { get; set; }
        public string OrganizationName { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MobilePhonePrimary { get; set; }
        public string HomePhoneSecondary { get; set; }
        public string EmailAddress { get; set; }
        public string LeaveOn { get; set; }
        public string ReturnOn { get; set; }
        public string Destination { get; set; }
        public string Distance { get; set; }
        public string Adults { get; set; }
        public string Children { get; set; }
        public string WebUserComments { get; set; }
        public string LeadSourceOid { get; set; }
        public string LeadSourceValue { get; set; }
        public string WebUserSelectedClass { get; set; }
        public string ClassOid { get; set; }
        public string VehicleName { get; set; }
        public string VehicleId { get; set; }
        public string LocationInsuranceCompanyOidsAndNames { get; set; }
        public string Comments { get; set; } // only for rmxaffiliate system (When mode.txt is "Event", it has some values)
        public string WebUserSelectedCountry { get; set; }
        public string EquipmentTypeOids { get; set; }
        public string FeeOids { get; set; }
        public string EquipmentTypeNames { get; set; } // only for rackspace webservice
        public string FeeNames { get; set; } // only for rackspace webservice
        public string OldVehicleID { get; set; } // only for rackspace webservice
    }
}