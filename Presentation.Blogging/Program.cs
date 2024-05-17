using Application.Blogging.Auth;
using Domain.Blogging;
using Infrastructure.Blogging;
using Infrastructure.Blogging.ServicesImpl.Auth;
using Microsoft.AspNetCore.Identity;
using Presentation.Blogging.OnException;
using System.Text.Json.Serialization;
using Application.Blogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using Application.Blogging.TemporaryAttachement;
using Infrastructure.Blogging.ServicesImpl.temporary_attachments;
using Application.Blogging.BlogApp;
using Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl;
using Infrastructure.Blogging.utils;
using Domain.Blogging.Utils.GenericFile;
using Infrastructure.Blogging.Repo.RepoBlogReact;
using Application.RepoInterface.BlogginRepoInterface;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.Blogging.RepoInterface.UserRepoInterface;
using Infrastructure.Blogging.Repo.RepoUser;
using Application.Blogging.UserApp;
using Infrastructure.Blogging.ServicesImpl.UserServicesImpl;
using Application.Blogging.DashboardApp;
using Infrastructure.Blogging.ServicesImpl.DashbaordImpl;
using Application.Blogging.NotificationApp;
using Infrastructure.Blogging.ServicesImpl.NotificationsImpl;
using Application.Blogging.RepoInterface.NotificationRepoInterface;
using Infrastructure.Blogging.Repo.RepoNotification;

var builder = WebApplication.CreateBuilder(args);

//Add all services
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddScoped<ITemporaryAttachmentsService, TemporaryAttachmentsServiceImpl>();
builder.Services.AddScoped<IBlogService, BlogServiceImpl>();
builder.Services.AddScoped<IBlogReactMappingService, BlogReactMappingServiceImpl>();
builder.Services.AddScoped<ICommentService, CommentServiceImpl>();
builder.Services.AddScoped<ICommentReactMappingService, CommentReactMappingServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IBlogHistoryService, BlogHistroyServiceImpl>();
builder.Services.AddScoped<ICommentHistoryService, CommentHistoryServiceImpl>();
builder.Services.AddScoped<INotificationService, NotificationServiceImpl>();

//solo service 
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<GenericFileUtils>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<MyHub>();

//repo
builder.Services.AddScoped<IBlogReactRepo, BlogReactRepoImpl>();
builder.Services.AddScoped<ICommentRepo, CommentRepoImpl>();
builder.Services.AddScoped<ICommentReactRepo, CommentReactRepoImpl>();
builder.Services.AddScoped<IBlogRepo, BlogRepoImpl>();
builder.Services.AddScoped<IUserRepo, UserRepoImpl>();
builder.Services.AddScoped<IDashboardService, DashboardServiceImpl>();
builder.Services.AddScoped<IBlogHistoryRepo, BlogHistoryRepoImpl>();
builder.Services.AddScoped<ICommentHistoryRepo, CommentHistoryRepoImpl>();
builder.Services.AddScoped<INotificationRepo, NotificationRepoImpl>();


builder.Services.AddDbContext<ApplicationDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorization();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//    };
//});
builder.Services.AddAuthorization();


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CustomExceptionFilterAttribute));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy" ,builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define roles
    string[] roles = { "Admin", "Blogger" };

    // Seed roles if they don't exist
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


app.MapIdentityApi<AppUser>();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MyHub>("/toastr");
});


//app.UseMiddleware<CustomMiddleware>();

app.MapControllers();

app.Run();
