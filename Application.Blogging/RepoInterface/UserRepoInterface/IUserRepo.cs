using Domain.Blogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.UserRepoInterface
{
    public interface IUserRepo
    {
        Task<Dictionary<string, object>> GetUserProfileFromToken(string id);
        Task<AppUser> FindById(string id);
        Task<Object> testing();

    }
}
