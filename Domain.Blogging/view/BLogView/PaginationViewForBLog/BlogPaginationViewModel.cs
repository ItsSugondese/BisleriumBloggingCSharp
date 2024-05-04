using Domain.Blogging.Utils.Paginations;
using Domain.Blogging.view.BLogView.ViewEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.BLogView.PaginationViewForBLog
{
    public class BlogPaginationViewModel : PaginationRequest
    {
        [Required]
        public BlogSortEnums sort { get; set; }
        public string name { get; set; }
        public string fromDate { get; set; } = "2024-01-01";    
        public string toDate { get; set; } = "2024-01-01";

        public bool ofUser { get; set; } = false;
        public bool isAll { get; set; } = false;
    }
}
