using Domain.Blogging;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.Entities.temporary_attachments;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionStringConstant.connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<BlogReactMapping>()
                .HasOne(e => e.Blog)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogReactMapping> BlogReactMappings { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<CommentReactMapping> CommentReactMappings { get; set; }
        public DbSet<TemporaryAttachments> TemporaryAttachments { get; set; }
    }
}
