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
            _dbContext.Recepies.Add(newRecepie);
            _dbContext.SaveChanges();
        }

        public bool UpdateRecepie(int id, Recepie changedRecepie)
        {
            var currentRecepie = _dbContext.Recepies.Where(x => x.Id == id).FirstOrDefault();

            if (currentRecepie == null)
                return false;

            currentRecepie.Name = changedRecepie.Name;
            currentRecepie.ImagePath = changedRecepie.ImagePath;
            currentRecepie.Description = changedRecepie.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteRecepie(int id)
        {
            var recepie = _dbContext.Recepies.Where(x => x.Id == id).FirstOrDefault();

            if (recepie == null)
                return false;

            _dbContext.Recepies.Remove(recepie);

            var ingAmounts = _dbContext.IngAmounts.Where(x => x.RecepieId == id);

            foreach(var ing in ingAmounts)
            {
                ing.RecepieId = null;
            }

            _dbContext.SaveChanges();
            return true;
        }
    }
}
