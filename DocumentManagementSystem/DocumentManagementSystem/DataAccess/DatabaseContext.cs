using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.DataAccess
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<DocumentData> Documents { get; set; }
    }

    /*public class DocumentContext :DbContext
    {
        public virtual DbSet<DocumentData> Documents { get; set; }
    }*/
}
