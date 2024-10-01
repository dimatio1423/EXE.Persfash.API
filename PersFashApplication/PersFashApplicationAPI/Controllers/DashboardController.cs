using BusinessObject.Models.DashboardModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AdminServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public DashboardController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewDashboard([FromQuery] DashboardReqModel dashboardReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _adminService.ViewDashboard(token, dashboardReqModel.startDate, dashboardReqModel.endDate);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View dashboard successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }
    }
}
