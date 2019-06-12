using rmxwebapi.Models;
using System;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class BaseClassViewModel
    {
        public List<VehicleClassModel> Classes { get; set; }
        public Nullable<int> BaseClass { get; set; }
    }
}