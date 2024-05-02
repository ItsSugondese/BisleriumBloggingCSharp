using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.BLogView
{
    public class CommentViewModel
    {
        public int? Id { get; set; }
        [Required]
        public int BlogId { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}
