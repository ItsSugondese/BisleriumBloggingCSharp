using Application.Blogging.NotificationApp;
using Application.Blogging.RepoInterface.NotificationRepoInterface;
using Domain.Blogging.Entities;
using Domain.Blogging.view.NotficationView;
using Infrastructure.Blogging.utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoNotification
{
    public class NotificationRepoImpl : INotificationRepo
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly ApplicationDbContext _dbContext;

        public NotificationRepoImpl(JwtTokenService jwtTokenService, ApplicationDbContext dbContext)
        {
            _jwtTokenService = jwtTokenService;
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, object>> GetNotificationPaginated(NotificationPaginationViewModel model)
        {
            string query = $@"update  ""Notifications""  set ""IsRead"" = true where ""UserId"" = '{_jwtTokenService.GetUserIdFromToken()}'";
            ConnectionStringConfig.deleteData(query);
            var total = _dbContext.Notifications.Count();
            string userId = _jwtTokenService.GetUserIdFromToken();
            
         
            var pageCount = Math.Ceiling(_dbContext.Blog.Count() / (float)model.Row);
            string queryString = $@"SELECT 
    to_char(n.""CreatedAt"" , 'YYYY-MM-DD HH:MI AM') as ""date"", 
    n.""IsRead""  as ""isSeen"",
    n.""Message"" as message
FROM 
    ""Notifications"" n 
    JOIN ""AspNetUsers""  u ON u.""Id""  = n.""UserId""  
WHERE 
    u.""Id""  = '{userId}'
ORDER BY 
    ""IsRead""  ASC, 
    n.""CreatedAt"" DESC";

            // Create a list to store the dictionaries
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, queryString);

            var userDictionary = new Dictionary<string, object>();

            userDictionary.Add("content", resultList);
            userDictionary.Add("totalPages", pageCount);
            userDictionary.Add("totalElements", total);
            userDictionary.Add("numberOfElements", resultList.Count);
            userDictionary.Add("currentPageIndex", model.Page);


            return userDictionary;
        }

        public int? UserUnreadNotificationCount(string userId)
        {
            string query = $@"select coalesce((select count(*) as num from ""Notifications"" n  join ""AspNetUsers"" u on u.""Id""  = n.""UserId""  
where  n.""IsRead""  is false and u.""Id""  = '{userId}'), 0)";
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, query);
            var result = resultList.FirstOrDefault();

            return int.Parse(result["coalesce"].ToString());

        }

        
    }
}
