using Domain.Blogging.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.DashboardApp
{
    public interface IDashboardService
    {
        Dictionary<string, object> GetSumData(DateRangeViewModel model);
    }
}
