using Domain.Blogging.view.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.Auth
{
    public interface IAuthService
    {
        Task registerUser(RegisterViewModel model);
        Task<AuthViewResponse> token(LoginViewModel model);
        Task<string> GeneratePasswordResetToken(string email);
        Task ResetUserPassword(ResetPasswordViewModel model);
    }
}
