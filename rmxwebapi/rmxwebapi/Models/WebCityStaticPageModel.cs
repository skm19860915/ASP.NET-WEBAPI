using System;

namespace rmxwebapi.Models
{
    public class WebCityStaticPageModel
    {
        public Guid? Location { get; set; }
        public string AnsweringPageURL { get; set; }
        public string MetaTitle { get; set; }
        public string HTMLPageContent { get; set; }
        public bool? IsPointOfInterest { get; set; }
        public bool? IsEvent { get; set; }
    }
}