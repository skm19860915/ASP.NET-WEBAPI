using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rmxwebapi.ViewModels
{
    public class VehicleImagesInLocationViewModel
    {
        public Guid? Vehicle { get; set; }
        public byte[] Photo { get; set; }
    }
}