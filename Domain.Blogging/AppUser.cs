using Domain.Blogging.Generics.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging
{
    public class AppUser:UserGenericModel
    {
        //public string? UserName { get; set; }
        //public string? EmailAddress {  get; set; } 
        public string? Password { get; set; }
        public string? ProfilePath { get; set; }


    }
}
