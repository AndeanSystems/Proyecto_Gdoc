﻿(function () {
    'use strict';

    angular.module('app').controller('documentodigital_controller', documentodigital_controller);
    documentodigital_controller.$inject = ['$location', 'app_factory', 'appService'];

    function documentodigital_controller($location, dataProvider, appService) {
        /* jshint validthis:true */
        ///Variables
        let TipoDocumento = "012";
        let PrioridadAtencion = "005";
        let TipoAcceso = "002";
        let TipoComunicacion = "022";

        var context = this;
        context.operacion = {};
        context.referencia = {};
        context.visible = "List";
        context.listaReferencia = [];

        //Crear Combo Auto Filters
        var pendingSearch, cancelSearch = angular.noop;
        var cachedQuery, lastSearch;
        context.usuarioRemitentes = [];
        context.usuarioDestinatarios = [];
        context.filterSelected = true;
        context.querySearch = querySearch;
        var usuario = {};


        LlenarConcepto(TipoDocumento);
        LlenarConcepto(TipoAcceso);
        //LlenarConcepto(TipoDocumento);
        LlenarConcepto(PrioridadAtencion);
        //LlenarConcepto(TipoAcceso);
        //LlenarConcepto(TipoComunicacion);

        //context.gridOptions = {
        //    paginationPageSizes: [25, 50, 75],
        //    paginationPageSize: 25,
        //    enableFiltering: true,
        //    data: [],
        //    columnDefs: [
        //        { field: 'CodiConcepto', displayName: 'Codigo' },
        //        { field: 'DescripcionConcepto', displayName: 'Descripcion' },
        //        { field: 'DescripcionCorta', displayName: 'Abreviatura' },
        //        { field: 'ValorUno', displayName: 'Valor1' },
        //        { field: 'ValorDos', displayName: 'Valor2' },
        //        { field: 'TextoUno', displayName: 'Texto1' },
        //        { field: 'TextoDos', displayName: 'Texto2' },
        //        { field: 'EstadoConcepto', displayName: 'Estado' },
        //        { field: 'Empresa.DireccionEmpresa', displayName: 'Direción Empresa' }
        //    ]
        //};
        //Eventos
        context.agregarreferencia = function (referencia) {
            if (context.referencia.DescripcionIndice == undefined)
                alert("Ingrese Referencia");
            else {
                context.listaReferencia.push(context.referencia);
                console.log(context.listaReferencia);
                context.referencia = {};
            }
        }

        context.eliminarreferencia = function (referencia) {
            console.log(referencia);
            context.listaReferencia.splice(referencia, 1);//POR TERMINAR
            //context.referencia = {};
        }

        context.grabar = function () {
            console.log(context.operacion);
            let Operacion = context.operacion;
            //let DocumentoElectronicoOperacion = context.DocumentoElectronicoOperacion;
            //let listEUsuarioGrupo = [];
            //for (var ind in context.usuarioDestinatarios) {
            //    listEUsuarioGrupo.push(context.usuarioDestinatarios[ind]);
            //}
            //for (var ind in context.usuarioRemitentes) {
            //    listEUsuarioGrupo.push(context.usuarioRemitentes[ind]);
            //}
            console.log(context.DocumentoElectronicoOperacion);
            dataProvider.postData("DocumentoDigital/Grabar", { Operacion: Operacion}).success(function (respuesta) {
                console.log(respuesta);
            }).error(function (error) {
                //MostrarError();
            });
        }

        context.CambiarVentana = function (mostrarVentana) {
            context.visible = mostrarVentana;
            if (context.visible != "List") {
            }
        }
        ////
        function listarUsuarioGrupoAutoComplete(Nombre) {
            var UsuarioGrupo = { Nombre: Nombre };
            appService.buscarUsuarioGrupoAutoComplete(UsuarioGrupo).success(function (respuesta) {
                //context.listaUsuario = respuesta;
                context.listaUsuarioGrupo = respuesta;
            });
        }

        function querySearch(criteria) {
            listarUsuarioGrupoAutoComplete(criteria);
            cachedQuery = cachedQuery || criteria;
            return cachedQuery ? context.listaUsuarioGrupo : [];
        }

        function LlenarConcepto(tipoConcepto) {
            var concepto = { TipoConcepto: tipoConcepto };
            appService.listarConcepto(concepto).success(function (respuesta) {
                if (concepto.TipoConcepto == TipoDocumento)
                    context.listTipoDocumento = respuesta;
                else if (concepto.TipoConcepto == PrioridadAtencion)
                    context.listPrioridadAtencion = respuesta;
                else if (concepto.TipoConcepto == TipoAcceso)
                    context.listTipoAcceso = respuesta;
                else if (concepto.TipoConcepto == TipoComunicacion)
                    context.listTipoComunicacion = respuesta;
            });
        }
    }
})();
