using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Entities;
using Domain.Blogging.view.BLogView;
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

        public CommentReactMappingServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, ICommentReactRepo reactRepo, ICommentRepo commentRepo)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _reactRepo = reactRepo;
            _commentRepo = commentRepo;
        }

        public async Task SaveCommentReaction(CommentReactMappingViewModel model)
        {
            CommentReactMapping? mapping = await _reactRepo.GetByUserIdAndCommentId(_tokenService.GetUserIdFromToken(), model.CommentId);

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
