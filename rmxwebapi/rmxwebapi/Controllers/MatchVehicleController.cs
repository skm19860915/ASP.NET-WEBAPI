using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    // live sharemycoach
    public class MatchVehicleController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public MatchVehicleController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<MatchVehicleViewModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var list = await Task.FromResult(_repo.GetAllMatchVehicles());
            return await Task.FromResult(list.ToList());
        }

        public async Task<MatchVehicleViewModel> Get(int id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            MatchVehicleViewModel record = null;
            var list = await Task.FromResult(_repo.GetAllMatchVehicles());
            record = list.FirstOrDefault(x => x.OldVehicleID == id && x.IsActive == true);

            return await Task.FromResult(record);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        public string Get(int id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
