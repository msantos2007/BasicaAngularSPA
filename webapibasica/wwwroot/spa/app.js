(function ()
{
    'use strict';

    angular.module('basicaspa', ['common.core', 'common.ui'])
        .run(run)
        .config(config);

    config.$inject = ['$routeProvider', '$locationProvider'];
    function config($routeProvider, $locationProvider)
    {

        $routeProvider
            .when("/",
                {
                    templateUrl: "/spa/home/index.html"
                    , controller: "indexCtrl"
                    , controllerAs: "ctrlr"
                }).when("/register",
                    {
                        templateUrl: "/spa/register/register.html"
                        , controller: "registerCtrl"
                        , controllerAs: "ctrlr"

                        // }).when("/login",
                        // {
                        //       templateUrl: "scripts/spa/account/login.html"
                        //     , controller: "loginCtrl"

                    }).otherwise({ redirectTo: "/" });

        $locationProvider.html5Mode(true);
    }

    run.$inject = [];
    function run()
    {
        //carregaAddToHomeScreen
        //repository.loggedUser
        //isAuthenticated
    }

})();