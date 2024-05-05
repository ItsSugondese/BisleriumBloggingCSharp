using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class CommentServiceImpl : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogRepo _blogRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly ICommentReactRepo _reactRepo;
        private readonly ICommentHistoryService _historyService;

        public CommentServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, IBlogRepo blogRepo,
           ICommentRepo commentRepo, ICommentReactRepo reactRepo, ICommentHistoryService historyService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
            _reactRepo = reactRepo;
            _historyService = historyService;
        }

        public async Task deleteComment(int id)
        {

            string commentHistory = $@"delete from ""CommentHistory"" crm  where crm.""CommentId"" = {id}";
            ConnectionStringConfig.deleteData(commentHistory);

            Comments comment = await _commentRepo.FindById(id);
            _dbContext.CommentReactMappings.RemoveRange(await _reactRepo.GetAllByCommentId(id));
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

        }

        

        public async Task saveComment(CommentViewModel model)
        {

            Comments comment;

            if (model.Id != null)
            {
                comment = await _commentRepo.FindById((int)model.Id);
            }
            else
            {
                // Insert scenario: Create a new blog instance
                comment = new Comments
                {
                    CreatedAt = DateTime.UtcNow,
                    User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken()),
                    Blog = await _blogRepo.FindById(model.BlogId),
                };

                _dbContext.Add(comment);
                await _dbContext.SaveChangesAsync();
            }

            await _historyService.SaveHistory(model, comment);
        }
    }
}
