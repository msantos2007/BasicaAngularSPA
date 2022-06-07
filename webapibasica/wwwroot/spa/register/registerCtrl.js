(function (app) {
    'use strict';

    app.controller('registerCtrl', registerCtrl);

    registerCtrl.$inject = ['$scope', '$rootScope', 'apiService', '$timeout', 'notificationService'];

    function registerCtrl($scope, $rootScope, apiService, $timeout, notificationService) {
        $scope.pageClass = 'page-register';
        $rootScope.pageClassAtual = $scope.pageClass;
        var ctrlr = this;

        ctrlr.BuscarAlunos = BuscarAlunos;
        ctrlr.BuscarImagens = BuscarImagens;
        ctrlr.FileUpload = FileUpload;
        ctrlr.Arquivos = [];

        function FileUpload(files) {
            debugger;
            fileUploadService.uploadImage(files, 'api/ImageUpload', FileUploadSuccess);

            function FileUploadSuccess() {
                notificationService.displaySuccess('Mensagem disparada com sucesso!');
            }
        }

        function BuscarAlunos() {
            var config;

            apiService.get('/api/Basica/BuscarAlunos', config, BuscarAlunosCompleted, null);

            function BuscarAlunosCompleted(result) {
                ctrlr.AlunosLista = result.data;

                for (var x = 0; x < 5; x++) {
                    var novo = [];
                    novo.Nome = "dummy";
                    novo.imagem = 1;
                    ctrlr.AlunosLista.push(novo);
                }

                $timeout(function () {
                    ctrlr.BuscarImagens();
                }, 100);
            }
        }

        function BuscarImagens() {
            var config;
            apiService.get('/api/UploadFile/ImageList', config, BuscarImagensCompleted, null);

            function BuscarImagensCompleted(result) {
                var imagens = result.data;

                imagens.forEach(function (cada) {
                    ctrlr.AlunosLista.forEach(function (aluno) {
                        if (!aluno.imagem) {
                            aluno.imagem = cada;
                            return;
                        }
                    });
                });
            }
        }

        $timeout(function () {
            ctrlr.BuscarAlunos();
        }, 100);


        return ctrlr;
    }

})(angular.module('basicaspa'));