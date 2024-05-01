using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.BLogView
{
    public class BlogViewModel
    {
        public int? FileId { get; set; }
        public int? Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

    }
}
