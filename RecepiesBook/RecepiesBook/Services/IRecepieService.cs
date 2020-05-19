using RecepiesBook.Models;
using System.Collections.Generic;

namespace RecepiesBook.Services
{
    public interface IRecepieService
    {
        void CreateNewRecepie(Recepie newRecepie);
        bool DeleteRecepie(int id);
        IEnumerable<Recepie> GetAllRecepies();
        Recepie GetRecepieById(int id);
        bool UpdateRecepie(int id, Recepie changedRecepie);
    }
}