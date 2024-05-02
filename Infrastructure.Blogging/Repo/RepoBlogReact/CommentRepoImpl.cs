using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class CommentRepoImpl : ICommentRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepoImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comments>> GetAllByBlogId(int blogId)
        {
            return await _dbContext.Comments
       .FromSqlRaw("SELECT * FROM \"Comments\" c WHERE c.\"BlogId\" = {0}", blogId)
       .ToListAsync();
        }

        public async Task<Comments> FindById(int id)
        {
            Comments? comment = await _dbContext.Comments.FirstOrDefaultAsync(s => s.Id == id);

            if (comment == null)
            {
                throw new Exception(MessageConstantMerge.notExist("id", ModuleNameConstant.COMMENT));
            }
            return comment;
        }
    }


}
