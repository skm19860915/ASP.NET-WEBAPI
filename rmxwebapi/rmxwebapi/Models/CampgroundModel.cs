namespace rmxwebapi.Models
{
    public class CampgroundModel
    {
        public double? Id { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Website { get; set; }
        public double? Zip { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool DeleteFlag { get; set; }
        public string DeleteDate { get; set; }
    }
}