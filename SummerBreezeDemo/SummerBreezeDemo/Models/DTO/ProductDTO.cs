﻿using SummerBreeze;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SummerBreezeDemo.Models.DTO
{
    [BreezeLocalizable(false)]
    [BreezeAutoGeneratedKeyType(SummerBreezeEnums.AutoGeneratedKeyType.Identity)] //This tells breeze that The "dummy" primary key SearchResultDTOId is an identity column and will be set by the server
    public class ProductDTO
    {
        [Key] // Your DTO must ALWAYS have primary keys (real or dummy ones), breeze needs to know how primary keys are generated for each entity type
        public Guid ProductDTOId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string PicUrl { get; set; }
        public string SearchDate { get; set; }
    }
 
}