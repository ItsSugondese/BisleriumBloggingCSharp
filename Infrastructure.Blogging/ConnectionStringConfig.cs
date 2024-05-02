using Domain.Blogging.Constant;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging
{
    public class ConnectionStringConfig
    {
        public static NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(ConnectionStringConstant.connection);
        } 

        public static List<Dictionary<string, object>> getValueFromQuery(List<Dictionary<string, object>> resultList, string queryString)
        {
            using (NpgsqlConnection connection = getConnection())
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
                                row[columnName] = DBNull.Value.Equals(columnValue) ? null : columnValue;
                            }

                            // Add the dictionary representing the current row to the list
                            resultList.Add(row);
                        }
                    }
                }
                connection.Close();
            }
            return resultList;
        }
    }
}
