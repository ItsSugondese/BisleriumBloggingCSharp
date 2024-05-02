using Domain.Blogging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepoInterface.BlogginRepoInterface
{ 
    public interface ICommentRepo
    {
        Task<List<Comments>> GetAllByBlogId(int blogId);
        Task<Comments> FindById(int id);

    }
}
