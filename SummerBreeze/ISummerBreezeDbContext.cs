using Breeze.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SummerBreeze
{
    public interface ISummerBreezeDbContext
    {
   
        void SaveChanges(SaveWorkState saveWorkState);
        Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap);
        bool BeforeSaveEntity(EntityInfo entityInfo);
        List<string> GetTrackedEntities();
    }
}
