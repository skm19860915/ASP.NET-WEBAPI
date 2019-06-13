using rmxwebapi.Models;
using rmxwebapi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Reflection;
using rmxwebapi.ViewModels;
using rmxwebapi.Models.Location;

namespace rmxwebapi.Controllers
{
    public class LocationController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public LocationController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        [Route("api/location")]
        public async Task<IEnumerable<LocationModel>> GetAllInfos(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            return await Task.FromResult(locations);
        }

        [Route("api/location/base")]
        public async Task<IEnumerable<LocationBaseInfoModel>> GetAllBaseInfos(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());

            var initInfos = from l in locations where l.IsLocationBehindOnPaying != true
                            select new LocationBaseInfoModel
                            {
                                SequenceId = l.SequenceId,
                                City = l.City,
                                State = l.State,
                                WebRegionalName = l.WebRegionalName
                            };

            return await Task.FromResult(initInfos);
        }

        [Route("api/location/{id}")]
        public async Task<LocationModel> GetSpecificInfo(Guid id, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var location = locations.FirstOrDefault(x => x.Oid == id);
            return await Task.FromResult(location);
        }

        [Route("api/location")]
        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }

        [Route("api/location/{id}")]
        public string Get(Guid id)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
