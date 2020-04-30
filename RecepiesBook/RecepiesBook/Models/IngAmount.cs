using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Models
{
    public class IngAmount
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Amount must be number.")]
        public int Amount { get; set; }
        [Required]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int? RecepieId { get; set; }
        public int? ShoppingListId { get; set; }

    }
}
