using Application.Blogging.BlogApp;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging;
using Domain.Blogging.view.BLogView;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Blogging.Repo.RepoBlogReact;
using Domain.Blogging.Entities;
using Application.RepoInterface.BlogginRepoInterface;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.Blogging.NotificationApp;
using Domain.Blogging.view.NotficationView;
using Application.Blogging.RepoInterface.NotificationRepoInterface;

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class BlogReactMappingServiceImpl : IBlogReactMappingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBlogReactRepo _reactRepo;
        private readonly IBlogRepo _blogRepo;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepo _notificationRepo;
        private readonly MyHub _hub;


        public BlogReactMappingServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, IBlogReactRepo reactRepo, IBlogRepo blogRepo,
           INotificationService notificationService, MyHub hub, INotificationRepo notificationRepo)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _reactRepo = reactRepo;
            _blogRepo = blogRepo;
            _notificationService = notificationService;
            _hub = hub;
            _notificationRepo = notificationRepo;
        }
        public async Task SaveBlogReaction(BlogReactMappingViewModel model)
        {
            // getting BlogReactMapping class if user have reacted to the blog 
            BlogReactMapping? mapping = await _reactRepo.GetByUserIdAndBlogId(_tokenService.GetUserIdFromToken(), model.BlogId);
            bool latest = false;

            // if user haven't reaceted to the selected blog yet
            if (mapping == null)
            {
                latest = true;
                mapping = new BlogReactMapping();
                mapping.Blog = await _blogRepo.FindById(model.BlogId);
                mapping.User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken());
                mapping.CreatedAt = DateTime.UtcNow;
                _dbContext.Add(mapping);
            }
            else
            {
                // if last reaction is same as current reaction, remove the last reacted data from database 
                if(mapping.Reaction == model.Reaction.ToString())
                {
                    _dbContext.BlogReactMappings.Remove(mapping);
                }
            }

            mapping.Reaction = model.Reaction.ToString();
            await _dbContext.SaveChangesAsync();

            // if new comment, sent notification to to auther of the blog
            if (latest)
            {
                string message = $"Someone {model.Reaction} your post.";
                string userId = mapping.Blog.User.Id;
                NotificationViewModel notificationModel = new NotificationViewModel()
                {
                    Message = message,
                    UserId = userId
                };
                await _notificationService.SaveNotification(notificationModel);
                //NotificationSocketViewModel notModel = new NotificationSocketViewModel()
                //{
                //    userId = message,
                //    count = (int) _notificationRepo.UserUnreadNotificationCount(userId)
                //};
                //    _hub.SendNotification(notModel);
            }

        }
    }
}
