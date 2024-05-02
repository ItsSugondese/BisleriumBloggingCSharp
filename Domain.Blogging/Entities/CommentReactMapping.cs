using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Blogging.Generics.Model;

namespace Domain.Blogging.Entities
{
    public class CommentReactMapping: GenericModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Comments? Comment { get; set; }
        public string Reaction { get; set; }
        public AppUser? User { get; set; }
    }
}
