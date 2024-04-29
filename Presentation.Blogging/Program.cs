using Application.Blogging.Auth;
using Domain.Blogging;
using Infrastructure.Blogging;
using Infrastructure.Blogging.ServicesImpl.Auth;
using Microsoft.AspNetCore.Identity;
using Presentation.Blogging.OnException;
using System.Text.Json.Serialization;
using Application.Blogging;

var builder = WebApplication.CreateBuilder(args);

//Add all services
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
// Add services to the container.


builder.Services.AddDbContext<ApplicationDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CustomExceptionFilterAttribute));
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



app.UseMiddleware<CustomMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
