using Application.Blogging.RepoInterface.UserRepoInterface;
using Application.Blogging.UserApp;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view.UserView;
using Domain.Blogging.view.UserView.PaginationForUsers;
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
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(IUserRepo userRepo, JwtTokenService jwtTokenService, IUserService userService)
        {
            _userRepo = userRepo;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            this.moduleName = ModuleNameConstant.USER;
        }

        [HttpPost]
        public async Task<Object> SaveUser(UpdateUserViewModel model)
        {
            await _userService.updateUser(model);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.SAVE,
               true);
        }

        [HttpPost("paginated")]
        public async Task<Object> GetAllUsers(UserPaginationViewModel model)
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName), CrudStatus.GET, 
                await _userRepo.GetAllUsersBasicDetailsPaginated(model));
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

        [HttpDelete("photo/{id}")]
        public async Task<Object> DeleteUserImage(string id)
        {
            await _userRepo.DeleteProfilePicByUserId(id);
            return SuccessResponse("Image deleted successfully",
                CrudStatus.SAVE,
               true);
        }
        
        [HttpDelete("{id}")]
        public async Task<Object> DeleteUser(string id)
        {
            await _userService.DeleteUserAccount(id);
            return SuccessResponse("User deleted successfully",
                CrudStatus.SAVE,
               true);
        }
        

        [HttpGet("test")]
        public async Task<Object> Testing()
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName), CrudStatus.GET,
                await _userRepo.testing());
        }
    
    }
    
}
