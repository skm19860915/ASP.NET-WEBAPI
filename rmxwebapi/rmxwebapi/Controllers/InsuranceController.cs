using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class InsuranceController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;
        public InsuranceController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<LocationInsuranceCompanyModel>> Get(long id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var insuranceCompanies = await Task.FromResult(_repo.GetAllLocationInsuranceCompanies());
            if (insuranceCompanies == null || insuranceCompanies.Count() < 1)
                return null;

            var locationOid = locations.FirstOrDefault(x => x.SequenceId == id).Oid;
            var targetInsuranceCompanies = insuranceCompanies.Where(x => x.Location == locationOid).ToList();
            return await Task.FromResult(targetInsuranceCompanies);
        }

        public string Get(long id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
