define(['durandal/system', 'durandal/plugins/router', 'services/logger'],
    function (system, router, logger) {
 	
        var adminRoutes = ko.computed(function () {
            return router.allRoutes().filter(function (r) {
                return r.settings.admin;
            });
        });

        var shell = {
            activate: activate,
            adminRoutes: adminRoutes,
            router: router
        };
        
        return shell;

        //#region Internal Methods
        function activate() {
            return boot();
        }

        function boot() {
            router.mapNav('search');
            router.mapNav('add');
            log('Hot Towel SPA Loaded!', null, true);
            return router.activate('search');
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });