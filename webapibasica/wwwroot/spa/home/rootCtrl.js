(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope', '$rootScope', '$location', '$window', '$timeout', 'apiService', '$mdDialog', '$filter'];

    function rootCtrl($scope, $rootScope, $location, $window, $timeout, apiService, $mdDialog, $filter) {
        $scope.pageClass = 'page-root';
        $rootScope.pageClassAtual = $scope.pageClass;

        var ctrlr = this;
        ctrlr.loading = true;

        // ctrlr.hrefGoto = hrefGoto;

        // function hrefGoto(caminho)
        // {
        //     $rootScope.urlLocation(caminho);
        // }

        $timeout(function () {
            ctrlr.loading = false;
        });

        return ctrlr;
    }

})(angular.module('basicaspa'));