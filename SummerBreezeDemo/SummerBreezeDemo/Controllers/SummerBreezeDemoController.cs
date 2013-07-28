using Breeze.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummerBreeze;
using SummerBreezeDemo.Datalayer;
using SummerBreezeDemo.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SummerBreezeDemo.Controllers
{
    [BreezeJsonFormatter, ODataActionFilter]
    public class SummerBreezeDemoController : ApiController
    {
        //Our 2 ContextProviders: The EFContextProvider for direct DB access and SummerBreezeContextProvider for DTO´s
        private readonly EFContextProvider<SummerBreezeDbContext> _EFContextProvider = new EFContextProvider<SummerBreezeDbContext>();
        private readonly SummerBreezeContextProvider<DTOContextProvider> _summerBreezeContextProvider = new SummerBreezeContextProvider<DTOContextProvider>();
        
        
        //[AcceptVerbs("GET")]
        //public IQueryable<ProductDTO> Search([FromUri] string[] hashtags)
        //{
            
        //    var products = _EFContextProvider.Context.Products.ToList().Where(p => hashtags.Any(t => p.Tags.Contains(t.ToString().Substring(1))));
            
        //    if(products.Count() > 0)
        //    {
        //        //this one uses SummerBreezeContextProvider
        //        return _summerBreezeContextProvider.Context.GetSearchResultsDTO(products.ToList()).AsQueryable();
        //    }
            
        //    return null;
        //}


        [AcceptVerbs("GET")]
        public IQueryable<ProductDTO> ProductsDTO()
        {
            //Getting all products as DTO´s 
            var products = _EFContextProvider.Context.Products;
            
            return _summerBreezeContextProvider.Context.GetSearchResultsDTO(products.ToList()).AsQueryable();
          
        }

        [AcceptVerbs("Get")]
        public string Metadata() // this is the EF metadata (is called by breeze)
        {
            return _EFContextProvider.Metadata();        
        }

        [AcceptVerbs("Get")]
        public string SummerBreezeMetadata()
        {
            return _summerBreezeContextProvider.Metadata(); //this is the DTO´s metadata (is called by summerbreeze)
        }

        [AcceptVerbs("POST")]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            //check if object type exists with GetTrackedEntities 
            var counter = _summerBreezeContextProvider.GetTrackedEntities().Where(e => saveBundle["entities"].Any(t => t["entityAspect"]["entityTypeName"].ToString().Contains(e))).Count();

            if (counter > 0)
            {
                //SummerBreezeContextProvider
                return _summerBreezeContextProvider.SaveChanges(saveBundle);
            }

            //EFContextProvider
            return _EFContextProvider.SaveChanges(saveBundle);
        }

    }
}
