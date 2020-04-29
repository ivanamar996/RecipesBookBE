﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Models
{
    public class Recepie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }
        public ICollection<IngAmount> IngAmounts { get; set; }
    }
}
