using System;

namespace rmxwebapi.Models
{
    public class LocationInsuranceCompanyModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string InsuranceCompanyName { get; set; }
    }
}