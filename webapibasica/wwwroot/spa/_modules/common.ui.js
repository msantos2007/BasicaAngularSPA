(function () {
    'use strict';

    angular.module('common.ui', ['ngAnimate', 'ngMaterial']) //, 'ngMessages'
        .config(function ($mdThemingProvider) {
            $mdThemingProvider.theme('default').dark();
                            //   .primaryPalette('blue')
                            //   .accentPalette('indigo')
                            //   .warnPalette('red')
                            //   .backgroundPalette('grey');
            // $mdThemingProvider.generateThemesOnDemand(true);
            // $mdTheming.generateTheme('regular');
        })
        .config(function ($mdAriaProvider) {   // Globally disables all ARIA warnings.
            $mdAriaProvider.disableWarnings();
        });
})();
