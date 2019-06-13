using System;

namespace rmxwebapi.Models
{
    public class WorkspaceFeesAndEquipmentModel
    {
        public Guid Oid { get; set; }
        public Guid? Location { get; set; }
        public string LocationName { get; set; }
        public float? Quantity { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? ExtendedPrice { get; set; }
        public Guid? FeeItemOid { get; set; }
    }
}