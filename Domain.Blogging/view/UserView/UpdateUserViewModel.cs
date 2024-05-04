using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.UserView
{
    public class UpdateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        public int? FileId { get; set; }


    }
}
