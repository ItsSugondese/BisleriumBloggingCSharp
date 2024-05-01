using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.temporary_attachments
{
    public class TemporaryAttachmentsViewModel
    {
        public int? Id { get; set; }
        public List<IFormFile> Attachments { get; set; }

        public List<string>? FilePaths { get; set; }

        public List<string>? Name { get; set; }

    }
    
}
