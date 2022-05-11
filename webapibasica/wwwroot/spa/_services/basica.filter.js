(function (app) {
    'use strict';

    app.filter('casasDecimais', function () {
        return function (input) {
            if (!input) return;
            var vlr = input;

            vlr = vlr.replace(/\./g, ':');
            vlr = vlr.replace(/,/g, ';');
            vlr = vlr.replace(/:/g, ',');
            vlr = vlr.replace(/;/g, '.');

            return vlr;
        };
    });

    app.filter('capitalize', function () {
        return function (input) {
            return (angular.isString(input) && input.length > 0) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : input;
        };
    });

})(angular.module('basicaspa'));
