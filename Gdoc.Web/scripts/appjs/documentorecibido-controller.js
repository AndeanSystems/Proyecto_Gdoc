﻿(function () {
    'use strict';
    angular.module('app').controller('documentorecibido_controller', documentorecibido_controller);
    documentorecibido_controller.$inject = ['$location', 'app_factory', 'appService'];
    function documentorecibido_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        var context = this;
        context.operacion = {};

        context.mostrarPDF = function (rowIndex) {
            context.operacion = context.gridOptions.data[rowIndex];
            dataProvider.postData("DocumentosRecibidos/ListarDocumentoPDF", context.operacion).success(function (respuesta) {
                console.log(respuesta)
                window.open(respuesta, '_blank');
            }).error(function (error) {
                //MostrarError();
            });

            
            
            
        }
        context.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            data: [],
            appScopeProvider: context,
            columnDefs: [
                { field: 'NumeroOperacion', displayName: 'Nº Documento' },
                { field: 'TipoOpe.DescripcionCorta', displayName: 'T.Oper' },
                { field: 'TipoDoc.DescripcionCorta', displayName: 'T.Doc' },
                { field: 'TituloOperacion', displayName: '	Titulo' },
                { field: 'FechaRegistro', displayName: 'Fecha Emisión', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                { field: 'FechaVigente', displayName: '	Fecha Recepción', cellFilter: 'toDateTime | date:"dd/MM/yyyy HH:mm:ss"' },
                {
                    name: 'Ver',
                    cellTemplate: '<i class="fa fa-paperclip" ng-click="grid.appScope.mostrarPDF(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" target="_blank" data-placement="bottom" data-toggle="tooltip" title="Abrir pdf"></i>' +
                            '<i ng-click="grid.appScope.editarOperacion(grid.renderContainers.body.visibleRowCache.indexOf(row))" style="padding: 4px;font-size: 1.4em;" class="fa fa-eye" data-placement="bottom" data-toggle="tooltip" title="Adjuntos"></i>'
                }
            ]
        };
        //Eventos

        //Metodos
        context.buscarLogOperacion = function (operacion) {
            dataProvider.postData("LogOperacion/ListarLogOperacion", operacion).success(function (respuesta) {
                context.gridOptions.data = respuesta;
            }).error(function (error) {
                //MostrarError();
            });
        }

        function listarOperacion() {
            dataProvider.getData("DocumentosRecibidos/ListarOperacion").success(function (respuesta) {
                context.gridOptions.data = respuesta;
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }
        //Carga
        listarOperacion();
    }
})();
