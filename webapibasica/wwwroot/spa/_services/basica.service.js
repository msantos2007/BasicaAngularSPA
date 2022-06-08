(function (app)
{
    'use strict';

    app.factory('basicaService', basicaService);

    basicaService.$inject = ['moment', 'apiService'];

    function basicaService(moment, apiService)
    {

        var service = {
            pad: pad
            , isNumeric: isNumeric
            , PrintElemHTML: PrintElemHTML
            , clearSelection: clearSelection

            , ServerLocalTime: ServerLocalTime  //Retorna data-hora do dispositivo, corrigido pelo deslocamento obtido por ServerLocalNow (Brasilia)
            , ServerLocalBRZT: ServerLocalBRZT  //Retorna data-hora do servidor, Brasilia

            , ServerUTCNow: ServerUTCNow        //Requisita: Retorna ou callback data-hora servidor em UTC
            , ServerLocalNow: ServerLocalNow    //Requisita: Retorna ou callback data-hora servidor em UTC, convertido para Brasilia

            , Any2Noon: Any2Noon
            , Any2SQL: Any2SQL

            , clrLocalStorage_obj: clrLocalStorage_obj
            , chkLocalStorage_obj: chkLocalStorage_obj
            , setLocalStorage_obj: setLocalStorage_obj
            , getLocalStorage_obj: getLocalStorage_obj


        };

        var timeoffset = 0;

        function ServerLocalTime()
        {
            //HTML: | amTimezone: 'America/Sao_Paulo'
            var date = moment().add(timeoffset, 'miliseconds');
            return date.toDate();
        }

        function ServerLocalBRZT()
        {
            var date = moment().add(timeoffset, 'miliseconds');
            return moment.tz(date.utc(), "America/Sao_Paulo");
        }

        function ServerUTCNow(callback)
        {
            apiService.get('api/financeiro/buscarDataServidorUTC/', null, buscarDataServidorUTC_success, buscarDataServidorUTC_failed, callback, null);

            function buscarDataServidorUTC_success(result, callback, attr)
            {
                if (!callback) return moment(result.data.data).toISOString();

                callback(moment(result.data.data).toISOString());
            }

            function buscarDataServidorUTC_failed(response)
            {
                return null;
            }
        }

        function ServerLocalNow(callback)
        {
            apiService.get('api/financeiro/buscarDataServidorLocal/', null, buscarDataServidorLocal_success, buscarDataServidorLocal_failed, callback, null);

            function buscarDataServidorLocal_success(result, callback, attr)
            {
                if (!callback) return new Date(moment(result.data.data).toISOString());

                callback(new Date(moment(result.data.data).toISOString()));
            }

            function buscarDataServidorLocal_failed(response)
            {
                return null;
            }
        }

        function Any2SQL(givenDate)
        {
            return moment(givenDate).format("YYYY-MM-DDTHH:mm:ss.SSS");
        }

        function Any2Noon(givenDate, no_milisseconds_precision, zulu)
        {
            //no_milisseconds_precision: because of combo box matching: ex. buscarReferenciaDisponivel
            no_milisseconds_precision = no_milisseconds_precision || false;

            //estruturaTarifariaModal.html
            zulu = zulu || false;
            let retorno;

            if (no_milisseconds_precision) retorno = moment(givenDate).format("YYYY-MM-DDT12:00:00");
            if (!no_milisseconds_precision) retorno = moment(givenDate).format("YYYY-MM-DDT12:00:00.000");
            if (zulu) retorno = retorno + "Z";

            return retorno;
        }

        function clearSelection()
        {
            if (document.selection && document.selection.empty)
            {
                document.selection.empty();
            } else if (window.getSelection)
            {
                var sel = window.getSelection();
                sel.removeAllRanges();
            }
        }

        function setLocalStorage_obj(key, value_obj)
        {
            if (!key || !value_obj) return false;

            var jsonObject = JSON.stringify(value_obj);
            try
            {
                localStorage.setItem(key, jsonObject);
            }
            catch (e)
            {
                return false;
            }

            return true;
        }

        function getLocalStorage_obj(key)
        {
            var retorno = undefined;
            var json_string = localStorage.getItem(key);

            if (json_string)
            {
                var obj = JSON.parse(json_string);
                retorno = obj;
            }

            return retorno;
        }

        function chkLocalStorage_obj(key_pattern, partial_key)
        {
            partial_key = partial_key || false;

            var arr = []; // Array to hold the keys
            // Iterate over localStorage and insert the keys that meet the condition into arr
            if (!partial_key)
            {
                for (let i = 0; i < localStorage.length; i++)
                {
                    if (localStorage.key(i) === key_pattern)
                    {
                        arr.push(localStorage.key(i));
                    }
                }
            }

            if (partial_key)
            {
                for (let i = 0; i < localStorage.length; i++)
                {
                    if (localStorage.key(i).includes(key_pattern))
                    {
                        arr.push(localStorage.key(i));
                    }
                }
            }

            return arr.length;
        }

        function clrLocalStorage_obj(key_pattern, partial_key)
        {
            partial_key = partial_key || false;

            var arr = []; // Array to hold the keys
            // Iterate over localStorage and insert the keys that meet the condition into arr
            if (!partial_key)
            {
                for (let i = 0; i < localStorage.length; i++)
                {
                    if (localStorage.key(i) === key_pattern)
                    {
                        arr.push(localStorage.key(i));
                    }
                }
            }

            if (partial_key)
            {
                for (let i = 0; i < localStorage.length; i++)
                {
                    if (localStorage.key(i).includes(key_pattern))
                    {
                        arr.push(localStorage.key(i));
                    }
                }
            }

            // Iterate over arr and remove the items by key
            for (let i = 0; i < arr.length; i++)
            {
                localStorage.removeItem(arr[i]);
            }


            return arr.length;
        }

        function pad(n, width, z)
        {
            z = z || '0';
            n = n + '';
            return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
        }

        function isNumeric(n)
        {
            //Number.isInteger(novoValor)
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        function PrintElemHTML(title, elem, header, innerHTML)
        {
            if (!elem && !innerHTML) return;

            window.print();

            return true;
        }



        return service;
    }

})(angular.module('common.core'));