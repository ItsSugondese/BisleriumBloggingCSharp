using Application.Blogging.RepoInterface.BloggingRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoBlogReact
{
    public class CommentHistoryRepoImpl : ICommentHistoryRepo
    {
        public List<Dictionary<string, object>> GetCommentHistoryBasicDetalsByCommentId(int commentId)
        {
            string queryString = $@"select ch.""Content"" as content, to_char(ch.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') as ""postedOn""  from ""CommentHistory"" ch 
where ch.""CommentId"" = {commentId} order by ch.""CreatedAt"" asc ";

            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();
            ConnectionStringConfig.getValueFromQuery(resultList, queryString);

            return resultList;

        }
    }
}
