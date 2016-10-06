﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using Gdoc.Common.Utilitario;

namespace Gdoc.Dao
{
    public class DOperacion
    {
        public List<EOperacion> ListarOperacionBusqueda()
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 //join documentodigital in db.DocumentoDigitalOperacions
                                 //on operacion.IDOperacion equals documentodigital.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        tipooperacion.TipoConcepto.Equals("003") &&
                                        prioridad.TipoConcepto.Equals("005")

                                 select new { operacion, tipodocumento, /*documentodigital, usuariopart,*/ estado, tipooperacion, prioridad }).ToList();

                    list2.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,

                        //DocumentoDigitalOperacion=new DocumentoDigitalOperacion{
                        //    Comentario=x.documentodigital.Comentario,
                        //},

                        TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarDocumentosRecibidos(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var remitentes = new List<String>();
                    var listremitentes = (from remitente in db.UsuarioParticipantes

                                          join usuario in db.Usuarios
                                          on remitente.IDUsuario equals usuario.IDUsuario

                                          join operacion in db.Operacions
                                          on remitente.IDOperacion equals operacion.IDOperacion

                                          where 

                                           (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario 
                                            && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)) > 0)
                                            && remitente.TipoParticipante==Constantes.TipoParticipante.RemitenteDE
                                          select new { usuario}).ToList();

                    var list2 = (from operacion in db.Operacions

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        tipooperacion.TipoConcepto.Equals("003") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario 
                                            && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)) > 0)

                                 select new { operacion, tipodocumento, estado, tipooperacion, prioridad }).ToList();

                    
                    foreach (var item in listremitentes)
                    {
                        remitentes.Add(item.usuario.NombreUsuario);
                    }

                    foreach (var item in list2)
                    {
                        listOperacion.Add(new EOperacion
                        {
                            IDOperacion = item.operacion.IDOperacion,
                            IDEmpresa = item.operacion.IDEmpresa,
                            TipoOperacion = item.operacion.TipoOperacion,
                            FechaEmision = item.operacion.FechaEmision,
                            NumeroOperacion = item.operacion.NumeroOperacion,
                            TituloOperacion = item.operacion.TituloOperacion,
                            AccesoOperacion = item.operacion.AccesoOperacion,
                            EstadoOperacion = item.operacion.EstadoOperacion,
                            DescripcionOperacion = item.operacion.DescripcionOperacion,
                            PrioridadOperacion = item.operacion.PrioridadOperacion,
                            FechaCierre = item.operacion.FechaCierre,
                            FechaRegistro = item.operacion.FechaRegistro,
                            FechaEnvio = item.operacion.FechaEnvio,
                            FechaVigente = item.operacion.FechaVigente,
                            DocumentoAdjunto = item.operacion.DocumentoAdjunto,
                            TipoComunicacion = item.operacion.TipoComunicacion,
                            NotificacionOperacion = item.operacion.NotificacionOperacion,
                            TipoDocumento = item.operacion.TipoDocumento,
                            NombreFinal = item.operacion.NombreFinal,

                            TipoOpe = new Concepto { DescripcionCorta = item.tipooperacion.DescripcionCorta },
                            TipoDoc = new Concepto { DescripcionCorta = item.tipodocumento.DescripcionCorta },
                            Estado = new Concepto { DescripcionConcepto = item.estado.DescripcionConcepto },
                            Prioridad = new Concepto { DescripcionConcepto = item.prioridad.DescripcionConcepto },


                            Remitente = string.Join(",",remitentes.ToArray()),
                        });
                    }
                        //list2.ForEach(x => listOperacion.Add(new EOperacion
                        //{
                        //    IDOperacion = x.operacion.IDOperacion,
                        //    IDEmpresa = x.operacion.IDEmpresa,
                        //    TipoOperacion = x.operacion.TipoOperacion,
                        //    FechaEmision = x.operacion.FechaEmision,
                        //    NumeroOperacion = x.operacion.NumeroOperacion,
                        //    TituloOperacion = x.operacion.TituloOperacion,
                        //    AccesoOperacion = x.operacion.AccesoOperacion,
                        //    EstadoOperacion = x.operacion.EstadoOperacion,
                        //    DescripcionOperacion = x.operacion.DescripcionOperacion,
                        //    PrioridadOperacion = x.operacion.PrioridadOperacion,
                        //    FechaCierre = x.operacion.FechaCierre,
                        //    FechaRegistro = x.operacion.FechaRegistro,
                        //    FechaEnvio = x.operacion.FechaEnvio,
                        //    FechaVigente = x.operacion.FechaVigente,
                        //    DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        //    TipoComunicacion = x.operacion.TipoComunicacion,
                        //    NotificacionOperacion = x.operacion.NotificacionOperacion,
                        //    TipoDocumento = x.operacion.TipoDocumento,
                        //    NombreFinal = x.operacion.NombreFinal,

                        //    TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        //    TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        //    Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        //    Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },


                        //    Remitente = remitentes,

                        //}));
                    

                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarOperacionDigital(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db= new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join documentodigital in db.DocumentoDigitalOperacions
                                 on operacion.IDOperacion equals documentodigital.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && x.TipoParticipante == Constantes.TipoParticipante.EmisorDD) > 0)

                                 select new { operacion, tipodocumento, documentodigital,/* usuariopart,*/ estado }).ToList();

                    list2.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,
                        NombreFinal=x.operacion.NombreFinal,
                        DocumentoDigitalOperacion = new DocumentoDigitalOperacion
                        {
                            DerivarDocto = x.documentodigital.DerivarDocto,
                        },


                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                    }));

                    
                }
                
            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarOperacionElectronico(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    //var query1 = db.UsuarioParticipantes.Include("Operacion")
                    //                .Where(x=>x.IDUsuario == eUsuarioParticipante.IDUsuario).ToList();
                    //var operacionsert = db.Operacions.ToList();

                    //var list = db.Operacions.ToList();

                    var list = (from operacion in db.Operacions

                                 join documentoelectronico in db.DocumentoElectronicoOperacions
                                 on operacion.IDOperacion equals documentoelectronico.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 //join usuario in db.Usuarios
                                 //on usuariopart.IDUsuario equals usuario.IDUsuario

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && x.TipoParticipante == Constantes.TipoParticipante.RemitenteDE ) > 0)
                                        
                                 select new { operacion, tipodocumento, documentoelectronico, /*usuariopart,*/ estado /*,usuario*/ }).ToList();

                    list.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,
                        NombreFinal = x.operacion.NombreFinal,
                        DocumentoElectronicoOperacion = new DocumentoElectronicoOperacion
                        {
                            IDDoctoElectronicoOperacion=x.documentoelectronico.IDDoctoElectronicoOperacion,
                            IDOperacion=x.documentoelectronico.IDOperacion,
                            Memo = x.documentoelectronico.Memo,
                        },
                        
                        //UsuarioParticipante = new UsuarioParticipante
                        //{
                        //    IDUsuarioParticipante=x.usuariopart.IDUsuarioParticipante,
                        //    IDUsuario=x.usuariopart.IDUsuario,
                        //    TipoParticipante=x.usuariopart.TipoParticipante,
                        //},

                        //Usuario=new Usuario{
                        //    NombreUsuario=x.usuario.NombreUsuario,
                        //},
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarMesaVirtual(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join tipomesa in db.Conceptoes
                                 on operacion.TipoDocumento equals tipomesa.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 where tipomesa.TipoConcepto.Equals("011") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario &&  x.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV ) > 0)

                                 select new { operacion, tipomesa,  /*usuariopart,*/ estado /*,usuario*/, prioridad }).ToList();

                    list2.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,
                        NombreFinal = x.operacion.NombreFinal,
                        TipoDoc = new Concepto { DescripcionCorta = x.tipomesa.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionCorta = x.prioridad.DescripcionCorta },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarMesaTrabajoVirtual(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join tipomesa in db.Conceptoes
                                 on operacion.TipoDocumento equals tipomesa.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 join organizador in db.UsuarioParticipantes
                                 on operacion.IDOperacion equals organizador.IDOperacion

                                 join usuario in db.Usuarios
                                 on organizador.IDUsuario equals usuario.IDUsuario

                                 where tipomesa.TipoConcepto.Equals("011") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        //(operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario &&  (x.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV || x.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV)) > 0)
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) > 0)
                                        && organizador.TipoParticipante.Equals(Constantes.TipoParticipante.OrganizadorMV)
                                 select new { operacion, tipomesa,  /*usuariopart,*/ estado /*,usuario*/, prioridad,organizador,usuario }).ToList();

                    list2.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,

                        TipoDoc = new Concepto { DescripcionCorta = x.tipomesa.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionCorta = x.prioridad.DescripcionCorta },

                        OrganizadorMV = x.usuario.NombreUsuario,
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public short Grabar(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Operacions.Add(operacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public short EditarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Operacions.Find(operacion.IDOperacion);
                    entidad.TituloOperacion = operacion.TituloOperacion;
                    entidad.AccesoOperacion = operacion.AccesoOperacion;
                    entidad.DescripcionOperacion = operacion.DescripcionOperacion;
                    entidad.PrioridadOperacion = operacion.PrioridadOperacion;
                    entidad.TipoComunicacion = operacion.TipoComunicacion;
                    entidad.TipoDocumento = operacion.TipoDocumento;
                    entidad.FechaEnvio = operacion.FechaEnvio;
                    entidad.FechaVigente = operacion.FechaVigente;
                    entidad.FechaCierre = operacion.FechaCierre; 
                    entidad.NombreFinal = operacion.NombreFinal;
                    entidad.EstadoOperacion = operacion.EstadoOperacion;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public short EliminarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var ope = db.Operacions.Find(operacion.IDOperacion);
                    ope.EstadoOperacion = operacion.EstadoOperacion;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
