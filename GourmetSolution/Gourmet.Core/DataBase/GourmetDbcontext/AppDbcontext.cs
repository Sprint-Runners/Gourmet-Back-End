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
            modelbuilder.Entity<RecipeIngredients>()
                  .HasKey(m => new { m.RecipeId, m.IngredientId });
            modelbuilder.Entity<RecentFoodUser>()
                  .HasKey(m => new { m.userId, m.FoodId });
            modelbuilder.Entity<FavouritRecipeUser>()
                  .HasKey(m => new { m.userId, m.RecipeId });
        }
        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
        public DbSet<RecentFoodUser> RecentFoodUsers { get; set; }
        public DbSet<FavouritRecipeUser> FavouritRecipeUsers { get; set; }
    }
}
