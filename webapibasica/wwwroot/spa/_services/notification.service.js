(function (app) {
    'use strict';

    app.factory('notificationService', notificationService);

    notificationService.$inject = ['$mdToast'];

    function notificationService($mdToast) {
        
        var service = {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        };   
        
        function templating(message, delay, css_class)
        {            
            //pass vs inject 
            $mdToast.show(
                $mdToast.simple()
                .toastClass(css_class)
                .capsule(true)
                .textContent(message)
                .position('top right')
                .hideDelay(delay))
                .then(function() {
                $log.log('Toast dismissed.');
                }).catch(function() {
                $log.log('Toast failed or was forced to close early by another toast.');
                });
        }

        function displaySuccess(message)
        {
            templating(message, 3000, 'custom_toastsuccess');
        }

        function displayError(message, maisTempo)
        {
            maisTempo = 3000 + (maisTempo  || 0);
            templating(message, maisTempo, 'custom_toasterror');
        }

        function displayWarning(message)
        { 
            templating(message, 3000, 'custom_toastwarning');
        }

        function displayInfo(message, maisTempo)
        { 
            templating(message, 3000 + maisTempo, 'custom_toastinfo');
        }

        

        return service;
    }

})(angular.module('common.core'));