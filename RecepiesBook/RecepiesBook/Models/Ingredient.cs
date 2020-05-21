using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesBook.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<IngAmount> IngAmounts { get; set; }
    }
}
