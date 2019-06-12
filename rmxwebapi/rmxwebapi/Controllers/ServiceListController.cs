using rmxwebapi.Models;
using rmxwebapi.Utility;
using rmxwebapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class ServiceListController : ApiController
    {
        private GeneralUtility _utility;
        private DataRepository _repo;

        private List<EquipmentTypeModel> equipmentTypeList;
        private List<FeeModel> feeList;
        private List<LocationInsuranceCompanyModel> locationInsuranceCompanyList;
        private List<LeadSourceModel> leadSourceList;

        public ServiceListController()
        {
            _utility = new GeneralUtility();
            _repo = new DataRepository();
        }

        public async Task<ServiceListViewModel> Get(Guid id, string tag, string token)
        {
            var isAvailable = _utility.CheckAvailableToken(token);
            if (!isAvailable)
                return null;

            var fees = await Task.FromResult(_repo.GetAllFees());
            var equipmentTypes = await Task.FromResult(_repo.GetAllEquipmentTypes());
            var locationInsuranceCompanies = await Task.FromResult(_repo.GetAllLocationInsuranceCompanies());
            var leadSources = await Task.FromResult(_repo.GetAllLeadSources());

            if (equipmentTypes != null && equipmentTypes.Count() > 0)
                equipmentTypeList = equipmentTypes.Where(x => x.Tag.ToLower().Contains(tag)).ToList();

            if (fees != null && fees.Count() > 0)
                feeList = fees.Where(x => x.Tag.ToLower().Contains(tag)).ToList();

            if (locationInsuranceCompanies != null && locationInsuranceCompanies.Count() > 0)
                locationInsuranceCompanyList = locationInsuranceCompanies.Where(x => x.Location == id).ToList();

            if (leadSources != null && leadSources.Count() > 0)
            {
                var availableLeadSources = leadSources.Where(x => x.IsSystemObject == true || x.Location == id);
                if (availableLeadSources != null && availableLeadSources.Count() > 0)
                    leadSourceList = availableLeadSources.Where(x => x.ShowOnWeb == true).ToList();
            }

            var list = new ServiceListViewModel()
            {
                EquipmentTypeList = equipmentTypeList,
                FeeList = feeList,
                LocationInsuranceCompanyList = locationInsuranceCompanyList,
                LeadSourceList = leadSourceList
            };

            return await Task.FromResult(list);
        }

        public string Get(Guid id, string tag)
        {
            return _utility._noAuthorizationMessage;
        }
    }
}
