using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Infrastructure.Blogging.utils;
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
        private readonly JwtTokenService _jwtTokenService;

        public CommentRepoImpl(ApplicationDbContext dbContext, JwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
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

        public async Task<List<Dictionary<string, object>>> GetCommentsOfBlogByBlogId(int blogId)
        {
            string userId = _jwtTokenService.GetUserIdFromToken();
            string queryString = $@"select * from (select  c.""Id"" as id, anu.""Id"" as ""userId"", to_char(c.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') ""postedOn"", 
anu.""UserName"" as username, anu.""ProfilePath"" as ""userProfile"",
case when c.""UserId""  = '{userId}' then true else false end as ""myComment"",
coalesce ((select sum(case when crm.""Reaction""  = 'UPVOTE' then 2 else -1 end) from ""CommentReactMappings"" crm where crm.""CommentId""  = c.""Id""),0) score,
coalesce ((select case when crm2.""Reaction"" = 'UPVOTE' then true else false end from ""CommentReactMappings"" crm2 
where crm2.""UserId"" = '{userId}' and crm2.""CommentId"" = c.""Id""), null) ""hasReacted""
from ""Comments"" c  join ""AspNetUsers"" anu on anu.""Id""  = c.""UserId"" 
where c.""BlogId"" = {blogId}) parent  join lateral
(
select ch.""Content"" as content  from ""CommentHistory"" ch where ch.""CommentId"" = parent.id order by ch.""CreatedAt"" desc limit 1
) child on true
order by score desc";

            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, queryString);


            return resultList;
        }
    }


}
