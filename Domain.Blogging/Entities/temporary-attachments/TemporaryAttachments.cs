using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Blogging.enums;

namespace Domain.Blogging.Entities.temporary_attachments
{
    public class TemporaryAttachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? FileSize { get; set; }

        public FileType FileType { get; set; }


    }
}
