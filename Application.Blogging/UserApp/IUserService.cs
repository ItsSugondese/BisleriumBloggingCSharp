using Domain.Blogging.view.UserView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.UserApp
{
    public interface IUserService
    {
        Task updateUser(UpdateUserViewModel model);
        Task DeleteUserAccount(string id);

    }
}
