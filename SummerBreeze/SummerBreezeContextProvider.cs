using Breeze.WebApi;
using SummerBreeze;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace SummerBreeze

{
    public class SummerBreezeContextProvider<T> : ContextProvider where T : ISummerBreezeDbContext
    {
        private IKernel _kernel;
        private Assembly _assembly = null;
        public T Context { get; private set; }
        
       

        public SummerBreezeContextProvider(string assemblyName = null)
        {
            /*bind types*/
            _kernel = new StandardKernel();
            _kernel.Bind<ISummerBreezeDbContext>().To<T>();

            Context = (T)_kernel.Get<ISummerBreezeDbContext>();

            if (assemblyName == null)
            {
                _assembly = Assembly.GetCallingAssembly();
            }
            else
            {
                try
                {
                    var an = Assembly.GetCallingAssembly().GetReferencedAssemblies().Where(a => a.Name == assemblyName).FirstOrDefault();
                    _assembly = Assembly.Load(an);
                   
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }

        }

        /*base overrides*/

        protected override string BuildJsonMetadata()
        {
            return GetMetadataFromAssembly();
        }

        //protected override List<KeyMapping> SaveChangesCore(Dictionary<Type, List<EntityInfo>> saveMap)
        //{
        //    return _kernel.Get<ISummerBreezeDbContext>().SaveChanges(saveMap);
        //}

        protected override void SaveChangesCore(SaveWorkState saveWorkState)
        {
            _kernel.Get<ISummerBreezeDbContext>().SaveChanges(saveWorkState);
            
        }

        protected override Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            return _kernel.Get<ISummerBreezeDbContext>().BeforeSaveEntities(saveMap);
        }

        protected override bool BeforeSaveEntity(EntityInfo entityInfo)
        {
            return _kernel.Get<ISummerBreezeDbContext>().BeforeSaveEntity(entityInfo);
        }

        public List<string> GetTrackedEntities()
        {
            return _kernel.Get<ISummerBreezeDbContext>().GetTrackedEntities();
        }

        
        private string GetMetadataFromAssembly()
        {
            
            var entityMedatataList = new List<BreezeEntityTypeMetadata>();

            var entity = _assembly.GetTypes().Where(x => x.GetCustomAttributes(typeof(BreezeLocalizableAttribute), false).Length > 0).ToList();
            
            entity.ForEach((e) =>
            {
                entityMedatataList.Add(GetEntityTypeMetadata(e));
            });

            return JsonConvert.SerializeObject(entityMedatataList);
        }

        private BreezeEntityTypeMetadata GetEntityTypeMetadata(Type t)
        {
            var autogeneratedKeyAttr = t.GetCustomAttributes(typeof(BreezeAutoGeneratedKeyTypeAttribute), false) != null ? (t.GetCustomAttributes(typeof(BreezeAutoGeneratedKeyTypeAttribute), false).FirstOrDefault() as BreezeAutoGeneratedKeyTypeAttribute).AutoGeneratedKeyType.ToString() : null;
            var allUnmapped = ((t.GetCustomAttributes(typeof(BreezeLocalizableAttribute), false).FirstOrDefault() as BreezeLocalizableAttribute).UnMapAll);
            var metadata = new BreezeEntityTypeMetadata();
            
            metadata.shortName = t.Name;
            metadata.@namespace = t.Namespace;
            metadata.autoGeneratedKeyType = autogeneratedKeyAttr ?? SummerBreezeEnums.AutoGeneratedKeyType.None.ToString();

            var datapropertyMetadataList = new List<BreezeDataPropertyMetadata>();
            var navigationpropertyMetadataList = new List<BreezeNavigationPropertyMetadata>();

            var propertyInfo = GetPropertyInfo(t);

            propertyInfo.ForEach((i) =>
            {
                
                //Navigation Properties
                var nav = i.GetCustomAttributes(typeof(BreezeNavigationPropertyAttribute), false).FirstOrDefault();
                if (nav != null)
                {
                    navigationpropertyMetadataList.Add(new BreezeNavigationPropertyMetadata
                     {
                         name = i.Name,
                         entityTypeName = i.PropertyType.GetGenericArguments().FirstOrDefault() == null ? i.PropertyType.Name + ":#" + i.PropertyType.Namespace : i.PropertyType.GetGenericArguments().FirstOrDefault().Name + ":#" + i.PropertyType.GetGenericArguments().FirstOrDefault().Namespace,
                         isScalar = (nav as BreezeNavigationPropertyAttribute).ForeignKeyNames != null && (nav as BreezeNavigationPropertyAttribute).ForeignKeyNames.Count() > 0,
                         associationName = (nav as BreezeNavigationPropertyAttribute).Association,
                         foreignKeyNames = (nav as BreezeNavigationPropertyAttribute).ForeignKeyNames
                       

                     });

                  

                }
                else
                {
                    //Data Properties
                    datapropertyMetadataList.Add(new BreezeDataPropertyMetadata
                    {
                        name = i.Name,
                        dataType = i.PropertyType.Name.Contains("Nullable") ? Nullable.GetUnderlyingType(i.PropertyType).Name : i.PropertyType.GetGenericArguments().FirstOrDefault() == null ? i.PropertyType.Name : i.PropertyType.GetGenericArguments().FirstOrDefault().Name,
                        isNullable = i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>),
                        isPartOfKey = i.GetCustomAttributes(typeof(KeyAttribute), false) != null && i.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0,
                        isUnmapped = allUnmapped ? allUnmapped : i.GetCustomAttributes(typeof(BreezeUnmappedAttribute), false) != null && i.GetCustomAttributes(typeof(BreezeUnmappedAttribute), false).Count() > 0,
                        validators = GetValidatorsForProperty(i)

                    });

                  
                }
                
                //if (i.GetGetMethod().IsVirtual)
                //{
                //    navigationpropertyMetadataList.Add(new NoDbNavigationPropertyMetadata 
                //    { 
                //         name = i.Name,
                //         entityTypeName = i.PropertyType.Name,
                //         isScalar = true,
                //         associationName = "
                //    });
                //}


                //add navigation properties
                if (navigationpropertyMetadataList.Count > 0)
                {
                    metadata.navigationProperties = navigationpropertyMetadataList;
                }

                //add data properties
                if (datapropertyMetadataList.Count > 0)
                {
                    metadata.dataProperties = datapropertyMetadataList;
                }
               
                
            });

           

            return metadata;

        }

        private List<string> GetValidatorsForProperty(PropertyInfo i)
        {
            var validatorsList = new List<string>();

            //Required
            var required = i.GetCustomAttributes(typeof(RequiredAttribute), false);

            if (required != null && required.Length > 0)
            {
                validatorsList.Add("breeze.Validator.required()");
            }

            var maxLength = i.GetCustomAttributes(typeof(MaxLengthAttribute), false);

            //MaxLength
            if (maxLength != null && maxLength.Length > 0)
            {
                validatorsList.Add("breeze.Validator.maxLength({ maxLength:" + (maxLength.FirstOrDefault() as MaxLengthAttribute).Length + "})");
            }

            return validatorsList;

        }

        private List<PropertyInfo> GetPropertyInfo(Type t)
        {
            List<PropertyInfo> info = null;

            t.GetCustomAttributes(typeof(BreezeLocalizableAttribute), true).ToList().ForEach((a) =>
            {
                var attr = (a as BreezeLocalizableAttribute);

                if (attr.Include == null || attr.Include.Length == 0)
                    info = t.GetProperties().ToList(); //all properties
                else
                    info = t.GetProperties().Where(i => attr.Include.Any(o => o.Equals(i.Name))).ToList();  //cherry picked properties
            });


            return info;
        }

        protected override void CloseDbConnection()
        {
           
        }

        public override System.Data.IDbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }

        protected override void OpenDbConnection()
        {
           
        }

       
    }
}
