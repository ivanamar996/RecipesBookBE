using Microsoft.EntityFrameworkCore;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Data
{
    public class RecepiesBookDbContext : DbContext
    {
        public RecepiesBookDbContext(DbContextOptions options) : base(options)
        {     
        }

        public DbSet<Recepie> Recepies { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngAmount> IngAmounts { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasMany(i => i.IngAmounts)
                .WithOne(x=>x.Ingredient)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
