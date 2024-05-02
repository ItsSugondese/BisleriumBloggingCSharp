using Application.Blogging.Auth;
using Domain.Blogging.Constant;
using Domain.Blogging.view.auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;
using Domain.Blogging.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Presentation.Blogging.Controllers
{
   
    [Route("api/auth")]
    [ApiController]

    public class AuthController : GenericController
    {
        private readonly IAuthService authService;
        private string moduleName;
        public record UserSession(string? Id, string? Name, string? Email, string? Role);
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
        
        [HttpPost("login")]
        public async Task<Object> LoginUser (LoginViewModel model)
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName), CrudStatus.SAVE,
                await authService.token(model));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize]
        [HttpGet()]
        public async Task<Object> test ()
        {
            
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName), CrudStatus.SAVE, true);
        }
    }
}
