using rmxwebapi.Models;
using rmxwebapi.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class AllCityPageController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public AllCityPageController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<string>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var pages = await Task.FromResult(_repo.GetAllWebCityStaticPages());
            var urls = pages.Select(x => x.AnsweringPageURL).ToList();
            return await Task.FromResult(urls);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
