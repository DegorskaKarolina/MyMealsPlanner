using Microsoft.EntityFrameworkCore;
using MyMealsPlanner.Shared.Models;

namespace MyMealsPlanner.Server.Data
{
    public class RecipeContext : DbContext
    {
        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
