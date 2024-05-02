using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.Utils.Paginations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page cannot be less than 1")]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Request Per Page cannot be less than 1")]
        public int Row { get; set; } = 10;

        public int? Offset => (Page - 1) * Row;

        public int? Limit => Row;

    }

}
