using Domain.Blogging.Entities;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.BloggingRepoInterface
{
    public interface IBlogRepo
    {
        Task<Blog> FindById(int id);

        Task<Dictionary<string, object>> GetBlogPaginataed(BlogPaginationViewModel model);
        Dictionary<string, object> GetBlogBasicDetailsByBlogId(int blogId);

    }
}
