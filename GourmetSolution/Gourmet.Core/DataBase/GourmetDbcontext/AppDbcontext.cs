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
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.DataProtection;

namespace Gourmet.Core.DataBase.GourmetDbcontext
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
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
        public DbSet<Difficulty_Level> DFs { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<InCompleteRecipe> InCompleteRecipes { get; set; }
        public DbSet<Email_Pass> Email_Passwords { get; set; }
        public DbSet<Temp_Password> Temproary_Passwords { get; set; }
        public DbSet<Secret> Secrets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<RecipeIngredients>()
                  .HasKey(m => new { m.RecipeId, m.IngredientId });
            modelbuilder.Entity<RecentRecipeUser>()
                  .HasKey(m => new { m.userId, m.RecipeId });
            modelbuilder.Entity<FavouritRecipeUser>()
                  .HasKey(m => new { m.userId, m.RecipeId });
            modelbuilder.Entity<RecipeStep>()
                  .HasKey(m => new { m.RecipeId, m.Number });
            modelbuilder.Entity<ScoreRecipeUser>()
                  .HasKey(m => new { m.RecipeId, m.userId });
        }
        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
        public DbSet<RecentRecipeUser> RecentRecipeUsers { get; set; }
        public DbSet<FavouritRecipeUser> FavouritRecipeUsers { get; set; }
        public DbSet<ScoreRecipeUser> ScoreRecipeUsers { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
    }
}
