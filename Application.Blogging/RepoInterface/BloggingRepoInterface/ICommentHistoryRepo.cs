using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.BloggingRepoInterface
{
    public interface ICommentHistoryRepo
    {
        List<Dictionary<string, object>> GetCommentHistoryBasicDetalsByCommentId(int commentId);
    }
}
