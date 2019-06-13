using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    // search of live sharemycoach
    public class SearchController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public SearchController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<string> Get(string id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            if (vehicles == null || vehicles.Count() < 1)
                return null;

            var vehicle = vehicles.FirstOrDefault(x => x.QuickFindKeyWord == id);
            if (vehicle == null)
                return string.Empty;

            var webUniqueId = vehicle.WebUniqueId;
            return await Task.FromResult(webUniqueId);
        }

        public string Get(string id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
