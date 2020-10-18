using System;
using System.Collections.Generic;

namespace MyMealsPlanner.Shared.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public IList<Ingredient> Ingredients { get; set; }
        public IList<Tag> Tags { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CreateTime { get; set; }


        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}
