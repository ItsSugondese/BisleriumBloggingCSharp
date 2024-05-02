using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using Infrastructure.Blogging.Repo.RepoBlogReact;
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

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class BlogServiceImpl : IBlogService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogReactRepo _blogReactRepo;
        private readonly IBlogRepo _blogRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly GenericFileUtils genericFileUtils;


        public BlogServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, GenericFileUtils genericFileUtils, IBlogReactRepo blogReactRepo, IBlogRepo blogRepo,
           ICommentRepo commentRepo)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            this.genericFileUtils = genericFileUtils;
            _blogReactRepo = blogReactRepo;
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
        }

        public async Task deleteBlog(int id)
        {
            Blog blog = await _blogRepo.FindById(id);
            _dbContext.BlogReactMappings.RemoveRange(await _blogReactRepo.GetAllByBlogId(id));
            _dbContext.Blog.Remove(blog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dictionary<string, object>> GetBlogDetailsById(int blogId)
        {
            Blog blog = await _blogRepo.FindById(blogId);

            Dictionary<string, object> dic = _blogRepo.GetBlogBasicDetailsByBlogId(blogId);
            dic.Add("commentDetails", await _commentRepo.GetCommentsOfBlogByBlogId(blogId));
            

            return dic;

        }

        public async Task<Dictionary<string, object>> GetBlogPaginataed(BlogPaginationViewModel model)
        {
            return await _blogRepo.GetBlogPaginataed(model);

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
