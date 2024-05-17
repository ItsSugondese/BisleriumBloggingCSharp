using Application.Blogging.BlogApp;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Entities.temporary_attachments;
using Domain.Blogging.Utils.GenericFile;
using Domain.Blogging.view.BLogView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.BloggingServiceImpl
{
    public class CommentHistoryServiceImpl : ICommentHistoryService
    {
        private readonly ApplicationDbContext _context;
        public CommentHistoryServiceImpl(ApplicationDbContext context) {
            _context = context;
        }

        // to save comment history. comment Details and comment is send in paraemeter after saving 
        // comment process is done
        public async Task SaveHistory(CommentViewModel model, Comments comment)
        {
            CommentHistory history = new CommentHistory
            {
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                Content = model.Content,
            };

            _context.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
