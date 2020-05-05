using Microsoft.EntityFrameworkCore;
using RecepiesBook.Data;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            foreach (var ing in newShoppingList.IngAmounts)
            {
                var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Id == ing.IngredientId);

                if(ingredient == null)
                {
                    ingredient =  _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                }

                ing.Ingredient = ingredient;

                _dbContext.IngAmounts.Add(ing);
  
            }

            _dbContext.ShoppingLists.Add(newShoppingList);

            _dbContext.SaveChanges();
        }

        public bool UpdateShoppingList(int id, ShoppingList changedShoppingList)
        {
            var currentShoppingList = _dbContext.ShoppingLists.Where(x => x.Id == id).FirstOrDefault();

            if (currentShoppingList == null)
                return false;

            foreach (var ing in changedShoppingList.IngAmounts)
            {
                if (ing.IngredientId == null)
                {
                    _dbContext.Ingredients.Add(ing.Ingredient);
                    _dbContext.IngAmounts.Add(ing);
                }
                else
                {
                    var ingAmount = _dbContext.IngAmounts.SingleOrDefault(x => x.Id == ing.Id);

                    ingAmount.Amount = ing.Amount;
                    ingAmount.Ingredient = ing.Ingredient;
                }
            }

            currentShoppingList.Name = changedShoppingList.Name;
            currentShoppingList.Description = changedShoppingList.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool AddIngredientsFromRecepieToSl(int id, int recepieId)
        {
            var currentShoppingList = _dbContext.ShoppingLists.SingleOrDefault(x => x.Id == id);

            var recepie = _dbContext.Recepies.Include(x=>x.IngAmounts)
                                            .SingleOrDefault(x => x.Id == recepieId);

            if (currentShoppingList == null || recepie == null)
                return false;

            foreach (var ing in recepie.IngAmounts)
            {
                ing.ShoppingListId = id;
            }

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

                if (ing.RecepieId == null)
                {
                    _dbContext.IngAmounts.Remove(ing);
                }
            }

            _dbContext.SaveChanges();
            return true;
        }
    }
}
