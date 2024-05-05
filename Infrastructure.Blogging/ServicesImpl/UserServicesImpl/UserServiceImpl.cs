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

        public async Task DeleteUserAccount(string id)
        {
            AppUser user = await _userRepo.FindById(id);

            string userRole = $@"DELETE FROM public.""AspNetUserRoles""
WHERE ""UserId""='{id}'";

            string blogReact = $@"delete from ""BlogReactMappings"" where ""Id"" in (
select brm.""Id""  from ""BlogReactMappings"" brm join ""Blog"" b on b.""Id"" = brm.""BlogId"" where b.""UserId"" = '{id}'
)";

            string commentReact = $@"delete from ""CommentReactMappings""  where ""Id"" in (
select crm.""Id""  from ""CommentReactMappings""  crm join
""Comments"" c on c.""Id"" = crm.""CommentId"" 
join ""Blog"" b on b.""Id""  = c.""BlogId"" 
where b.""UserId"" = '{id}'
)";

            string blogHistory = $@"delete from ""BlogHistory"" where ""Id"" in (
select brm.""Id""  from ""BlogHistory""  brm join ""Blog"" b on b.""Id"" = brm.""BlogId"" where b.""UserId"" = '{id}'
)";

            string commentHistory = $@"delete from ""CommentHistory""  where ""Id"" in (
select crm.""Id""  from ""CommentHistory""  crm join
""Comments"" c on c.""Id"" = crm.""CommentId"" 
join ""Blog"" b on b.""Id""  = c.""BlogId"" 
where b.""UserId"" = '{id}'
)";

            string comment = $@"delete from ""Comments""  where ""Id"" in (
select c.""Id""  from ""Comments""  c join ""Blog"" b on b.""Id""  = c.""BlogId"" where b.""UserId"" = '{id}'
)";
            string blog = $@"DELETE FROM ""Blog"" b  WHERE b.""UserId""  = '{id}'";





            
            ConnectionStringConfig.deleteData(userRole);
            ConnectionStringConfig.deleteData(commentReact);
            ConnectionStringConfig.deleteData(commentHistory);
            ConnectionStringConfig.deleteData(comment);
            ConnectionStringConfig.deleteData(blogReact);
            ConnectionStringConfig.deleteData(blogHistory);
            ConnectionStringConfig.deleteData(blog);



            await _userRepo.DeleteProfilePicByUserId(id);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();


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
