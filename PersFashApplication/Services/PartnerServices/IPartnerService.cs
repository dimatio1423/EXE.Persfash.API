using BusinessObject.Models.PartnerModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PartnerServices
{
    public interface IPartnerService 
    {
        Task<List<PartnerViewModel>> RecommendPartnerForCustomer(string token, int? page, int? size);
    }
}
