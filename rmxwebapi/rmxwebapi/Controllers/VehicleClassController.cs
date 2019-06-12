using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    // affiliate system
    public class VehicleClassController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public VehicleClassController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<BaseClassViewModel>> Get(string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var classes = await Task.FromResult(_repo.GetAllVehicleClasses());
            var vehicles = await Task.FromResult(_repo.GetAllVehicles());
            
            var baseClass = (from c in classes
                          join v in vehicles on c.Oid equals v.VehicleClass where c.BaseClass != null
                          select new 
                          {
                              BaseClass = c.BaseClass
                          }).GroupBy(x => x.BaseClass).Select(y => y.FirstOrDefault()).OrderBy(x => x.BaseClass);

            var visibleClasses = (from c in classes
                                   join v in vehicles on c.Oid equals v.VehicleClass
                                   where c.BaseClass != null
                                   select new VehicleClassModel
                                   {
                                       Oid = c.Oid,
                                       BaseClass = c.BaseClass,
                                       ClassType = c.ClassType,
                                       Name = c.Name,
                                       WebBannerName = c.WebBannerName
                                   }).GroupBy(x => x.Oid).Select(y => y.FirstOrDefault()).OrderBy(x => x.BaseClass);

            var list = new List<BaseClassViewModel>();
            foreach(var item in baseClass)
            {
                var targetClasses = visibleClasses.Where(x => x.BaseClass == item.BaseClass).ToList();
                var record = new BaseClassViewModel()
                {
                    Classes = targetClasses,
                    BaseClass = item.BaseClass
                };
                list.Add(record);
            }
            return await Task.FromResult(list);
        }

        public string Get()
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
