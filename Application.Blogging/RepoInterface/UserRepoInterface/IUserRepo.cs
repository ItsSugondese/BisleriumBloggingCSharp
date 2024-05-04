using Domain.Blogging;
using Domain.Blogging.view.UserView.PaginationForUsers;
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
        Task<Dictionary<string, object>> GetAllUsersBasicDetailsPaginated(UserPaginationViewModel model);
        Task<AppUser> FindById(string id);
        Task<AppUser> FindByEmail(string email);
        Task DeleteProfilePicByUserId(string id);
        Task<Object> testing();

    }
}
