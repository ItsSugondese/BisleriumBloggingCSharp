using Application.Blogging.BlogApp;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.Entities;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Blogging.TemporaryAttachement;
using Application.Blogging.RepoInterface.BloggingRepoInterface;

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class BlogHistroyServiceImpl : IBlogHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericFileUtils genericFileUtils;
        private readonly IBlogHistoryRepo _blogHistoryRepo;

        public BlogHistroyServiceImpl(ApplicationDbContext context, GenericFileUtils genericFileUtils, IBlogHistoryRepo blogHistoryRepo)
        {
            _context = context;
            this.genericFileUtils = genericFileUtils;
            this._blogHistoryRepo = blogHistoryRepo;
        }
        public async Task SaveBlogHistory(BlogViewModel model, Blog blog)
        {
            BlogHistory? lastBlog = await _blogHistoryRepo.FindLatestByBlogId(blog.Id);

            BlogHistory history = new BlogHistory
            {
                Blog = blog,
                CreatedAt = DateTime.UtcNow,
                Content = model.Content,
                Title = model.Title,
            };

           
            if (model.FileId != null)
            {
                TemporaryAttachments tempAttach = await _context.TemporaryAttachments.FirstOrDefaultAsync(s => s.Id == model.FileId);
                history.ImagePath = genericFileUtils.CopyFileToServer(tempAttach.Location, FilePathMapping.BLOG_PICTURE, FilePathConstants.TempPath);
            }

            if(lastBlog != null && lastBlog.ImagePath != null && model.FileId == null)
            {
                history.ImagePath = lastBlog.ImagePath;
            }
         
              

                _context.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
