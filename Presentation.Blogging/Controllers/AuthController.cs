using Application.Blogging.Auth;
using Domain.Blogging.Constant;
using Domain.Blogging.view.auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;
using Domain.Blogging.enums;

namespace Presentation.Blogging.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : GenericController
    {
        private readonly IAuthService authService;
        private string moduleName;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            this.moduleName = ModuleNameConstant.AUTH;
        }

        [HttpPost]
        public async Task<Object> RegisterUser (RegisterViewModel model)
        {
            await authService.registerUser(model);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName), CrudStatus.SAVE, true);
        }
    }
}
