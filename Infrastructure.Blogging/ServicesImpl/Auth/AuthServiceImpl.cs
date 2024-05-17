using Application.Blogging.Auth;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.Entities;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Blogging.RepoInterface.UserRepoInterface;
using Infrastructure.Blogging.utils;
using System.Net;

namespace Infrastructure.Blogging.ServicesImpl.Auth
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly GenericFileUtils genericFileUtils;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepo _userRepo;
        private readonly EmailService _emailService;

        

        public AuthServiceImpl(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            GenericFileUtils genericFileUtils, ApplicationDbContext dbContext, IUserRepo userRepo, EmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            this.genericFileUtils = genericFileUtils;
            _dbContext = dbContext;
            _userRepo = userRepo;
            _emailService = emailService;
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            AppUser user = await _userRepo.FindByEmail(email);
        

            // Generate the password reset token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode the token
            string encodedToken = WebUtility.UrlEncode(token);

            // Construct the reset password URL
            string resetUrl = $"{_configuration["FrontUrl"]}/reset-password/{user.Email}/{encodedToken}";
            string emailBody = $"<a href='{resetUrl}'>Click here to reset your password.</a>";

            // Send password reset mail to user email
             _emailService.SendEmailAsync(email, "Click link to reset password", emailBody);
            return "Check your email";
        }

        public async Task registerUser(RegisterViewModel model)
        {


            var user = new AppUser { UserName = model.Username, Email = model.Email, CreatedAt = DateTime.UtcNow,  };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                throw new Exception("Invalid role specified.");
            }

            // for user profile picture, saving selected file in a new locaation and assign
            if (model.FileId != null)
            {
                TemporaryAttachments tempAttach = await _dbContext.TemporaryAttachments.FirstOrDefaultAsync(s => s.Id == model.FileId);

                // copying the user selected file from temporary folder to picture folder
                user.ProfilePath = genericFileUtils.CopyFileToServer(tempAttach.Location, FilePathMapping.BLOG_PICTURE, FilePathConstants.TempPath);
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            else
            {

                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task ResetUserPassword(ResetPasswordViewModel model)
        {
            AppUser user = await _userRepo.FindByEmail(model.Email);
         

            // Resetting password
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed.");
            }

        }

        public async Task<AuthViewResponse> login(LoginViewModel model)
        {

            AppUser user = await _userRepo.FindByEmail(model.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (result)
            {
                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName),
                    }),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new AuthViewResponse() { 
                    jwtToken= tokenHandler.WriteToken(token) ,
                    roles = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    username = user.UserName,
                    userId = user.Id

            }; ;
            }

            throw new Exception("Invalid password.");
        }

       

     
    }
}
