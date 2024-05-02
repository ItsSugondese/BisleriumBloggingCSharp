using Domain.Blogging.Entities;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.BlogApp
{
    public interface IBlogService
    {
        Task saveBlog(BlogViewModel model);
        Task<Dictionary<string, object>> GetBlogPaginataed(BlogPaginationViewModel model);
        Task<Dictionary<string, object>> GetBlogDetailsById(int blogId);
        Task deleteBlog(int id);

    }
}
