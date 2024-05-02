using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Blogging.Generics.Model;
using Domain.Blogging.enums.BlogEnums;
using System.Text.Json.Serialization;

namespace Domain.Blogging.Entities
{
    public class BlogReactMapping: GenericModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Blog? Blog { get; set; }
        public string Reaction { get; set; }
        public AppUser? User { get; set; }
    }
}
