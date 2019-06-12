using System;

namespace rmxwebapi.Models
{
    public class LeadSourceModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string Name { get; set; }
        public bool? IsSystemObject { get; set; }
        public bool? ShowOnWeb { get; set; }
    }
}