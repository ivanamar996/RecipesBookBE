using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Data
{
    public class RecepiesBookDbContext : IdentityDbContext<ApplicationUser>
    {
        public RecepiesBookDbContext(DbContextOptions<RecepiesBookDbContext> options) : base(options)
        {     
        }

        public DbSet<Recepie> Recepies { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngAmount> IngAmounts { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingredient>()
                .HasMany(i => i.IngAmounts)
                .WithOne(x=>x.Ingredient)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
