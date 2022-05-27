(function (app)
{
    'use strict';

    app.controller('registerCtrl', registerCtrl);

    registerCtrl.$inject = ['$scope', '$rootScope', 'apiService', '$timeout', 'notificationService'];

    function registerCtrl($scope, $rootScope, apiService, $timeout, notificationService)
    {
        $scope.pageClass = 'page-register';
        $rootScope.pageClassAtual = $scope.pageClass;
        var ctrlr = this;

        ctrlr.BuscarAlunos = BuscarAlunos;
        ctrlr.FileUpload = FileUpload;
        ctrlr.Arquivos = [];

        function FileUpload(files)
        {
            notificationService.displaySuccess('Mensagem disparada com sucesso!');
        }

        function BuscarAlunos()
        {
            var config;

            apiService.get('/api/Basica/BuscarAlunos', config, BuscarAlunosCompleted, null);

            function BuscarAlunosCompleted(result)
            {
                ctrlr.AlunosLista = result.data;
            }
        }

        $timeout(function()
        {
            ctrlr.BuscarAlunos();
        }, 500);
        

        return ctrlr;
    }

})(angular.module('basicaspa'));