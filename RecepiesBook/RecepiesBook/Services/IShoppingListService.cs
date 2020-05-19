using RecepiesBook.Models;
using System.Collections.Generic;

namespace RecepiesBook.Services
{
    public interface IShoppingListService
    {
        bool AddIngredientsFromRecepieToSl(int id, int recepieId);
        void CreateNewShoppingList(ShoppingList newShoppingList);
        bool DeleteShoppingList(int id);
        IEnumerable<ShoppingList> GetAllSoppingLists();
        ShoppingList GetShoppingListById(int id);
        bool UpdateShoppingList(int id, ShoppingList changedShoppingList);
    }
}