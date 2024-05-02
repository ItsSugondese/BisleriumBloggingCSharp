using Application.Blogging.RepoInterface.UserRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{

    [Route("api/user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : GenericController
    {
        private readonly IUserRepo _userRepo;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(IUserRepo userRepo, JwtTokenService jwtTokenService)
        {
            _userRepo = userRepo;
            _jwtTokenService = jwtTokenService;
            this.moduleName = ModuleNameConstant.USER;
        }

        [HttpGet("profile")]
        public async Task<Object> GetUserProfile()
        {

            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName), CrudStatus.GET, 
                await _userRepo.GetUserProfileFromToken(_jwtTokenService.GetUserIdFromToken()));
        }

        [HttpGet("photo/{id}")]
        public async Task<Object> GetDocs(string id)
        {
            AppUser user = await _userRepo.FindById(id);
            string? photoPath = user.ProfilePath;

            if (photoPath != null && !string.IsNullOrEmpty(photoPath))
            {
                Byte[] b = System.IO.File.ReadAllBytes(photoPath);
                return File(b, "image/jpeg");
            }

            return null;
        }
    [HttpGet("test")]
        public async Task<Object> Testing()
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName), CrudStatus.GET,
                await _userRepo.testing());
        }
    
    }
    
}
