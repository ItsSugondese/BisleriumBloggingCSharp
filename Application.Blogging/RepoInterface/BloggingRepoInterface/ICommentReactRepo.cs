using Domain.Blogging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.BloggingRepoInterface
{
    public interface ICommentReactRepo
    {
        Task<CommentReactMapping?> GetByUserIdAndCommentId(string userId, int CommentId);
        Task<List<CommentReactMapping>> GetAllByCommentId(int CommentId);

    }
}
