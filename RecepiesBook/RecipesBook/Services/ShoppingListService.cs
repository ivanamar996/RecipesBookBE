using Microsoft.EntityFrameworkCore;
using RecipesBook.Data;
using RecipesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RecipesBook.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly RecipesBookDbContext _dbContext;

        public ShoppingListService(RecipesBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ShoppingList> GetAllSoppingLists()
        {
            return _dbContext.ShoppingLists
                    .Include(x => x.IngAmounts)
                        .ThenInclude(x => x.Ingredient);
        }

        public ShoppingList GetShoppingListById(int id)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(x => x.IngAmounts)
                    .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.Id == id);

            return shoppingList;

        }

        public void CreateNewShoppingList(ShoppingList newShoppingList)
        {

            foreach (var ing in newShoppingList.IngAmounts)
            {
                var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Id == ing.IngredientId) ?? _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                ing.Ingredient = ingredient;

                _dbContext.IngAmounts.Add(ing);

            }

            _dbContext.ShoppingLists.Add(newShoppingList);

            _dbContext.SaveChanges();
        }

        public bool UpdateShoppingList(int id, ShoppingList changedShoppingList)
        {
            var currentShoppingList = _dbContext.ShoppingLists.Include(x => x.IngAmounts).SingleOrDefault(x => x.Id == id);

            if (currentShoppingList == null)
                return false;

            foreach (var ing in changedShoppingList.IngAmounts)
            {
                if (ing.IngredientId == null)
                    CreateNewIngAmount(ing);
                else
                    UpdateIngAmount(ing);
            }

            var deletedIngAmounts = currentShoppingList.IngAmounts.Where(i => changedShoppingList.IngAmounts.All(x => x.Id != i.Id));
            _dbContext.IngAmounts.RemoveRange(deletedIngAmounts);

            currentShoppingList.Update(changedShoppingList);

            _dbContext.SaveChanges();
            return true;
        }

        public bool AddIngredientsFromRecipeToSl(int id, int recipeId)
        {
            var currentShoppingList = _dbContext.ShoppingLists.SingleOrDefault(x => x.Id == id);

            var recipe = _dbContext.Recipes.Include(x => x.IngAmounts)
                                            .SingleOrDefault(x => x.Id == recipeId);

            if (currentShoppingList == null || recipe == null)
                return false;

            foreach (var ing in recipe.IngAmounts)
            {
                var ingAmount = _dbContext.IngAmounts.SingleOrDefault(x => x.ShoppingListId == id && x.IngredientId == ing.IngredientId);
                if (ingAmount != null)
                    ingAmount.Amount += ing.Amount;
                else
                    _dbContext.IngAmounts.Add(new IngAmount() { Amount = ing.Amount, IngredientId = ing.IngredientId, ShoppingListId = id });
            }

            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteShoppingList(int id)
        {
            var shoppingList = _dbContext.ShoppingLists.FirstOrDefault(x => x.Id == id);

            if (shoppingList == null)
                return false;

            _dbContext.ShoppingLists.Remove(shoppingList);

            var ingAmounts = _dbContext.IngAmounts.Where(x => x.ShoppingListId == id);
            _dbContext.IngAmounts.RemoveRange(ingAmounts);
            _dbContext.SaveChanges();

            return true;
        }

        private void UpdateIngAmount(IngAmount ing)
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

        private void CreateNewIngAmount(IngAmount ing)
        {
            var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Name.Equals(ing.Ingredient.Name));

            if (ingredient != null)
            {
                ing.IngredientId = ingredient.Id;
            }
            else
            {
                ingredient = _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                ing.IngredientId = ingredient.Id;
            }

            _dbContext.IngAmounts.Add(ing);
        }
    }
}
