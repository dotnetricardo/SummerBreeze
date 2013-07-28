using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SummerBreezeDemo.Models.DBObjects
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string PicUrl { get; set; }
        public double Price { get; set; }
        public DateTime RegisteredDate { get; set; }

    }
}