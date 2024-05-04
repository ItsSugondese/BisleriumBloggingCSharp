using Application.Blogging.RepoInterface.UserRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.view.UserView;
using Domain.Blogging.view.UserView.PaginationForUsers;
using Infrastructure.Blogging.utils;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.Repo.RepoUser
{
    public class UserRepoImpl: IUserRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepoImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteProfilePicByUserId(string id)
        {
            AppUser user = await FindById(id);
            if (user.ProfilePath != null)
            {


                try
                {
                    // Check if the file exists before attempting to delete it
                    if (File.Exists(user.ProfilePath))
                    {
                        File.Delete(user.ProfilePath);
                        Console.WriteLine("Image deleted successfully.");
                    }
                    else
                    {
                        throw new Exception("Somethign wrong happend when deleing image");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred: " + ex.Message);
                }
            }

            user.ProfilePath = null;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AppUser> FindById(string id)
        {
            AppUser? user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == id);

            if (user == null)
            {
                throw new Exception(MessageConstantMerge.notExist("id", ModuleNameConstant.USER));
            }

            return user;
        }

        public async Task<Dictionary<string, object>> GetAllUsersBasicDetailsPaginated(UserPaginationViewModel model)
        {
            var total = _dbContext.Users.Count();
            var pageCount = Math.Ceiling(total / (float)model.Row);
            string queryString = $@"SELECT anu.""Id"" AS id,
       anu.""UserName"" AS ""fullName"",
       anu.""ProfilePath"" AS ""profilePath"",
       anu.""Email"" AS email,
       (SELECT anr.""Name""
        FROM ""AspNetUserRoles"" anur
        JOIN ""AspNetRoles"" anr ON anr.""Id"" = anur.""RoleId""
        WHERE anur.""UserId"" = anu.""Id"") AS ""userType""
FROM ""AspNetUsers"" anu
WHERE case when '{model.name}' = '' then true else anu.""UserName"" ilike concat('%', '{model.name}', '%') end and
case when '{model.userType}' = 'ALL' then true else  upper((
          SELECT anr.""Name""
          FROM ""AspNetUserRoles"" anur
          JOIN ""AspNetRoles"" anr ON anr.""Id"" = anur.""RoleId""
          WHERE anur.""UserId"" = anu.""Id""
      )) = upper('{model.userType}') end
LIMIT {model.Row} OFFSET {(model.Page - 1) * model.Row}";

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

        public async Task<Dictionary<string, object>> GetUserProfileFromToken(string id)
        {
            AppUser user = await FindById(id);
            var userDictionary = new Dictionary<string, object>();

            // If the user exists, populate the dictionary with user properties
            if (user != null)
            {
                userDictionary.Add("id", user.Id);
                userDictionary.Add("fullName", user.UserName);
                userDictionary.Add("email", user.Email);
                userDictionary.Add("profilePath", user.ProfilePath);
                // Add other properties as needed
            }
            return userDictionary;
        }

        public async Task<object> testing()
        {

            string queryString = @"
    SELECT b.""Id"" AS id, 
           b.""Title"" AS title, 
           b.""Content"" AS content,
           TO_CHAR(b.""CreatedAt"", 'YYYY-MM-DD HH:MI AM') AS ""postedOn"", 
           anu.""UserName"" AS username 
    FROM ""Blog"" b 
    JOIN ""AspNetUsers"" anu ON b.""UserId"" = anu.""Id""";

// Create a list to store the dictionaries
List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

// Create a connection to PostgreSQL using Npgsql
using (NpgsqlConnection connection = ConnectionStringConfig.getConnection())
{
    // Create a command with your SQL query and the NpgsqlConnection
    using (NpgsqlCommand command = new NpgsqlCommand(queryString, connection))
    {
        // Open the connection
        connection.Open();
        
        // Execute the query and get a reader
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            // Loop through the results
            while (reader.Read())
            {
                // Create a dictionary to store the values of the current row
                Dictionary<string, object> row = new Dictionary<string, object>();

                // Populate the dictionary with column names and their respective values
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);
                    row[columnName] = columnValue;
                }

                // Add the dictionary representing the current row to the list
                resultList.Add(row);
            }
        }
    }
}

            return resultList;

        }
    }
}
