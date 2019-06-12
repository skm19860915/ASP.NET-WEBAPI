using rmxwebapi.Models;
using rmxwebapi.ViewModels.RackSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers.RackSpace
{
    public class VehicleInfoController : ApiController
    {
        private DataRepository _repo;

        public VehicleInfoController()
        {
            _repo = new DataRepository();
        }
        [Route("RackSpace")]
        public async Task<List<VehicleInfoViewModel>> Get()
        {
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var list = new List<VehicleInfoViewModel>();
            foreach(var item in vehicles)
            {
                var record = new VehicleInfoViewModel()
                {
                    VehicleOid = item.Oid,
                    QuickFindKeyWord = item.QuickFindKeyWord
                };
                list.Add(record);
            }
            return await Task.FromResult(list);
        }

        [Route("RackSpace/{id}")]
        public async Task<string> Get(Guid id)
        {
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var record = vehicles.FirstOrDefault(x => x.Oid == id);
            if (record == null)
                return null;

            var keyword = record.QuickFindKeyWord;
            return await Task.FromResult(keyword);
        }
    }
}
