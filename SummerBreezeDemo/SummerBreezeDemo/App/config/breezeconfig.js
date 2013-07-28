define(['breeze'],
    function (breeze) {

        var
            serviceName = 'breeze/summerbreezedemo', // requires a summerbreezedemo controller
            entityModel = breeze.entityModel,
            store = new entityModel.MetadataStore(),
            entityQuery = breeze.EntityQuery;

        //entityModel.NamingConvention.camelCase.setAsDefault();

        var breezeconfig = {
            createManager: createManager,
            entityQuery: entityQuery
        };

       
        function createManager() {
            return new entityModel.EntityManager({
                serviceName: serviceName, 
                metadataStore: store
            });
        };
       
        return breezeconfig;
        
    });