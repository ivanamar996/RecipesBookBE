using Microsoft.EntityFrameworkCore;
using RecepiesBook.Data;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Services
{
    public class IngredientService
    {
        private readonly RecepiesBookDbContext _dbContext;

        public IngredientService(RecepiesBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Ingredient> GetAllIngredients()
        {
            return _dbContext.Ingredients;
        }

        public Ingredient GetIngredientById(int id)
        {
            return _dbContext.Ingredients.Where(ing => ing.Id == id).FirstOrDefault(); 
        }

        public void CreateNewIngredient(Ingredient newIngredient)
        {
            _dbContext.Ingredients.Add(newIngredient);
            _dbContext.SaveChanges();
        }

        public bool UpdateIngredient(int id, Ingredient changedIngredient)
        {
            var currentIngredient = _dbContext.Ingredients.Where(x => x.Id == id).FirstOrDefault();

            if (currentIngredient == null)
                return false;

            currentIngredient.Name = changedIngredient.Name;


            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteIngredient(int id)
        {
            var ingredient = _dbContext.Ingredients.Where(x => x.Id == id).FirstOrDefault();

            if (ingredient == null)
                return false;

            _dbContext.Ingredients.Remove(ingredient);
            
            _dbContext.SaveChanges();
            return true;
        }
    }
}
