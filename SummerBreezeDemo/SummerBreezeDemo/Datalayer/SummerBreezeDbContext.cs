using SummerBreezeDemo.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SummerBreezeDemo.Datalayer
{
    public class SummerBreezeDbContext : DbContext 
    {
        public DbSet<Product> Products { get; set; }
        public SummerBreezeDbContext()
            : base("DefaultConnection")
        {
                
        }

        
    }
}