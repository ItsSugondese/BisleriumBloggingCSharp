using Domain.Blogging;
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
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BisleriumBlogging;Username=postgres;Password=admin");
        }

        public DbSet<Blog> Blog { get; set; }
        public DbSet<TemporaryAttachments> TemporaryAttachments { get; set; }
    }
}
