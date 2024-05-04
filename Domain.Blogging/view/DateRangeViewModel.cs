using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view
{
    public class DateRangeViewModel
    {
        public string fromDate { get; set; } = "2024-01-01";
        public string toDate { get; set; } = "2024-01-01";
        public bool isAll { get; set; } = true;
    }
}
