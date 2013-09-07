define(['services/logger', 'config/summerbreezeconfig', 'config/breezeconfig', 'ko'], function (logger, summerbreezeconfig, breezeconfig, ko) {

    ko.observableArray.fn.pushAll = function (valuesToPush) {
        var underlyingArray = this();
        this.valueWillMutate();
        ko.utils.arrayPushAll(underlyingArray, valuesToPush);
        this.valueHasMutated();
        return this;  //optional
    };

    var
        DTOProducts = ko.observableArray(),
        results = ko.observable(),
        isVisible = ko.observable(false)

        vm = {
            activate: activate,
            title: 'Search View',
            viewAttached: viewAttached,
            DTOProducts: DTOProducts,
            results: results,
            isVisible: isVisible
        };

        //DTOProducts = ko.observableArray([{ Name: 'aaa', Price: '123', PicUrl: 'aaa' }]);
       


    function generateEntities() {
        return summerbreezeconfig.getOrCreateGenerator().generate();
    }


    return vm;

    function viewAttached(view) {

        //get summerbreeze to do its job
        generateEntities()
            .then(function () {
                primeChacheWithData()
                .then(wireUpEventHandler);
            });

    }

    //#region Internal Methods
    function activate() {

        logger.log('Search View Activated', null, 'search', true);
        return true;
    }

    function wireUpEventHandler() {
        //hook up eventhandlers onto the search form button
        $('#searchBtn').on('click', function () {

            var
                words = $('#searchBox').val(),
                predicates = [];

            if (words.trim().length > 0) {
                words = $('#searchBox').val().split(' '),



           predicates = $(words).map(function (idx, value) {
               return breeze.Predicate("Name", "contains", value);
           });


                //create query with breeze EntityQuery object
                var query = breezeconfig.entityQuery
                            .from('ProductDTO')
                            .using(breeze.FetchStrategy.FromLocalCache)
                            .toType('ProductDTO')
                            .where(breeze.Predicate.or($.makeArray(predicates)));

                //now grab summerbreeze manager and use it 
                summerbreezeconfig.getOrCreateGenerator().manager.executeQuery(query)
                .then(function (data) {  //from here onwardsthe DTO entities are already inside the summerbreeze manager cache
                    if (data.results.length > 0) {
                        results('Found ' + data.results.length + ' product(s).');
                    } else {
                        results('No products found.');
                    }

                    isVisible(true);

                    DTOProducts([]);
                    DTOProducts.pushAll(data.results);

                });
            } else {
                logger.logError('There are no search terms!', null, 'search', true);
            }

           
        });

      

    }

    function primeChacheWithData() {
        //create query with breeze EntityQuery object
        var
            query = breezeconfig.entityQuery
                    .from('ProductsDTO'),
            generator = summerbreezeconfig.getOrCreateGenerator();


        //clear cache to avoid duplicates
        generator.manager.clear()

        //now use summerbreeze manager and use it to fetch data from server
        return generator.manager.executeQuery(query);
                

    }
    
    //#endregion
});
