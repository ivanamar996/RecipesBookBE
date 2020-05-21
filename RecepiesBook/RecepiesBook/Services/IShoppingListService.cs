using RecipesBook.Models;
using System.Collections.Generic;

namespace RecipesBook.Services
{
    public interface IShoppingListService
    {
        bool AddIngredientsFromRecipeToSl(int id, int recipeId);
        void CreateNewShoppingList(ShoppingList newShoppingList);
        bool DeleteShoppingList(int id);
        IEnumerable<ShoppingList> GetAllSoppingLists();
        ShoppingList GetShoppingListById(int id);
        bool UpdateShoppingList(int id, ShoppingList changedShoppingList);
    }
}