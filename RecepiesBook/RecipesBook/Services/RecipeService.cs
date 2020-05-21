using Microsoft.EntityFrameworkCore;
using RecipesBook.Data;
using RecipesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipesBook.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipesBookDbContext _dbContext;

        public RecipeService(RecipesBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _dbContext.Recipes
                    .Include(x => x.IngAmounts)
                        .ThenInclude(x => x.Ingredient);
        }

        public Recipe GetRecipeById(int id)
        {
            var recipe = _dbContext.Recipes
                .Include(x => x.IngAmounts)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(recipe => recipe.Id == id);

            return recipe;
        }

        public void CreateNewRecipe(Recipe newRecipe)
        {

            foreach (var ing in newRecipe.IngAmounts)
            {
                var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Name.Equals(ing.Ingredient.Name));

                if (ingredient == null)
                {
                    ingredient = _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                }

                ing.Ingredient = ingredient;

                _dbContext.IngAmounts.Add(ing);

            }

            _dbContext.Recipes.Add(newRecipe);
            _dbContext.SaveChanges();
        }

        public bool UpdateRecipe(int id, Recipe changedRecipe)
        {
            var currentRecipe = _dbContext.Recipes.Include(x => x.IngAmounts).SingleOrDefault(x => x.Id == id);

            if (currentRecipe == null)
                return false;

            foreach (var ing in changedRecipe.IngAmounts)
            {
                if (ing.IngredientId == null)
                {
                    var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Name.Equals(ing.Ingredient.Name));

                    if (ingredient != null)
                    {
                        ing.Ingredient = ingredient;

                    }
                    else
                    {
                        ingredient = _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                        ing.IngredientId = ingredient.Id;
                    }
                    _dbContext.IngAmounts.Add(ing);

                }
                else
                {
                    var ingAmount = _dbContext.IngAmounts.SingleOrDefault(x => x.Id == ing.Id);

                    if (ingAmount != null)
                    {
                        ingAmount.Amount = ing.Amount;
                        ingAmount.IngredientId = ing.IngredientId;
                    }
                    else
                    {
                        _dbContext.IngAmounts.Add(ing);
                    }
                }
            }

            var deletedIngAmounts = currentRecipe.IngAmounts.Where(i => !changedRecipe.IngAmounts.Any(x => x.Id == i.Id));
            _dbContext.IngAmounts.RemoveRange(deletedIngAmounts);


            currentRecipe.Name = changedRecipe.Name;
            currentRecipe.ImagePath = changedRecipe.ImagePath;
            currentRecipe.Description = changedRecipe.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteRecipe(int id)
        {
            var recipe = _dbContext.Recipes.SingleOrDefault(x => x.Id == id);

            if (recipe == null)
                return false;

            _dbContext.Recipes.Remove(recipe);

            var ingAmounts = _dbContext.IngAmounts.Where(x => x.RecipeId == id);
            _dbContext.IngAmounts.RemoveRange(ingAmounts);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
