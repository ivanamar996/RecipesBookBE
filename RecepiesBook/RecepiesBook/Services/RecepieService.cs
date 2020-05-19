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
    public class RecepieService : IRecepieService
    {
        private readonly RecepiesBookDbContext _dbContext;

        public RecepieService(RecepiesBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Recepie> GetAllRecepies()
        {
            return _dbContext.Recepies
                    .Include(x => x.IngAmounts)
                        .ThenInclude(x => x.Ingredient);
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
                var ingredient = _dbContext.Ingredients.SingleOrDefault(x => x.Name.Equals(ing.Ingredient.Name));

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
            var currentRecepie = _dbContext.Recepies.Include(x => x.IngAmounts).SingleOrDefault(x => x.Id == id);

            if (currentRecepie == null)
                return false;

            foreach (var ing in changedRecepie.IngAmounts)
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

            var deletedIngAmounts = currentRecepie.IngAmounts.Where(i => !changedRecepie.IngAmounts.Any(x => x.Id == i.Id));
            _dbContext.IngAmounts.RemoveRange(deletedIngAmounts);


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
            _dbContext.IngAmounts.RemoveRange(ingAmounts);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
