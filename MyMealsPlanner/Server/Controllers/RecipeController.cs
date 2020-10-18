using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyMealsPlanner.Server.Data;
using MyMealsPlanner.Shared.Models;

namespace MyMealsPlanner.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeContext _context;

        private readonly ILogger<RecipeController> _logger;

        public RecipeController(ILogger<RecipeController> logger, RecipeContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return _context.Recipes
                 .OrderBy(recipe => recipe.Name)
                 .AsNoTracking()
                 .ToList();
        }

        /// <summary>
        /// Gets a specific recipe by Id.
        /// </summary>
        [HttpGet("{id}", Name = "GetRecipe")]
        public ActionResult<Recipe> GetById(int id)
        {
            var item = _context.Recipes.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            item = _context.Recipes
                .Include(recipe => recipe.Ingredients)
                .Single(p => p.Id == id);
            return item;
        }

        /// <summary>
        /// Creates a recipe.
        /// </summary>
        [HttpPost]
        //  [Authorize]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.CreateTime = DateTime.Now;
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return CreatedAtRoute("GetRecipe", new { id = recipe.Id }, recipe);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates a recipe with a specific Id.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                var existingRecipe = _context.Recipes
                    .Include(r => r.Ingredients)
                    .Single(p => p.Id == id);
                if (existingRecipe == null)
                {
                    return NotFound();
                }

                // Update Existing Recipe
                _context.Entry(existingRecipe).CurrentValues.SetValues(recipe);

                // Delete Ingredients
                foreach (var existingIngredient in existingRecipe.Ingredients.ToList())
                {
                    if (!recipe.Ingredients.Any(c => c.Id == existingIngredient.Id))
                        _context.Ingredients.Remove(existingIngredient);
                }

                // Update and Insert Contacts
                foreach (var ingredient in recipe.Ingredients)
                {
                    var existingIngredient = existingRecipe.Ingredients
                        .Where(c => c.Id == ingredient.Id)
                        .SingleOrDefault();
                    if (existingIngredient != null)
                        _context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
                    else
                    {
                        var newIngredient = new Ingredient
                        {
                            Id = ingredient.Id,
                            Name = ingredient.Name,
                            Quantity = ingredient.Quantity
                        };
                        existingRecipe.Ingredients.Add(newIngredient);
                    }
                }

                _context.SaveChanges();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes a specific patient by Id.
        /// </summary>
        [HttpDelete("{id}")]
        //  [Authorize]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var patient = _context.Recipes.Find(id);
                if (patient == null)
                {
                    return NotFound();
                }

                _context.Recipes.Remove(patient);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
