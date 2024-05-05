using Domain.Blogging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.BloggingRepoInterface
{
    public interface IBlogHistoryRepo
    {
        Task<List<BlogHistory>> GetAllByBlogId(int blogId);
        Task<BlogHistory> FindById(int id);
        Task<BlogHistory?> FindLatestByBlogId(int id);

        Task deleteImage(int historyId);

        List<Dictionary<string, object>> GetHistoryBasicDetailsByBlogId(int id);
    }
}
