using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class OptionalEquipmentController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        public OptionalEquipmentController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<List<OptionalEquipmentViewModel>> Get(long id, string tag, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var locations = await Task.FromResult(_repo.GetAllLocations());
            if (locations == null || locations.Count() < 1)
                return null;

            var equipmentTypes = await Task.FromResult(_repo.GetAllEquipmentTypes());
            var fees = await Task.FromResult(_repo.GetAllFees());
            var locationOid = locations.FirstOrDefault(x => x.SequenceId == id).Oid;
            var list = new List<OptionalEquipmentViewModel>();

            if (equipmentTypes != null && equipmentTypes.Count() > 0)
            {
                var filteredEquipmentTypes = equipmentTypes.Where(x => x.Tag.ToLower().Contains(tag)).ToList();
                var targetEquipmentTypes = filteredEquipmentTypes.Where(x => x.Location == locationOid).ToList();
                foreach (var item in targetEquipmentTypes)
                {
                    var optionalEquipmentRecord = new OptionalEquipmentViewModel()
                    {
                        Name = item.Name,
                        WebPrice = GetCurrencyValue(item.WebPrice)
                    };
                    list.Add(optionalEquipmentRecord);
                }
            }
            
            if(fees != null && fees.Count() > 0)
            {
                var filteredFees = fees.Where(x => x.Tag.ToLower().Contains(tag)).ToList();
                var targetFees = filteredFees.Where(x => x.Location == locationOid).ToList();
                foreach (var item in targetFees)
                {
                    var feeRecord = new OptionalEquipmentViewModel()
                    {
                        Name = item.Name,
                        WebPrice = GetCurrencyValue(item.WebPrice)
                    };
                    list.Add(feeRecord);
                }
            }
            
            return list;
        }

        public string Get(long id, string tag)
        {
            return _utility._noAuthorizationMessage;
        }

        private string GetCurrencyValue(string webPrice)
        {
            if (string.IsNullOrEmpty(webPrice))
                return "$0";
            return webPrice;
        }
    }
}
