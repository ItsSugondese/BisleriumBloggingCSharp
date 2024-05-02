using Domain.Blogging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepoInterface.BlogginRepoInterface
{
    public interface IBlogReactRepo
    {
        Task<BlogReactMapping?> GetByUserIdAndBlogId(string userId, int blogId);
        Task<List<BlogReactMapping>> GetAllByBlogId(int blogId);
    }
}
