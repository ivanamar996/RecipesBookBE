using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesBook.Data
{
    public class RecipesBookDbContext : IdentityDbContext<ApplicationUser>
    {
        public RecipesBookDbContext(DbContextOptions<RecipesBookDbContext> options) : base(options)
        {     
        }

        public DbSet<Recipe> Recipes { get; set; }
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
