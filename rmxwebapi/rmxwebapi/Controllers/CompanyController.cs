using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class CompanyController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public CompanyController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<CompanyModel> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var companies = await Task.FromResult(_repo.GetAllCompanies().FirstOrDefault(x => x.IsAllowSmartyStreets == true));
            return await Task.FromResult(companies);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
