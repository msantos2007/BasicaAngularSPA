(function (app)
{
    'use strict';

    app.directive('focusIf', focusIf); //uso: <elm focus-if="true" focus-delay="300">/<elm> //usado com variavel: ex. visitanteNovo.html
    focusIf.$inject = ['$timeout'];
    function focusIf($timeout)
    {
        function link($scope, $element, $attrs)
        {
            var dom = $element[0];
            if ($attrs.focusIf)
            {
                $scope.$watch($attrs.focusIf, focus);
            } else
            {
                focus(true);
            }
            function focus(condition)
            {
                if (condition)
                {
                    $timeout(function ()
                    {
                        dom.focus();
                    }, $scope.$eval($attrs.focusDelay) || 0);
                }
            }
        }
        return {
            restrict: 'A',
            link: link
        };
    }

})(angular.module('basicaspa'));
