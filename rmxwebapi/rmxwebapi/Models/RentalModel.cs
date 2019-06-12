using System;

namespace rmxwebapi.Models
{
    public class RentalModel
    {
        public Guid? Location { get; set; }
        public Nullable<DateTime> LeaveOn { get; set; }
        public Nullable<DateTime> ReturnOn { get; set; }
        public string Destination { get; set; }
        public Guid? Vehicle { get; set; }
        public Nullable<bool> IsOwnerRenting { get; set; }
        public DateTime? BookedOn { get; set; }
        public Nullable<int> ReferenceEstimateSequenceId { get; set; }
    }
}