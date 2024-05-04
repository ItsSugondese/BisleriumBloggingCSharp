using Application.Blogging.DashboardApp;
using Domain.Blogging.view;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.DashbaordImpl
{
    public class DashboardServiceImpl : IDashboardService
    {
        public Dictionary<string, object> GetSumData(DateRangeViewModel model)
        {
            string query = $@"select 
coalesce (sum(case when brm.""Reaction""= 'UPVOTE' then 1 else 0 end), 0) upvote,
coalesce (sum(case when brm.""Reaction""= 'DOWNVOTE' then 1 else 0 end), 0) downvote,
coalesce ((select count(*) from ""Comments"" c where case when {model.isAll} is true then true else cast(c.""CreatedAt"" as date) between cast('{model.fromDate}' as date) and cast('{model.toDate}' as date) end), 0) comments,
coalesce((select count(*) from ""Blog"" b where case  when {model.isAll} is true then true else cast(b.""CreatedAt"" as date) between cast('{model.fromDate}' as date) and cast('{model.toDate}' as date) end), 0) blog
from ""BlogReactMappings"" brm where case when {model.isAll} is true then true else cast(brm.""CreatedAt"" as date) between cast('{model.fromDate}' as date) and cast('{model.toDate}' as date) end";


            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            // Create a connection to PostgreSQL using Npgsql
            ConnectionStringConfig.getValueFromQuery(resultList, query);
            var result = resultList.FirstOrDefault();


            return result;
        }
    }
}
