using Application.Blogging.BlogApp;
using Application.Blogging.DashboardApp;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    [Route("api/dashboard")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DashboardController : GenericController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService) 
        {
            _dashboardService = dashboardService;
        }

        [HttpPost("sum-data")]
        public async Task<Object> GetBlogById(DateRangeViewModel model)
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.GET,
                _dashboardService.GetSumData(model));
        }
    }
}
