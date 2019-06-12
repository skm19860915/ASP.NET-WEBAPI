using System;

namespace rmxwebapi.Models
{
    public class VehicleMediaItemModel
    {
        public Guid? Vehicle { get; set; }
        public byte[] Photo { get; set; }
        public int? PhotoCode { get; set; }
    }
}