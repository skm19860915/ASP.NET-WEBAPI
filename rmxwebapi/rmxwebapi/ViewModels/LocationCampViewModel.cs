using rmxwebapi.Models;
using System.Collections.Generic;

namespace rmxwebapi.ViewModels
{
    public class LocationCampViewModel
    {
        public string WebGoogleMapJavaScriptAPIKey { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public List<CampgroundModel> MapList { get; set; }
    }
}