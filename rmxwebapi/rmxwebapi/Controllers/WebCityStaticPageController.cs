using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class WebCityStaticPageController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public WebCityStaticPageController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<IEnumerable<WebCityStaticPageViewModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var urls = await Task.FromResult(_repo.GetAllWebCityStaticPages());
            var locations = await Task.FromResult(_repo.GetAllLocations());
            var result = from u in urls
                         join l in locations on u.Location equals l.Oid
                         select new WebCityStaticPageViewModel
                         {
                             Location = u.Location,
                             MetaTitle = u.MetaTitle,
                             HTMLPageContent = u.HTMLPageContent,
                             IsPointOfInterest = u.IsPointOfInterest,
                             IsEvent = u.IsEvent,
                             IsLocationBehindOnPaying = l.IsLocationBehindOnPaying,
                             ControllerName = GetNode(u.AnsweringPageURL, true),
                             ActionName = GetNode(u.AnsweringPageURL, false)
                         };

            return await Task.FromResult(result);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        private string GetNode(string url, bool isControllerName)
        {
            var urlNode = url.Split('/');
            if (isControllerName)
                return urlNode[0].Trim();

            return urlNode[1].Trim();
        }
    }
}
