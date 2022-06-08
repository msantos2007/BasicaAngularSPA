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
        ctrlr.BuscarImagens = BuscarImagens;
        ctrlr.FileUpload = FileUpload;
        ctrlr.Arquivos = [];

        function FileUpload(files)
        {
            debugger;
            fileUploadService.uploadImage(files, 'api/ImageUpload', FileUploadSuccess);

            function FileUploadSuccess()
            {
                notificationService.displaySuccess('Mensagem disparada com sucesso!');
            }
        }

        function BuscarAlunos()
        {
            var config;

            apiService.get('/api/Basica/BuscarAlunos/', config, BuscarAlunosCompleted, null);

            function BuscarAlunosCompleted(result)
            {

                ctrlr.AlunosLista = result.data;

                var listaImagem = [];

                ctrlr.AlunosLista.forEach(function (cada)
                {
                    if (cada.alunoImagens.length)
                    {
                        listaImagem.push(cada.alunoImagens[0]);
                    }
                });

                $timeout(function ()
                {
                    if (listaImagem.length) ctrlr.BuscarImagens(listaImagem, AssociarImagem);
                });


                function AssociarImagem(listaImagem)
                {

                    listaImagem.forEach(function (cada)
                    {
                        ctrlr.AlunosLista.forEach(function (aluno)
                        {
                            if (aluno.id == cada.alunoId && cada.imagemByte)
                            {
                                aluno.imagem = cada.imagemByte;
                                return;
                            }
                        });
                    });
                }
            }
        }

        function BuscarImagens(listaImagem, callback)
        {
            //var config = listaImagem;
            apiService.post('/api/UploadFile/ImageList/', listaImagem, BuscarImagensCompleted, BuscarImagensFailed);

            function BuscarImagensCompleted(result)
            {
                var imagens = result.data;
                callback(imagens);
            }

            function BuscarImagensFailed(response)
            {
                debugger;
            }
        }

        $timeout(function ()
        {
            ctrlr.BuscarAlunos();
        }, 100);


        return ctrlr;
    }

})(angular.module('basicaspa'));