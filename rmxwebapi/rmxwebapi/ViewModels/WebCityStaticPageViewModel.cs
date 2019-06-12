using System;

namespace rmxwebapi.ViewModels
{
    public class WebCityStaticPageViewModel
    {
        public Guid? Location { get; set; }
        public string MetaTitle { get; set; }
        public string HTMLPageContent { get; set; }
        public bool? IsPointOfInterest { get; set; }
        public bool? IsEvent { get; set; }
        public bool? IsLocationBehindOnPaying { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}