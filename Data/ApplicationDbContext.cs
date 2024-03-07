using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Models;

namespace AaronColacoAsp.NETProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AaronColacoAsp.NETProject.Models.Item> Item { get; set; } = default!;
    }
}
