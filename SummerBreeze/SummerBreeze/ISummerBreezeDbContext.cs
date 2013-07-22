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
        List<KeyMapping> SaveChanges(Dictionary<Type, List<EntityInfo>> saveMap);
        Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap);
        bool BeforeSaveEntity(EntityInfo entityInfo);
        List<string> GetTrackedEntities();
    }
}
