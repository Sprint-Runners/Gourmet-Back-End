using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace Gourmet.Core.DataBase.GourmetDbcontext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Primary_Source_of_Ingredient> PSOIs { get; set; }
        public DbSet<Cooking_Method> CMs { get; set; }
        public DbSet<Food_type> FTs { get; set; }
        public DbSet<Nationality> Ns { get; set; }
        public DbSet<Meal_Type> MTs { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<FoodIngredients>()
                  .HasKey(m => new { m.RecipeId, m.IngredientId });
        }
        public DbSet<FoodIngredients> FoodIngredients { get; set; }
        
    }
}
