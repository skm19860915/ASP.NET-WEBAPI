using System;

namespace rmxwebapi.Models
{
    public class TestimonialModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Testimonial { get; set; }
        public Guid? Location { get; set; }
        public string Photo { get; set; }
    }
}