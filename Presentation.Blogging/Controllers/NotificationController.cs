using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.Blogging.RepoInterface.NotificationRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using Domain.Blogging.view.NotficationView;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    [Route("api/notification")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class NotificationController : GenericController
    {

        private readonly INotificationRepo notificationRepo;
        private readonly JwtTokenService _jwtTokenService;

        public NotificationController(INotificationRepo notificationRepo, JwtTokenService jwtTokenService)
        {
            this.notificationRepo = notificationRepo;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("count")]
        public Object GetNotificationCount()
        {
            return SuccessResponse("Image removed successfully",
            CrudStatus.GET,
               notificationRepo.UserUnreadNotificationCount(_jwtTokenService.GetUserIdFromToken()));
        }

        [HttpPost("paginated")]
        public async Task<Object> GetBlogPaginated(NotificationPaginationViewModel requestPojo)
        {

            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.GET,
                await notificationRepo.GetNotificationPaginated(requestPojo));
        }

    }
}
