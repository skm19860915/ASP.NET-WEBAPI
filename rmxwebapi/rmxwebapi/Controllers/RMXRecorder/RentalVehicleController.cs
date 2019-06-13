using rmxwebapi.Models;
using rmxwebapi.ViewModels.RMXRecorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers.RMXRecorder
{
    public class RentalVehicleController : ApiController
    {
        private DataRepository _repo;
        public RentalVehicleController()
        {
            _repo = new DataRepository();
        }

        public async Task<IEnumerable<RentalVehicleViewModel>> Get(long id)
        {
            var targetDate = DateTime.Now.AddDays(2);
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            var rentals = await Task.FromResult(_repo.GetAllRentals());
            var locations = await Task.FromResult(_repo.GetAllLocations());
            var targetLocation = locations.Where(x => x.SequenceId == id);

            var targetRentals = rentals.Where(x => x.BookedOn == null && x.LeaveOn > targetDate || x.BookedOn != null && x.ReturnOn > targetDate).OrderBy(x => x.LeaveOn);
            var vehiclesInLocation = from vehicle in vehicles
                                     join location in targetLocation on vehicle.Location equals location.Oid
                                     select new
                                     {
                                         Oid = vehicle.Oid,
                                         QuickFindKeyWord = vehicle.QuickFindKeyWord,
                                         LicensePlate = vehicle.LicensePlate,
                                         LocationSequenceId = location.SequenceId
                                     };

            var list = from rental in targetRentals
                       join vehicle in vehiclesInLocation on rental.Vehicle equals vehicle.Oid
                       select new RentalVehicleViewModel
                       {
                           ReferenceEstimateSequenceId = rental.ReferenceEstimateSequenceId ?? 0,
                           QuickFindKeyWord = vehicle.QuickFindKeyWord,
                           LicensePlate = vehicle.LicensePlate,
                           LocationSequenceId = vehicle.LocationSequenceId ?? 0
                       };

            return list.OrderBy(x => x.LocationSequenceId);
        }
    }
}
