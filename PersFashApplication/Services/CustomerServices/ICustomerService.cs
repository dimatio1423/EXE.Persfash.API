using BusinessObject.Models.CustomerModels.Request;
using BusinessObject.Models.CustomerModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserServices
{
    public interface ICustomerService
    {
        Task<CustomerProfileViewModel> GetCustomerProfile(string token);
        Task CustomerReigster(CustomerRegisterReqModel customerRegisterReqModel);
        Task CustomerProfileSetup(string token, CustomerProfileSetupReqModel customerProfileSetupReqModel);
        Task CustomerProfileSetupUpdate(string token, CustomerProfileSetupUpdateReqModel customerProfileSetupUpdateReqModel);
        Task<CustomerInformationViewModel> GetCustomerInformation(int customerId);
        Task UpdateCustomerInformation(string token, CustomerInformationUpdateReqModel customerInformationUpdateReqModel);
        Task<bool> CheckCustomerProfileExisted(string token);
    }
}
