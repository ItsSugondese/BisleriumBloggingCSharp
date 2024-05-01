using Domain.Blogging.view.temporary_attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.TemporaryAttachement
{
    public interface ITemporaryAttachmentsService
    {
        Task<List<int>> SaveTemporaryAttachment(TemporaryAttachmentsViewModel requestPojo);
    }
}
