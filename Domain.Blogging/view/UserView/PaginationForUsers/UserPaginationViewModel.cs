using Domain.Blogging.Utils.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.UserView.PaginationForUsers
{
    public class UserPaginationViewModel : PaginationRequest
    {
        public string? userType { get; set; }
        public string name { get; set; } = "";
        
    }
}
