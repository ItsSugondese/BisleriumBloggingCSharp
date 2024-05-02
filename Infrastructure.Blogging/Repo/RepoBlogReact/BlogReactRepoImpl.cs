using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class BlogReactRepoImpl : IBlogReactRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogReactRepoImpl(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<List<BlogReactMapping>> GetAllByBlogId(int blogId)
        {
            return await _dbContext.BlogReactMappings
        .FromSqlRaw("SELECT * FROM \"BlogReactMappings\" brm WHERE brm.\"BlogId\" = {0}",  blogId)
        .ToListAsync();
        }

        public async Task<BlogReactMapping?> GetByUserIdAndBlogId(string userId, int blogId)
        {
           return await _dbContext.BlogReactMappings
        .FromSqlRaw("SELECT * FROM \"BlogReactMappings\" brm WHERE brm.\"UserId\" = {0} AND brm.\"BlogId\" = {1}", userId, blogId)
        .FirstOrDefaultAsync();

        }
    }
}
