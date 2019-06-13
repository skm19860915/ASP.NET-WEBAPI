using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rmxwebapi.ViewModels
{
    public class CustomVehicleMediaItemViewModel
    {
        public Guid? Vehicle { get; set; }
        public PhotoInfoViewModel PhotoInfo { get; set; }
    }
}