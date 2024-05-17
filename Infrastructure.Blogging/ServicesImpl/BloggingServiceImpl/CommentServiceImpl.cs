using Application.Blogging.BlogApp;
using Application.Blogging.NotificationApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.NotficationView;
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
        private readonly INotificationService _notificationService;


        public CommentServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, IBlogRepo blogRepo,
           ICommentRepo commentRepo, ICommentReactRepo reactRepo, ICommentHistoryService historyService,
           INotificationService notificationService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _blogRepo = blogRepo;
            _commentRepo = commentRepo;
            _reactRepo = reactRepo;
            _historyService = historyService;
            _notificationService = notificationService;
        }

        public async Task deleteComment(int id)
        {
            // check if commetn exists
            Comments comment = await _commentRepo.FindById(id);

            //  qery to delete comment history
            string commentHistory = $@"delete from ""CommentHistory"" crm  where crm.""CommentId"" = {id}";

            // method call to delete comment history
            ConnectionStringConfig.deleteData(commentHistory);

            // add rection on this comment to delete
            _dbContext.CommentReactMappings.RemoveRange(await _reactRepo.GetAllByCommentId(id));

            // add comment to delete
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

        }



        public async Task saveComment(CommentViewModel model)
        {
            Comments comment;
            AppUser user = new AppUser();
            Blog blog = new Blog();

            // if id is not null then updating else creating new
            if (model.Id != null)
            {
                comment = await _commentRepo.FindById((int)model.Id);
            }
            else
            {
                // extracting details of the user that comment using jwt token
                user = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken());
                blog = await _blogRepo.FindById(model.BlogId);
                comment = new Comments
                {
                    CreatedAt = DateTime.UtcNow,
                    User = user,
                    Blog = blog,
                };
                _dbContext.Add(comment);
                await _dbContext.SaveChangesAsync();
            }
            // saving comemnt in comment history
            await _historyService.SaveHistory(model, comment);

            // in the case creating comment, sending notificaiton to the author of the blog
            if (model.Id == null)
            {
                string commenter = user.UserName == blog.User.UserName ? 
                    "You" : comment.User.UserName;
                NotificationViewModel notificationModel = new NotificationViewModel()
                {
                    Message = $"{commenter} commented on your blog.",
                    UserId = blog.User.Id
                };
                await _notificationService.SaveNotification(notificationModel);
            }
        }
    }
}
