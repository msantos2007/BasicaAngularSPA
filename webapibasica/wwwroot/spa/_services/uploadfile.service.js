(function (app) {
    'use strict';

    app.factory('fileUploadService', fileUploadService);

    fileUploadService.$inject = ['$rootScope', '$upload', 'notificationService', 'apiService'];

    function fileUploadService($rootScope, $upload, notificationService, apiService)
    {
        $rootScope.upload = [];

        var service = {
            uploadImage: uploadImage,
            uploadImageGeneric: uploadImageGeneric,
            removeImageGeneric: removeImageGeneric,
            existsImageGeneric: existsImageGeneric
        };

        function uploadImage($files, mensagemId, callback)
        {
            //2022: mensagemId carrega todo o caminho
            //      antes: "./api/mensagem/images/upload?mensagemId=" + mensagemId

            //$files: an array of files selected                 
            for (var i = 0; i < $files.length; i++)
            {
                var $file = $files[i];
                (function (index)
                {
                    $rootScope.upload[index] = $upload.upload({
                        url: mensagemId, // webapi url
                        method: "POST",
                        file: $file
                    }).progress(function (evt)
                    {
                    }).success(function (data, status, headers, config)
                    {
                        // file is uploaded successfully                        
                        notificationService.displaySuccess(data.FileName + ' uploaded com sucesso!');
                        if (index === $files.length - 1) callback();
                    }).error(function (data, status, headers, config)
                    {
                        notificationService.displayError(data.Message);
                    });
                })(i);
            }
        }

        //Depois do Angular 1.6.6 | Generic == _imagery
        function uploadImageGeneric($files, caminho_completo, callback)
        {
            //2022: antes "./api/Account/images/upload?caminho_completo=" + caminho_completo

            //$files: an array of files selected                 
            for (var i = 0; i < $files.length; i++)
            {
                var $file = $files[i];
                (function (index)
                {
                    $rootScope.upload[index] = $upload.upload({
                        url: caminho_completo, // webapi url
                        method: "POST",
                        file: $file
                    }).then(function (data, status, headers, config)
                    {
                        // file is uploaded successfully                        
                        notificationService.displaySuccess('Imagem salva com sucesso!');
                        if (index === $files.length - 1) callback();
                    }, function (data, status, headers, config)
                        {
                            notificationService.displayError(data.Message);
                        });
                })(i);
            }
        }
        
        function removeImageGeneric(caminho_completo, callback, callback_attr)
        {
            //Antes: 'api/Account/images/remove?caminho_completo=' + caminho_completo
            apiService.post(caminho_completo, null, callback, callback_attr);
        }

        function existsImageGeneric(caminho_completo, callback)
        {
            //Antes: 'api/Account/images/exists?caminho_completo=' + caminho_completo
            apiService.get(caminho_completo, null, callback, null);
        }
        return service;
    }

})(angular.module('common.core'));