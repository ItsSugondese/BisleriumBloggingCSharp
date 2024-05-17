using Application.Blogging.Auth;
using Domain.Blogging.Constant;
using Domain.Blogging.view.auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;
using Domain.Blogging.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Infrastructure.Blogging;

namespace Presentation.Blogging.Controllers
{
   
    [Route("api/auth")]
    [ApiController]

    public class AuthController : GenericController
    {
        private readonly IAuthService authService;
        private readonly MyHub hub;
        private string moduleName;
        public record UserSession(string? Id, string? Name, string? Email, string? Role, MyHub hub);
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            this.moduleName = ModuleNameConstant.AUTH;
            this.hub = hub;
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
                await authService.login(model));
        } 
        
        [HttpGet("forgot-password/{email}")]
        public async Task<Object> ForgotPassword (string email)
        {
            string message = await authService.GeneratePasswordResetToken(email);
            return SuccessResponse(message, CrudStatus.SAVE,
               true);
        }
        
        [HttpPost("reset-password")]
        public async Task<Object> ResetPassword (ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));

                throw new Exception($"Validation failed: {errorMessages}");
            }
            await authService.ResetUserPassword(model);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName), CrudStatus.SAVE,
                true);
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
