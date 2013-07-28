define(['services/logger', 'config/breezeconfig', 'config/summerbreezeconfig'], function (logger, breezeconfig, summerbreezeconfig) {
    var vm = {
        activate: activate,
        title: 'Add Product View',
        viewAttached: viewAttached
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Add Product View Activated', null, 'add', true);
        return true;
    }

    function viewAttached(view) {
        wireUpEventHandlers();
    }

    function wireUpEventHandlers() {

        $('input[placeholder="Price"]').on('keydown', function (e) {
            return isNumber(e);
        });

        $('#addBtnDTO').on('click', function () {
            var
                name = $('input[placeholder="Product Name"]').eq(0).val(),
                url = $('input[placeholder="Picture Url"]').eq(0).val(),
                price = $('input[placeholder="Price"]').eq(0).val(),
                date = moment().format('YYYY/MM/DD'),
                arr = [name, url, price, date];
               


            if (isFormValid(arr)) {
                // create a new Product DTO and save it
                arr.push(date);
                createNewDTOAndSave(arr);

            } else {
                logger.logError('Please fill in all (DTO) form fields.', null, 'search', true);
            }

        });

        $('#addBtnDB').on('click', function () {
            var
                name = $('input[placeholder="Product Name"]').eq(1).val(),
                url = $('input[placeholder="Picture Url"]').eq(1).val(),
                tags = $('input[placeholder="Tags"]').eq(0).val(),
                price = $('input[placeholder="Price"]').eq(1).val(),
                arr = [name, url, tags, price];

            if (isFormValid(arr)) {
                // create a new Product DB Object and save it
                createNewDBObjectAndSave(arr);
            } else {
                logger.logError('Please fill in all (DB) form fields.', null, 'add', true);
            }

        });
    }

    function isFormValid(values) {
        var result = true;
        $(values).each(function (idx, elem) {
            if (elem.trim() === '') {
                result = false;
            }
        });

        return result;
    }

    function isNumber(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode != 43 && charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function createNewDTOAndSave(values) {
        var generator = summerbreezeconfig.getOrCreateGenerator();
        var mgr = generator.manager;
        var newDTO = mgr.createEntity('ProductDTO', { Name: values[0], PicUrl: values[1], Price: values[2], SearchDate: values[3] });

        if (mgr.hasChanges()) {
            mgr.saveChanges()
                .then(saveOK)
                .fail(saveKO);
        }
    }

    function createNewDBObjectAndSave(values) {
        var mgr = breezeconfig.createManager();

        //fetch metadata first
        mgr.fetchMetadata()
        .then(function () {
            //create new Product
            var newProduct = mgr.createEntity('Product', { Name: values[0], PicUrl: values[1], Tags: values[2], Price: values[3] });
            if (mgr.hasChanges()) {
                mgr.saveChanges()
                .then(saveOK)
                .fail(saveKO);
            }
        });
    }

    function saveOK() {
        logger.log('Product saved successfully.', null, 'add', true);
        
    }

    function saveKO() {
        logger.logError('An error ocurred. Please try again.', null, 'add', true);
    }

    //#endregion
});