﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesBook.Models
{
    public class IngAmount
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Amount must be number.")]
        public int Amount { get; set; }
        public int? IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int? RecipeId { get; set; }
        public int? ShoppingListId { get; set; }

    }
}
