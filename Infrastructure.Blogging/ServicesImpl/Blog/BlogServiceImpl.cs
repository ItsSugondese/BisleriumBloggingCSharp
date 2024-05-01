using Application.Blogging.BlogApp;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.BlogImpl
{
    public class BlogServiceImpl : IBlogService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly GenericFileUtils genericFileUtils;

        public BlogServiceImpl(ApplicationDbContext dbContext,  IHttpContextAccessor httpContextAccessor, JwtTokenService tokenService,
           UserManager<AppUser> userManager, GenericFileUtils genericFileUtils)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _userManager = userManager;
            this.genericFileUtils = genericFileUtils;
        }

        public async Task saveBlog(BlogViewModel model)
        {
            var userId = _tokenService.GetUserIdFromToken();

            Blog blog;

            if (model.Id != null)
            {
                // Update scenario: Fetch the existing blog from the database
                blog = await _dbContext.Blog.FirstOrDefaultAsync(s => s.Id == model.Id);

                if (blog == null)
                {
                    throw new Exception($"Blog with ID {model.Id} not found.");
                }
            }
            else
            {
                // Insert scenario: Create a new blog instance
                blog = new Blog
                {
                    CreatedAt = DateTime.UtcNow,
                    User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken())
                };

                _dbContext.Add(blog);
            }
            // Update or set properties
            blog.Content = model.Content;
            blog.Title = model.Title;

            // Handle image attachment if provided
            if (model.FileId != null)
            {
                TemporaryAttachments tempAttach = await _dbContext.TemporaryAttachments.FirstOrDefaultAsync(s => s.Id == model.FileId);
                blog.ImagePath = genericFileUtils.CopyFileToServer(tempAttach.Location, FilePathMapping.BLOG_PICTURE, FilePathConstants.TempPath);
            }


            // Save changes to the database
            await _dbContext.SaveChangesAsync();


        }
    }
}
