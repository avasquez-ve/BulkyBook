using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace BulkyBook.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
