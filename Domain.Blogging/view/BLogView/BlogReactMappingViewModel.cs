using Domain.Blogging.enums.BlogEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.BLogView
{
    public class BlogReactMappingViewModel
    {
        [Required]
        public int BlogId { get; set; }

        [Required]
        public ReactionType Reaction { get; set; }
    }
}
