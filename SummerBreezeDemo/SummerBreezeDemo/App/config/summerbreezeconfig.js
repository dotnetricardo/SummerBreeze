define(['breeze', 'summerbreeze'],
    function (breeze, summerbreeze) {
        var
            serviceName = 'breeze/summerbreezedemo', // requires a summerbreezedemo controller
            generator = null,
            summerbreezeconfig = {
                getOrCreateGenerator: getOrCreateGenerator
            };

        return summerbreezeconfig;

        function getOrCreateGenerator() {
            if (generator) {
                return generator;
            } else {
                generator = new summerbreeze.generator({ service: serviceName })
                return generator;
            }
        }


    });