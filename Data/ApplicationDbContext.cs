using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Models;
using Microsoft.AspNetCore.Identity;

namespace AaronColacoAsp.NETProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AaronColacoAsp.NETProject.Models.Item> Item { get; set; } = default!;
        public DbSet<AaronColacoAsp.NETProject.Models.Category> Category { get; set; } = default!;

        public DbSet<AaronColacoAsp.NETProject.Models.Order> Order { get; set; } = default!;

        public DbSet<AaronColacoAsp.NETProject.Models.OrderItem> OrderItem { get; set; } = default!;

        public DbSet<AaronColacoAsp.NETProject.Models.Gift> Gift { get; set; } = default!;

        public DbSet<AaronColacoAsp.NETProject.Models.GiftRecipient> GiftRecipient { get; set; } = default!;

        public DbSet<AaronColacoAsp.NETProject.Models.Status> Status { get; set; } = default!;
      
       
        public DbSet<AaronColacoAsp.NETProject.Models.Customer> Customer { get; set; } = default!;







    }
}
