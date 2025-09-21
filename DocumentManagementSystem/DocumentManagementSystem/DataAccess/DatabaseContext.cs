using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.DataAccess
{
    public class DatabaseContext(DbContextOptions<DocumentContext> options) : DbContext(options)
    {
        public DbSet<DocumentData> Documents { get; set; }
    }

    public class DocumentContext :DbContext
    {
        public DbSet<DocumentData> Documents { get; set; }
    }
}
