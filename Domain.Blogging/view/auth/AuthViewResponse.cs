using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.auth
{
    public class AuthViewResponse
    {
        public String jwtToken { get; set; }
        public String username { get; set; }
        public String roles { get; set; }
        public String userId { get; set; }
    }
}
