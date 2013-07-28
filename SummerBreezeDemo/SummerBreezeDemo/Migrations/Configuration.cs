namespace SummerBreezeDemo.Migrations
{
    using SummerBreezeDemo.Datalayer;
    using SummerBreezeDemo.Models.DBObjects;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SummerBreezeDemo.Datalayer.SummerBreezeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SummerBreezeDbContext>());
          
        }

        protected override void Seed(SummerBreezeDemo.Datalayer.SummerBreezeDbContext context)
        {

            context.Products.AddOrUpdate(
                p => p.ProductId,
                new Product() { ProductId = Guid.NewGuid(), Name = "Surface", RegisteredDate = DateTime.Now, Tags = "microsoft;surface;tablet;pro;windows8", Price = 899.99, PicUrl = "http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL_Surface_WinRT_64GB_7ZR-00002/en-INTL_L_Surface_WinRT_64GB_7ZR-00002_mnco.jpg" },
                new Product() { ProductId = Guid.NewGuid(), Name = "Samsung Galaxy S3", RegisteredDate = DateTime.Now.AddDays(-30), Tags = "samsung;galaxy;s3;smartphone", Price = 434.99, PicUrl = "http://cdn3.pcadvisor.co.uk/cmsdata/reviews/3421346/GALAXY_SIII_2.jpg" },
                new Product() { ProductId = Guid.NewGuid(), Name = "Nokia Lumia 620", RegisteredDate = DateTime.Now.AddDays(-60), Tags = "nokia;lumia;620;tablet;smartphone", Price = 349.99, PicUrl = "http://cdn0.sbnation.com/entry_photo_images/7301357/lumia620final_large_verge_medium_landscape.jpg" },
                new Product() { ProductId = Guid.NewGuid(), Name = "Google Nexus 7", RegisteredDate = DateTime.Now.AddDays(-10), Tags = "nexus;7;tablet;android", Price = 443.80, PicUrl = "http://www.notebookcheck.info/uploads/tx_nbc2/googNEXUS7.png" },
                new Product() { ProductId = Guid.NewGuid(), Name = "iPad Mini", RegisteredDate = DateTime.Now.AddDays(-10), Tags = "nexus;7;tablet;android", Price = 443.80, PicUrl = "http://www.digitaltrends.com/wp-content/uploads/2012/11/ipad-mini-press.jpg" }
                );

            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
