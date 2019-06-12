using System;

namespace rmxwebapi.Models
{
    public class EquipmentTypeModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string Name { get; set; }
        public string WebPrice { get; set; }
        public string WebDescription { get; set; }
        public string Tag { get; set; }
    }
}