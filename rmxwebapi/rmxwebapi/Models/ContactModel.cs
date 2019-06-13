using System;

namespace rmxwebapi.Models
{
    public class ContactModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public Guid? Location { get; set; }
    }
}