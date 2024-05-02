using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Domain.Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class CommentReactRepoImpl : ICommentReactRepo
    {

        private readonly ApplicationDbContext _dbContext;

        public CommentReactRepoImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CommentReactMapping>> GetAllByCommentId(int CommentId)
        {
            return await _dbContext.CommentReactMappings
       .FromSqlRaw("SELECT * FROM \"CommentReactMappings\" brm WHERE brm.\"CommentId\" = {0}", CommentId)
       .ToListAsync();
        }

        public async Task<CommentReactMapping?> GetByUserIdAndCommentId(string userId, int CommentId)
        {
            return await _dbContext.CommentReactMappings
        .FromSqlRaw("SELECT * FROM \"CommentReactMappings\" brm WHERE brm.\"UserId\" = {0} AND brm.\"CommentId\" = {1}", userId, CommentId)
        .FirstOrDefaultAsync();
        }
    }
}
