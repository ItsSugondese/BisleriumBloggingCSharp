using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class BlogHistoryRepoImpl : IBlogHistoryRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogHistoryRepoImpl(ApplicationDbContext dbContext) { 
        _dbContext = dbContext;
        }

        public async Task deleteImage(int historyId)
        {
            BlogHistory history = await FindById(historyId);

            history.ImagePath = null;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BlogHistory> FindById(int id)
        {
            BlogHistory? blogHistory = await _dbContext.BlogHistory.FirstOrDefaultAsync(s => s.Id == id);

            if (blogHistory == null)
            {
                throw new Exception(MessageConstantMerge.notExist("id", ModuleNameConstant.BLOG));
            }
            return blogHistory;
        }

        public async Task<BlogHistory?> FindLatestByBlogId(int id)
        {
            List<BlogHistory> list = await _dbContext.BlogHistory
       .FromSqlRaw("SELECT * FROM \"BlogHistory\" bh WHERE bh.\"BlogId\" = {0} order by  bh.\"CreatedAt\" desc limit 1", id)
       .ToListAsync();

            BlogHistory? blogHistory = list.FirstOrDefault();

            return blogHistory;


        }

        public async Task<List<BlogHistory>> GetAllByBlogId(int blogId)
        {
            return await _dbContext.BlogHistory
       .FromSqlRaw("SELECT * FROM \"BlogHistory\" bh WHERE bh.\"BlogId\" = {0}", blogId)
       .ToListAsync();
        }

        public List<Dictionary<string, object>> GetHistoryBasicDetailsByBlogId(int id)
        {
            string query = $@"select bh.""Id"" as ""historyId"", bh.""Title"" as title, bh.""Content""  as content, bh.""ImagePath"" as ""imageUrl"", 
to_char(bh.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') 
from ""BlogHistory"" bh where bh.""BlogId""  = {id} order by bh.""CreatedAt"" asc";

            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, query);

            return resultList;
        }
    }
}
