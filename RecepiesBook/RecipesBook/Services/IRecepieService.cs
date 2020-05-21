using RecipesBook.Models;
using System.Collections.Generic;

namespace RecipesBook.Services
{
    public interface IRecipeService
    {
        void CreateNewRecipe(Recipe newRecipe);
        bool DeleteRecipe(int id);
        IEnumerable<Recipe> GetAllRecipes();
        Recipe GetRecipeById(int id);
        bool UpdateRecipe(int id, Recipe changedRecipe);
    }
}