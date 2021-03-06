﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RecipesBook.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<IngAmount> IngAmounts { get; set; }

        public void Update(ShoppingList shoppingList)
        {
            Name = shoppingList.Name;
            Description = shoppingList.Description;
        }
    }
}
