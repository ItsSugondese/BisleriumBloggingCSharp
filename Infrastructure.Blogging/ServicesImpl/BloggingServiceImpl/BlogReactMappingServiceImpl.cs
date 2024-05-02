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

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class BlogReactMappingServiceImpl : IBlogReactMappingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBlogReactRepo _reactRepo;
        private readonly IBlogRepo _blogRepo;
        private readonly JwtTokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public BlogReactMappingServiceImpl(ApplicationDbContext dbContext, JwtTokenService tokenService,
           UserManager<AppUser> userManager, IBlogReactRepo reactRepo, IBlogRepo blogRepo)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _reactRepo = reactRepo;
            _blogRepo = blogRepo;
        }
        public async Task SaveBlogReaction(BlogReactMappingViewModel model)
        {
            BlogReactMapping? mapping = await _reactRepo.GetByUserIdAndBlogId(_tokenService.GetUserIdFromToken(), model.BlogId);

            if (mapping == null)
            {
                mapping = new BlogReactMapping();
                mapping.Blog = await _blogRepo.FindById(model.BlogId);
                mapping.User = await _userManager.FindByIdAsync(_tokenService.GetUserIdFromToken());
                mapping.CreatedAt = DateTime.UtcNow;
                _dbContext.Add(mapping);
            }
            else
            {
                if(mapping.Reaction == model.Reaction.ToString())
                {
                    _dbContext.BlogReactMappings.Remove(mapping);
                }
            }

            mapping.Reaction = model.Reaction.ToString();
            await _dbContext.SaveChangesAsync();



        }
    }
}
