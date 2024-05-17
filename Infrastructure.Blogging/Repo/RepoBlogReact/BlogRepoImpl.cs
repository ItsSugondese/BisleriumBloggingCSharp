using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using Infrastructure.Blogging.utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class BlogRepoImpl : IBlogRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenService _jwtTokenService;

        public BlogRepoImpl(ApplicationDbContext dbContext, JwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Blog> FindById(int id)
        {
            Blog? blog = await _dbContext.Blog
                .Include(b => b.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (blog == null)
            {
                throw new Exception(MessageConstantMerge.notExist("id", ModuleNameConstant.BLOG));
            }
            return blog;
        }

        public  Dictionary<string, object> GetBlogBasicDetailsByBlogId(int blogId)
        {
            string userId = "";

            try
            {
                userId = _jwtTokenService.GetUserIdFromToken();
            }
            catch (Exception e)
            {
            }
            string queryString = $@"select * from(select * from (select 
b.""Id"" as id,
anu.""UserName"" as username ,
to_char(b.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') as ""postedOn"",
anu.""ProfilePath"" as ""userProfile"", anu.""Id"" as ""userId"", b.""CreatedAt"",
case when b.""UserId""  = '{userId}' then true else false end as ""myBlog"",
anu.""ProfilePath"" as ""userProfile"", anu.""Id"" as ""userId"", b.""CreatedAt"",
coalesce ((select sum(case when brm.""Reaction""  = 'UPVOTE' then 2 else -1 end) from ""BlogReactMappings"" brm where brm.""BlogId"" = b.""Id""),0) score,
coalesce ((select count(*) from ""Comments"" c where ""BlogId"" = b.""Id""), 0) comments,
coalesce ((select case when brm2.""Reaction"" = 'UPVOTE' then true else false end from ""BlogReactMappings"" brm2 
where brm2.""UserId"" = '{userId}' and brm2.""BlogId"" = b.""Id""), null) ""hasReacted""
from ""Blog"" b  join ""AspNetUsers"" anu ON b.""UserId"" = anu.""Id"" 
where b.""Id"" = {blogId}) parent
join lateral 
(
select bh.""Id"" as ""historyId"", bh.""Title"" as title, bh.""Content"" as content, bh.""ImagePath"" as ""imageUrl""
from ""BlogHistory"" bh where bh.""BlogId"" = parent.id 
order by bh.""CreatedAt"" desc
limit 1
)child on true)foo";

            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, queryString);
            var result = resultList.FirstOrDefault();

            if(result == null)
            {
                throw new Exception(MessageConstantMerge.notExist("Id", ModuleNameConstant.BLOG));
            }
            return result;
        }

        public async Task<Dictionary<string, object>> GetBlogPaginataed(BlogPaginationViewModel model)
        {
            
            var total = _dbContext.Blog.Count();
            string userId = "";

            try
            {
                userId = _jwtTokenService.GetUserIdFromToken();
            }catch(Exception e)
            {
            }
            var pageCount = Math.Ceiling(_dbContext.Blog.Count() / (float)model.Row);
            string queryString = $@"select *, (foo.score + foo.comments) ""totalPoint"" from(select * from (select 
b.""Id"" as id,
anu.""UserName"" as username ,
to_char(b.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') as ""postedOn"",
anu.""ProfilePath"" as ""userProfile"", anu.""Id"" as ""userId"", b.""CreatedAt"",
coalesce ((select sum(case when brm.""Reaction""  = 'UPVOTE' then 2 else -1 end) from ""BlogReactMappings"" brm where brm.""BlogId"" = b.""Id""),0) score,
coalesce ((select count(*) from ""Comments"" c where ""BlogId"" = b.""Id""), 0) comments,
coalesce ((select case when brm2.""Reaction"" = 'UPVOTE' then true else false end from ""BlogReactMappings"" brm2 
where brm2.""UserId"" = '{userId}' and brm2.""BlogId"" = b.""Id""), null) ""hasReacted"",
coalesce ((select sum(case when brm.""Reaction"" = 'UPVOTE' then 1 else 0 end) from ""BlogReactMappings"" brm where brm.""BlogId"" = b.""Id""),0) upvote,
coalesce ((select sum(case when brm.""Reaction"" = 'DOWNVOTE' then 1 else 0 end) from ""BlogReactMappings"" brm where brm.""BlogId"" = b.""Id""),0) downvote
from ""Blog"" b  join ""AspNetUsers"" anu ON b.""UserId"" = anu.""Id"" 
where case when {model.isAll} is true then true else cast(b.""CreatedAt"" as date) between cast('{model.fromDate}' as date) and cast('{model.toDate}' as date) end and 
case when '{model.name}' = '' then true else anu.""UserName""  ilike  concat('%','{model.name}' ,'%')   end and 
case when {model.ofUser} is true then b.""UserId"" = '{userId}' else true end) parent
join lateral 
(
select bh.""Id"" as ""historyId"", bh.""Title"" as title, bh.""Content"" as content, bh.""ImagePath"" as imageUrl
from ""BlogHistory"" bh where bh.""BlogId"" = parent.id 
order by bh.""CreatedAt"" desc
limit 1
)child on true) foo
ORDER BY 
    CASE 
        WHEN '{model.sort.ToString()}' = 'RANDOM' THEN RANDOM() 
        END,
    CASE 
        WHEN '{model.sort.ToString()}' = 'POPULAR' THEN (foo.score + foo.comments)
        END DESC,
    foo.""CreatedAt"" DESC
LIMIT {model.Row} OFFSET {(model.Page - 1) * model.Row}";

            // Create a list to store the dictionaries
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, queryString);

            var userDictionary = new Dictionary<string, object>();

            userDictionary.Add("content", resultList);
            userDictionary.Add("totalPages", pageCount);
            userDictionary.Add("totalElements", total);
            userDictionary.Add("numberOfElements", resultList.Count);
            userDictionary.Add("currentPageIndex", model.Page);


            return userDictionary;            
        }
    }
}
