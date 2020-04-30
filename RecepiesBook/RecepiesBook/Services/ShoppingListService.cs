using Microsoft.EntityFrameworkCore;
using RecepiesBook.Data;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Services
{
    public class ShoppingListService
    {
        private readonly RecepiesBookDbContext _dbContext;

        public ShoppingListService(RecepiesBookDbContext dbContext)
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
                                    .Where(sl => sl.Id == id)
                                    .FirstOrDefault();

            return shoppingList;

        }

        public void CreateNewShoppingList(ShoppingList newShoppingList)
        {
            _dbContext.ShoppingLists.Add(newShoppingList);
            _dbContext.SaveChanges();
        }

        public bool UpdateShoppingList(int id, ShoppingList changedRecepie)
        {
            var currentShoppingList = _dbContext.ShoppingLists.Where(x => x.Id == id).FirstOrDefault();

            if (currentShoppingList == null)
                return false;

            currentShoppingList.Name = changedRecepie.Name;
            currentShoppingList.Description = changedRecepie.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteShoppingList(int id)
        {
            var shoppingList = _dbContext.ShoppingLists.Where(x => x.Id == id).FirstOrDefault();

            if (shoppingList == null)
                return false;

            _dbContext.ShoppingLists.Remove(shoppingList);

            var ingAmounts = _dbContext.IngAmounts.Where(x => x.ShoppingListId == id);

            foreach (var ing in ingAmounts)
            {
                ing.ShoppingListId = null;
            }

            _dbContext.SaveChanges();
            return true;
        }
    }
}
