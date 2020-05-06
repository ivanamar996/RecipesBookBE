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
    public class RecepieService
    {
        private readonly RecepiesBookDbContext _dbContext;

        public RecepieService(RecepiesBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Recepie> GetAllRecepies()
        {
            return _dbContext.Recepies
                    .Include(x=>x.IngAmounts)
                        .ThenInclude(x=>x.Ingredient);
        }

        public Recepie GetRecepieById(int id)
        {
            var recepie = _dbContext.Recepies
                                    .Include(x => x.IngAmounts)
                                    .ThenInclude(x => x.Ingredient)
                                    .Where(recepie => recepie.Id == id)
                                    .FirstOrDefault();

            return recepie;
        }

        public void CreateNewRecepie(Recepie newRecepie)
        {

            foreach (var ing in newRecepie.IngAmounts)
            {
                var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Id == ing.IngredientId);

                if (ingredient == null)
                {
                    ingredient = _dbContext.Ingredients.Add(ing.Ingredient).Entity;
                }

                ing.Ingredient = ingredient;

                _dbContext.IngAmounts.Add(ing);

            }

            _dbContext.Recepies.Add(newRecepie);
            _dbContext.SaveChanges();
        }

        public bool UpdateRecepie(int id, Recepie changedRecepie)
        {
            var currentRecepie = _dbContext.Recepies.Include(x=>x.IngAmounts).SingleOrDefault(x => x.Id == id);

            if (currentRecepie == null)
                return false;


            var deletedIngAmounts = currentRecepie.IngAmounts.Except(changedRecepie.IngAmounts);
            _dbContext.IngAmounts.RemoveRange(deletedIngAmounts);


            foreach (var ing in changedRecepie.IngAmounts)
            {
                if (ing.IngredientId == null)
                {
                    _dbContext.Ingredients.Add(ing.Ingredient);
                    _dbContext.IngAmounts.Add(ing);
                }
                else 
                {
                    var ingAmount = _dbContext.IngAmounts.SingleOrDefault(x=>x.Id == ing.Id);

                    if (ingAmount != null)
                    {
                        ingAmount.Amount = ing.Amount;
                        ingAmount.Ingredient = ing.Ingredient;
                    }
                    else
                    {
                        _dbContext.IngAmounts.Add(ing);
                    }
                }
            }


            currentRecepie.Name = changedRecepie.Name;
            currentRecepie.ImagePath = changedRecepie.ImagePath;
            currentRecepie.Description = changedRecepie.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteRecepie(int id)
        {
            var recepie = _dbContext.Recepies.SingleOrDefault(x => x.Id == id);

            if (recepie == null)
                return false;

            _dbContext.Recepies.Remove(recepie);

            var ingAmounts = _dbContext.IngAmounts.Where(x => x.RecepieId == id);

            foreach(var ing in ingAmounts)
            {
                ing.RecepieId = null;

                if(ing.ShoppingListId == null)
                {
                    _dbContext.IngAmounts.Remove(ing);
                }
            }

            _dbContext.SaveChanges();

            return true;
        }
    }
}
