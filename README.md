SummerBreeze
===========================

Metadata library and client-side entity generation script for BreezeJS for no DB, no EF ORM or EF ORM with DTO scenarios.


Instructions 
===========================


<h3>Getting metadata and creating entities:</h3>
<p><b>On the server-side of things:</b></p>
1 - Add reference to SummerBreeze.dll assembly.

2 - Decorate your POCOS with custom attributes (see <a href="https://github.com/dotnetricardo/SummerBreeze/wiki/Attributes">attributes</a> section).

3 - Create a custom ContextProvider class that implements the ISummerBreezeDbContext.

```
 public class DTOContextProvider : ISummerBreezeDbContext
    {
        public Dictionary<Type, List<Breeze.WebApi.EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<Breeze.WebApi.EntityInfo>> saveMap)
        {
            //implement custom behaviour here
            return saveMap;
        }

        public bool BeforeSaveEntity(Breeze.WebApi.EntityInfo entityInfo)
        {
            //implement custom behaviour here
            return true;
        }

         public void SaveChanges(Breeze.WebApi.SaveWorkState saveWorkState)
        {
            //implement custom save changes here
            throw new NotImplementedException();
        }


        public List<string> GetTrackedEntities()
        {
            var list = new List<string>();
            list.AddRange(new[] { "DTOModel" });

            return list;
        }
    }
```

4 - On your controller create an instance of SummerBreezeContextProvider and handle it the previous created contextprovider class.
Choose one of two ways to initialize the class.
```
   readonly SummerBreezeContextProvider<TodoContext> _summerBreezeContextProvider = new  SummerBreezeContextProvider<TodoContext>(); //don´t pass any parameter to the constructor if your models are in the same assembly of your project
```
```
    readonly SummerBreezeContextProvider<TodoContext> _summerBreezeContextProvider = new SummerBreezeContextProvider<TodoContext>("DataLayer.dll"); //pass the assembly name to the constructor if your models reside in another referenced assembly
```

5 - Create a SummerBreezeMetadata method that returns a string and call the Metadata method of your SummerBreezaContextProvider and you´re done.
```
        [AcceptVerbs("GET")]
        public string SummerBreezeMetadata()
        {
            return _summerBreezeContextProvider.Metadata();
        }
```



<p><b>On the client-side of things:</b></p>

1 - Add reference to the summerbreeze.js script right after breeze.js and it´s dependencies.

2 - Create a generator instance from the global summerBreeze object by passing an initialization object with your service name, if saving queues are allowed and if entity saves are automatically processed once changes are detected. 

3- Call the generator generate method to create all the entity types and add them its internal breeze metadatastore. This method returns a promise that is useful if you have custom initialization options.
```
     var generator = new summerBreeze.generator( { service:'breeze/Todo'
                                                   enableQueing: true, //requires reference to breeze.savequeuing.js
                                                   autoSave: true }); //false if you want to manually call the breeze savechanges method
                                                   

     generator.generate()
         .then(function () {
             registerCtors(); //if you have custom entity initialization logic register it here
         });
```

***
<h3>Queriying:</h3>
<p><b>On the server-side of things:</b></p>
1 - Create a method on your controller that returns an IQueryable<T> where T is the type of the DTO object you want to serialize to the client.
```
   [AcceptVerbs("GET")]
        public IQueryable<DTOModel> DTOModels()
        {
            var list = new List<DTOModel>();
            list.Add(new DTOModel { 
                Id = 1,
                Name = "NumberOne",
                IsTestable = true
            });
            
             list.Add(new DTOModel
            {
                Id = 2,
                Name = "NumberTwo",
                IsTestable = false
            });

             return list.AsQueryable();
        }
```
<p><b>On the client-side of things:</b></p>

1 - Create a query with breeze EntityQuery object.
```
  var query = EntityQuery.from('DTOModels');
```

2 - Grab the manager object from the SummerBreeze generator and execute query.
```
var mgr = generator.manager;
mgr.executeQuery(query);
```

***
<h3>Saving changes to entities:</h3>
<p><b>On the server-side of things:</b></p>
1 -Create a SaveChanges method on your controller. Remember if you´re using both EFContextProvider and SummerBreezeContextProvider to do some sort of logic to see which one to use.
In this example we´ve used the method on the ISummerBreezeDbContext GetTrackedEntities to return a list of the entities that where generated with SummerBreeze and use that context if a match is found.
```
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
            return _contextProvider.SaveChanges(saveBundle);
        }
```
<p><b>On the client-side of things:</b></p>
1 - Grab the manager object from the SummerBreeze generator and execute query.
```
var mgr = generator.manager;
```
2 - Create a new entity.
```
var newDTO = mgr.createEntity('DTOModel', { Name: 'Acme', IsTestable: false });
```

3 - Save changes.
```
if (mgr.hasChanges()) {
                mgr.saveChanges();
            }
```



Demo 
===========================
Check out the included SummerBreezeDemo to see this library in action.
