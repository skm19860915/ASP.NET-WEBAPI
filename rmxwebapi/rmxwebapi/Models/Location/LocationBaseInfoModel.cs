using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rmxwebapi.Models.Location
{
    public class LocationBaseInfoModel
    {
        public long? SequenceId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string WebRegionalName { get; set; }
    }
}