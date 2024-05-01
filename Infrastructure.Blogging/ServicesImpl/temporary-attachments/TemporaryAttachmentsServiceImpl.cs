using Application.Blogging.TemporaryAttachement;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.enums;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.temporary_attachments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.temporary_attachments
{
    public class TemporaryAttachmentsServiceImpl : ITemporaryAttachmentsService
    {
        private readonly GenericFileUtils _genericFileUtils;
        private readonly ApplicationDbContext _context;
        public TemporaryAttachmentsServiceImpl(ApplicationDbContext dbContext)
        {

            _context = dbContext;
            _genericFileUtils = new GenericFileUtils();
        }


        public async Task<List<int>> SaveTemporaryAttachment(TemporaryAttachmentsViewModel detailRequestPojo)
        {
            List<int> savedTemporaryAttachmentId = new List<int>();
            int i = 0;
            if (detailRequestPojo.Attachments != null)
            {


             
                foreach (var ticketAttachment in detailRequestPojo.Attachments)
                {
                    Dictionary<string, object> ticketAttachments = _genericFileUtils.SaveTempFile(ticketAttachment,
                        new List<FileType> { FileType.IMAGE, FileType.DOC, FileType.PDF, FileType.EXCEL });
                    string filename = ticketAttachment.FileName;

                    float fileSize = (float)ticketAttachment.Length / (1024 * 1024);
                    await SaveTemporaryFile(savedTemporaryAttachmentId, filename, fileSize, (FileType)ticketAttachments["fileType"],
                        (string)ticketAttachments["location"]);
                 
                }
            }

            return savedTemporaryAttachmentId;
        }

        private async Task SaveTemporaryFile(List<int> savedTemporaryAttachmentId, string fileName, float? fileSize, FileType fileType, string ticketAttachments)
        {
            TemporaryAttachments temporaryAttachments = new TemporaryAttachments
            {
                Location = ticketAttachments,
                Name =  fileName,
                FileSize = fileSize,
                FileType = fileType
            };

            _context.TemporaryAttachments.Add(temporaryAttachments);
            await _context.SaveChangesAsync(); // This saves the entity to the database and updates its ID if auto-generated

            // Now you can access the ID of the saved entity
            int? id = temporaryAttachments.Id;

            savedTemporaryAttachmentId.Add((int)id);
        }


    }
}
