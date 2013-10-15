using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;
using Telerik.OpenAccess.FetchOptimization;

namespace PortalProWebApi.Controllers
{
    public class FacturaController : ApiController
    {
        /// <summary>
        /// Obtiene todos las cabeceras de factura de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<CabFactura> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        select f).ToList<CabFactura>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        /// <summary>
        /// Obtiene la cabecera de factura cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la factura</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual CabFactura Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    CabFactura factura = (from f in ctx.CabFacturas
                                          where f.CabFacturaId == id
                                          select f).FirstOrDefault<CabFactura>();
                    if (factura != null)
                    {
                        factura = ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoXml, x => x.DocumentoPdf);
                        return factura;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un factura con el id proporcionado (CabFactura)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        /// <summary>
        /// Crear un nueva cabecera de factura
        /// </summary>
        /// <param name="CabFactura">Objeto a crear, el atributo CabFacturaId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual CabFactura Post(CabFactura factura, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // comprobar las precondiciones
                if (factura == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }

                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (factura.Proveedor != null)
                {
                    proveedorId = factura.Proveedor.ProveedorId;
                    factura.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (factura.DocumentoPdf != null)
                {
                    documentoPdfId = factura.DocumentoPdf.DocumentoId;
                    factura.DocumentoPdf = null;
                }
                int documentoXmlId = 0;
                if (factura.DocumentoXml != null)
                {
                    documentoXmlId = factura.DocumentoXml.DocumentoId;
                    factura.DocumentoXml = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(factura);
                if (proveedorId != 0)
                {
                    factura.Proveedor = (from p in ctx.Proveedors
                                         where p.ProveedorId == proveedorId
                                         select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    factura.DocumentoPdf = (from d in ctx.Documentos
                                            where d.DocumentoId == documentoPdfId
                                            select d).FirstOrDefault<Documento>();
                }
                if (documentoXmlId != 0)
                {
                    factura.DocumentoXml = (from d in ctx.Documentos
                                            where d.DocumentoId == documentoXmlId
                                            select d).FirstOrDefault<Documento>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        /// <summary>
        /// Modificar una cabecera de factura. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la cabecera de factura </param>
        /// <param name="factura">Cabecera de factura los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual CabFactura Put(int id, CabFactura factura, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // comprobar los formatos
                if (factura == null || id != factura.CabFacturaId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un factura con ese id existe
                CabFactura cfac = (from f in ctx.CabFacturas
                                   where f.CabFacturaId == id
                                   select f).FirstOrDefault<CabFactura>();
                // existe?
                if (cfac == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una factura con el id proporcionado (CabFactura)"));
                }
                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (factura.Proveedor != null)
                {
                    proveedorId = factura.Proveedor.ProveedorId;
                    factura.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (factura.DocumentoPdf != null)
                {
                    documentoPdfId = factura.DocumentoPdf.DocumentoId;
                    factura.DocumentoPdf = null;
                }
                int documentoXmlId = 0;
                if (factura.DocumentoXml != null)
                {
                    documentoXmlId = factura.DocumentoXml.DocumentoId;
                    factura.DocumentoXml = null;
                }
                // modificar el objeto
                ctx.AttachCopy<CabFactura>(cfac);
                // volvemos a leer el objecto para que lo maneje este contexto.
                cfac = (from f in ctx.CabFacturas
                        where f.CabFacturaId == id
                        select f).FirstOrDefault<CabFactura>();
                if (proveedorId != 0)
                {
                    factura.Proveedor = (from p in ctx.Proveedors
                                         where p.ProveedorId == proveedorId
                                         select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    factura.DocumentoPdf = (from d in ctx.Documentos
                                            where d.DocumentoId == documentoPdfId
                                            select d).FirstOrDefault<Documento>();
                }
                if (documentoXmlId != 0)
                {
                    factura.DocumentoXml = (from d in ctx.Documentos
                                            where d.DocumentoId == documentoXmlId
                                            select d).FirstOrDefault<Documento>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        /// <summary>
        /// Elimina la factura que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la factura a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // primero buscamos si una factura con ese id existe
                CabFactura cfac = (from f in ctx.CabFacturas
                                   where f.CabFacturaId == id
                                   select f).FirstOrDefault<CabFactura>();
                // existe?
                if (cfac == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una factura con el id proporcionado (CabFactura)"));
                }
                ctx.Delete(cfac);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}