(function (app)
{
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', '$rootScope', '$location'];

    function indexCtrl($scope, $rootScope, $location)
    {
        $scope.pageClass = 'page-index';
        $rootScope.pageClassAtual = $scope.pageClass;

        var ctrlr = this;

        ctrlr.carregarSwagger = carregarSwagger;

        function carregarSwagger(href)
        {
            document.location.href = href;
        }

        ctrlr.hrefGoto = hrefGoto;

        function hrefGoto(caminho)
        {
            $location.path(caminho);
        }

        return ctrlr;
    }

})(angular.module('basicaspa'));