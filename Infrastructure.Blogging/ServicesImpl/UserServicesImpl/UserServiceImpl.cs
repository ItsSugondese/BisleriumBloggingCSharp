using Application.Blogging.UserApp;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging;
using Domain.Blogging.view.UserView;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Blogging.RepoInterface.UserRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities.temporary_attachments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Blogging.ServicesImpl.UserServicesImpl
{
    public class UserServiceImpl : IUserService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly GenericFileUtils genericFileUtils;
        private readonly ApplicationDbContext _dbContext;
        
        


        public UserServiceImpl(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            GenericFileUtils genericFileUtils, ApplicationDbContext dbContext, IUserRepo userRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            this.genericFileUtils = genericFileUtils;
            _dbContext = dbContext;
            _userRepo = userRepo;
        }
        public async Task updateUser(UpdateUserViewModel model)
        {
            AppUser user = await _userRepo.FindById(model.Id);

            user.UserName = model.Username;

            if (model.FileId != null)
            {
                TemporaryAttachments tempAttach = await _dbContext.TemporaryAttachments.FirstOrDefaultAsync(s => s.Id == model.FileId);
                user.ProfilePath = genericFileUtils.CopyFileToServer(tempAttach.Location, FilePathMapping.BLOG_PICTURE, FilePathConstants.TempPath);
            }

            await _dbContext.SaveChangesAsync();




        }
    }
}
