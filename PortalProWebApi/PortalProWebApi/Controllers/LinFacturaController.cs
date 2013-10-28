﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;
using Telerik.OpenAccess.FetchOptimization;

namespace PortalProWebApi.Controllers
{
    public class LinFacturaController : ApiController
    {
        /// <summary>
        /// Devuelve las líneas que corresponden a una factura determinada
        /// </summary>
        /// <param name="idFac">Identificador de la factura a la que pertencen las líneas</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<LinFactura> GetLineas(int idFac, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    // comprobamos si existe la fcatura de referencia.
                    CabFactura factura = (from f in ctx.CabFacturas
                                          where f.CabFacturaId == idFac
                                          select f).FirstOrDefault<CabFactura>();
                    if (factura != null)
                    {
                        IEnumerable<LinFactura> lineas = factura.LinFacturas;
                        FetchStrategy fs = new FetchStrategy();
                        fs.LoadWith<LinFactura>(x => x.CabFactura);
                        lineas = ctx.CreateDetachedCopy<IEnumerable<LinFactura>>(lineas,fs);
                        return lineas;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un factura con el id proporcionado (LinFactura)"));
                    }

                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
            }
        }


        /// <summary>
        /// Devuelve la línea de factura que coincide con el id proporcionado
        /// </summary>
        /// <param name="id">Identificador único de la línea</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual LinFactura Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    LinFactura linea = (from l in ctx.LinFacturas
                                          where l.LinFacturaId == id
                                          select l).FirstOrDefault<LinFactura>();
                    if (linea != null)
                    {
                        linea = ctx.CreateDetachedCopy<LinFactura>(linea, x => x.CabFactura);
                        return linea;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un linea con el id proporcionado (LinFactura)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
            }
        }

        /// <summary>
        /// Crear una nueva línea de factura
        /// </summary>
        /// <param name="LinFactura">Objeto a crear, el atributo LinFacturaId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual LinFactura Post(LinFactura linea, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
                // comprobar las precondiciones
                if (linea == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int cabFacturaId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (linea.CabFactura != null)
                {
                    cabFacturaId = linea.CabFactura.CabFacturaId;
                    linea.CabFactura = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(linea);
                if (cabFacturaId != 0)
                {
                    linea.CabFactura = (from f in ctx.CabFacturas
                                            where f.CabFacturaId == cabFacturaId
                                            select f).FirstOrDefault<CabFactura>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<LinFactura>(linea, x => x.CabFactura);
            }
        }

        public virtual bool Put(int idFac, IEnumerable<LinFactura> lineas, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
                // comprobamos que las lineas no son nulas
                if (lineas == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // comprobamos que la factura a la que se asociarán las líneas existe
                CabFactura factura = (from f in ctx.CabFacturas
                                      where f.CabFacturaId == idFac
                                      select f).FirstOrDefault<CabFactura>();
                if (factura == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe una factura con el id proporcionado (LinFactura)"));
                }
                // eliminamos las líneas de fcatura anteriores para solo dar de alta estas
                ctx.Delete(factura.LinFacturas);
                // ahora damos de alta las nuevas lineas 
                decimal totalFactura = 0;
                foreach (LinFactura linea in lineas)
                {
                    LinFactura l = new LinFactura()
                    {
                        NumeroPedido = linea.NumeroPedido,
                        Descripcion = linea.Descripcion,
                        Importe = linea.Importe,
                        PorcentajeIva = linea.PorcentajeIva,
                        CabFactura = factura
                    };
                    totalFactura += linea.Importe;
                    ctx.Add(l);
                }
                factura.TotalFactura = totalFactura;
                ctx.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Modificar una linea de factura. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la linea de fcatura</param>
        /// <param name="linea">Linea de fcatura con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual LinFactura Put(int id, LinFactura linea, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
                // comprobar los formatos
                if (linea == null || id != linea.LinFacturaId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si uan linea con ese id existe
                LinFactura lin = (from l in ctx.LinFacturas
                               where l.LinFacturaId == id
                               select l).FirstOrDefault<LinFactura>();
                // existe?
                if (lin == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una linea con el id proporcionado (LinFactura)"));
                }
                int cabFacturaId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (lin.CabFactura != null)
                {
                    cabFacturaId = lin.CabFactura.CabFacturaId;
                    linea.CabFactura = null;
                }
                // modificar el objeto
                ctx.AttachCopy<LinFactura>(linea);
                // volvemos a leer el objecto para que lo maneje este contexto.
                linea = (from l in ctx.LinFacturas
                           where l.LinFacturaId == id
                           select l).FirstOrDefault<LinFactura>();
                if (cabFacturaId != 0)
                {
                    linea.CabFactura = (from f in ctx.CabFacturas
                                            where f.CabFacturaId == cabFacturaId
                                            select f).FirstOrDefault<CabFactura>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<LinFactura>(linea, x => x.CabFactura);
            }
        }

        /// <summary>
        /// Elimina la linea que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la linea a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
                // primero buscamos si un grupo con ese id existe
                LinFactura lin = (from l in ctx.LinFacturas
                               where l.LinFacturaId == id
                               select l).FirstOrDefault<LinFactura>();
                // existe?
                if (lin == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Usuarios)"));
                }
                ctx.Delete(lin);
                ctx.SaveChanges();
                return true;
            }
        }

    }
}
