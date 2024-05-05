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
        private readonly ICommentService _commentService;
        private readonly IBlogHistoryService _blogHistoryService;
        private readonly IBlogHistoryRepo _blogHistoryRepo;
        private readonly GenericFileUtils genericFileUtils;


        public BlogServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, GenericFileUtils genericFileUtils, IBlogReactRepo blogReactRepo, IBlogRepo blogRepo,
           ICommentRepo commentRepo, ICommentService commentService, IBlogHistoryService blogHistoryService, IBlogHistoryRepo blogHistoryRepo)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            this.genericFileUtils = genericFileUtils;
            _blogReactRepo = blogReactRepo;
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
            _commentService = commentService;
            _blogHistoryService = blogHistoryService;
            _blogHistoryRepo = blogHistoryRepo;
        }

        public async Task deleteBlog(int id)
        {

            Blog blog = await _blogRepo.FindById(id);
            List<BlogHistory> histories = await _blogHistoryRepo.GetAllByBlogId(id);

            foreach (var history in histories)
            {
                if (history.ImagePath != null)
                {
                try
                {
                    // Check if the file exists before attempting to delete it
                    if (File.Exists(history.ImagePath))
                    {
                        File.Delete(history.ImagePath);
                        Console.WriteLine("Image deleted successfully.");
                    }
                    else
                    {
                        throw new Exception("Somethign wrong happend when deleing image");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred: " + ex.Message);
                }

                }
            }
          

            string comemntReactQuery = $@"DELETE FROM ""CommentReactMappings"" WHERE ""CommentId"" IN (
    SELECT crm.""CommentId"" FROM ""CommentReactMappings"" crm
    JOIN ""Comments"" c ON c.""Id"" = crm.""CommentId""
    WHERE c.""BlogId"" = {id}
)";

            string commentQuery = $@"delete from ""Comments"" c where c.""BlogId"" = {id}";

            string commentHistory = $@"delete from ""CommentHistory"" crm  where crm.""Id"" in (
select crm.""Id""  from ""CommentHistory""  crm join
""Comments"" c on c.""Id"" = crm.""CommentId"" 
join ""Blog"" b on b.""Id""  = c.""BlogId"" 
where b.""Id"" = {id}
)";

            ConnectionStringConfig.deleteData(comemntReactQuery);
            ConnectionStringConfig.deleteData(commentHistory);
            ConnectionStringConfig.deleteData(commentQuery);

            //await _commentService.deleteComment(id);

            _dbContext.BlogReactMappings.RemoveRange(await _blogReactRepo.GetAllByBlogId(id));
            _dbContext.BlogHistory.RemoveRange(histories);
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
            var userId = _tokenService.GetUserIdFromToken();

                blog = new Blog
                {
                    CreatedAt = DateTime.UtcNow,
                    User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken())
                };

                _dbContext.Add(blog);
            await _dbContext.SaveChangesAsync();
            }

           await  _blogHistoryService.SaveBlogHistory(model, blog);


        }
    }
}
