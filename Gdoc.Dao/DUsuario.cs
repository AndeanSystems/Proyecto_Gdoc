﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DUsuario
    {
        public Usuario ValidarLogin(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    return db.Usuarios.Where(x => x.NombreUsuario == usuario.NombreUsuario && x.ClaveUsuario == usuario.ClaveUsuario).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<EUsuario> ListarUsuario()
        {
            var listUsuario = new List<EUsuario>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list4 = (from u in db.Usuarios
                                 join p in db.Personals
                                 on u.IDPersonal equals p.IDPersonal

                                 join Cargo in db.Conceptoes
                                 on p.CodigoCargo equals Cargo.CodiConcepto

                                 join Tipo in db.Conceptoes
                                 on u.CodigoTipoUsua equals Tipo.CodiConcepto

                                 join Area in db.Conceptoes
                                 on p.CodigoArea equals Area.CodiConcepto

                                 join Clase in db.Conceptoes
                                 on u.ClaseUsuario equals Clase.CodiConcepto

                                 where Cargo.TipoConcepto.Equals("007") &&
                                         Tipo.TipoConcepto.Equals("010") &&
                                         Area.TipoConcepto.Equals("013") &&
                                         Clase.TipoConcepto.Equals("021")


                                 select new { u, p, Cargo, Tipo, Area, Clase }).ToList();


                    list4.ForEach(x => listUsuario.Add(new EUsuario
                    {
                        IDUsuario = x.u.IDUsuario,
                        NombreUsuario = x.u.NombreUsuario,
                        FirmaElectronica = x.u.FirmaElectronica,
                        EstadoUsuario = x.u.EstadoUsuario,
                        FechaRegistro = x.u.FechaRegistro,
                        FechaUltimoAcceso = x.u.FechaUltimoAcceso,
                        FechaModifica = x.u.FechaModifica,
                        IntentoErradoClave = x.u.IntentoErradoClave,
                        IntentoerradoFirma = x.u.IntentoerradoFirma,
                        TerminalUsuario = x.u.TerminalUsuario,
                        UsuarioRegistro = x.u.UsuarioRegistro,
                        CodigoConexion = x.u.CodigoConexion,
                        IDPersonal = x.u.IDPersonal,
                        CodigoRol = x.u.CodigoRol,
                        CodigoTipoUsua = x.u.CodigoTipoUsua,
                        ExpiraClave = x.u.ExpiraClave,
                        ExpiraFirma = x.u.ExpiraFirma,
                        FechaExpiraClave = x.u.FechaExpiraClave,
                        FechaExpiraFirma = x.u.FechaExpiraFirma,
                        Personal = new Personal
                        {
                            IDPersonal = x.p.IDPersonal,
                            IDEmpresa = x.p.IDEmpresa,
                            CodigoCargo = x.p.CodigoCargo,
                            CodigoArea = x.p.CodigoArea,
                            NombrePers = x.p.NombrePers,
                            ApellidoPersonal = x.p.ApellidoPersonal,
                            EmailTrabrajo = x.p.EmailTrabrajo,
                            TelefonoPersonal = x.p.TelefonoPersonal

                        },
                        Cargo = new Concepto { DescripcionConcepto = x.Cargo.DescripcionConcepto },
                        TipoUsuario = new Concepto { DescripcionConcepto = x.Tipo.DescripcionConcepto },
                        Area = new Concepto { DescripcionConcepto = x.Area.DescripcionConcepto },
                        ClaseUsu = new Concepto { DescripcionConcepto = x.Clase.DescripcionConcepto }

                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listUsuario;
        }

    }
}