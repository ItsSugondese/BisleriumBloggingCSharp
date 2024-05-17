using Application.Blogging.BlogApp;
using Application.Blogging.NotificationApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Entities;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.NotficationView;
using Infrastructure.Blogging.utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class CommentReactMappingServiceImpl : ICommentReactMappingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICommentReactRepo _reactRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public CommentReactMappingServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, ICommentReactRepo reactRepo, ICommentRepo commentRepo,
           INotificationService notificationService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _reactRepo = reactRepo;
            _commentRepo = commentRepo;
            _notificationService = notificationService;
        }

        public async Task SaveCommentReaction(CommentReactMappingViewModel model)
        {
            // getting CommentReactMapping class if user have reacted to the comment 
            CommentReactMapping? mapping = await _reactRepo.GetByUserIdAndCommentId(_tokenService.GetUserIdFromToken(), model.CommentId);

            // if user haven't reaceted to the selected comment yet
            if (mapping == null)
            {
                mapping = new CommentReactMapping();
                mapping.Comment = await _commentRepo.FindById(model.CommentId);
                mapping.User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken());
                mapping.CreatedAt = DateTime.UtcNow;
                _dbContext.Add(mapping);

            }
            else
            {
                // if last reaction is same as current reaction, remove the last reacted data from database 
                if (mapping.Reaction == model.Reaction.ToString())
                {
                    _dbContext.CommentReactMappings.Remove(mapping);
                }
            }

            mapping.Reaction = model.Reaction.ToString();
            await _dbContext.SaveChangesAsync();   
        }
    }
}
