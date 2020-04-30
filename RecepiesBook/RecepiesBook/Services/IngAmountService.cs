using Microsoft.EntityFrameworkCore;
using RecepiesBook.Data;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Services
{
    public class IngAmountService
    {
        private readonly RecepiesBookDbContext _dbContext;

        public IngAmountService(RecepiesBookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<IngAmount> GetAllIngAmounts()
        {
            return _dbContext.IngAmounts.Include(x => x.Ingredient);
        }

        public IngAmount GetIngAmountsById(int id)
        {
            return _dbContext.IngAmounts.Where(x => x.Id == id).Include(x=>x.Ingredient).FirstOrDefault();
        }

        public void CreateNewIngAmount(IngAmount newIngAmount)
        {
            _dbContext.IngAmounts.Add(newIngAmount);
            _dbContext.SaveChanges();
        }

        public bool UpdateIngAmount(int id, IngAmount changedIngAmount)
        {
            var currentIngAmount = _dbContext.IngAmounts.Where(x => x.Id == id).Include(x=>x.Ingredient).FirstOrDefault();

            if (currentIngAmount == null)
                return false;

            currentIngAmount.Amount = changedIngAmount.Amount;
            currentIngAmount.IngredientId = changedIngAmount.IngredientId;
            currentIngAmount.RecepieId = changedIngAmount.RecepieId;
            currentIngAmount.ShoppingListId = changedIngAmount.ShoppingListId;


            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteIngAmount(int id)
        {
            var IngAmount = _dbContext.IngAmounts.Where(x => x.Id == id).FirstOrDefault();

            if (IngAmount == null)
                return false;

            _dbContext.IngAmounts.Remove(IngAmount);

            _dbContext.SaveChanges();

            return true;
        }
    }
}
