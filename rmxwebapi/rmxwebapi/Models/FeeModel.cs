using System;

namespace rmxwebapi.Models
{
    public class FeeModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string Name { get; set; }
        public string WebPrice { get; set; }
        public string WebDescription { get; set; }
        public string Tag { get; set; }
        public Nullable<long> SequenceId { get; set; }
    }
}