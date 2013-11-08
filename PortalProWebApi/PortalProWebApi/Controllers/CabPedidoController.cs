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
    public class PedidoController : ApiController
    {
        /// <summary>
        /// Obtiene todos los pedidos de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Pedido> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Pedido> pedidos = (from f in ctx.Pedidos
                                                   select f).ToList<Pedido>();

                    // Copiar y montar las url de los documentos asociados.
                    foreach (Pedido ped in pedidos)
                    {
                        if (ped.DocumentoPdf != null)
                            ped.DocumentoPdf.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(ped.DocumentoPdf, tk);
                    }
                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Pedido>(x => x.Proveedor);
                    fs.LoadWith<Pedido>(x => x.DocumentoPdf);
                    pedidos = ctx.CreateDetachedCopy<IEnumerable<Pedido>>(pedidos, fs);
                    return pedidos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
            }
        }

        public virtual IEnumerable<Pedido> Get(string proveedorId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    // Comprobamos que hay un proveedor que coincide
                    int pId = int.Parse(proveedorId);
                    IEnumerable<Pedido> pedidos = (from f in ctx.Pedidos
                                                   where f.Proveedor.ProveedorId == pId
                                                   select f).ToList<Pedido>();

                    // Copiar y montar las url de los documentos asociados.
                    foreach (Pedido ped in pedidos)
                    {
                        if (ped.DocumentoPdf != null)
                            ped.DocumentoPdf.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(ped.DocumentoPdf, tk);
                    }
                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Pedido>(x => x.Proveedor);
                    fs.LoadWith<Pedido>(x => x.DocumentoPdf);
                    pedidos = ctx.CreateDetachedCopy<IEnumerable<Pedido>>(pedidos, fs);
                    return pedidos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
            }
        }


        /// <summary>
        /// Obtiene la cabecera de pedido cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del pedido</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Pedido Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Pedido pedido = (from f in ctx.Pedidos
                                     where f.PedidoId == id
                                     select f).FirstOrDefault<Pedido>();
                    if (pedido != null)
                    {
                        //
                        if (pedido.DocumentoPdf != null)
                            pedido.DocumentoPdf.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(pedido.DocumentoPdf, tk);
                        pedido = ctx.CreateDetachedCopy<Pedido>(pedido, x => x.Proveedor, x => x.DocumentoPdf);
                        return pedido;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un pedido con el id proporcionado (Pedido)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
            }
        }

        /// <summary>
        /// Crear un nueva cabecera de pedido
        /// </summary>
        /// <param name="Pedido">Objeto a crear, el atributo PedidoId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Pedido Post(Pedido pedido, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
                // comprobar las precondiciones
                if (pedido == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (pedido.Proveedor != null)
                {
                    proveedorId = pedido.Proveedor.ProveedorId;
                    pedido.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (pedido.DocumentoPdf != null)
                {
                    documentoPdfId = pedido.DocumentoPdf.DocumentoId;
                    pedido.DocumentoPdf = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(pedido);
                if (proveedorId != 0)
                {
                    pedido.Proveedor = (from p in ctx.Proveedors
                                        where p.ProveedorId == proveedorId
                                        select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    pedido.DocumentoPdf = (from d in ctx.Documentos
                                           where d.DocumentoId == documentoPdfId
                                           select d).FirstOrDefault<Documento>();
                }
                pedido.FechaAlta = DateTime.Now;
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Pedido>(pedido, x => x.Proveedor, x => x.DocumentoPdf);
            }
        }

        public virtual Pedido Post(Pedido pedido, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
                // comprobar las precondiciones
                if (pedido == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // comprobamos si existen los ficheros que necesitamos
                string fPdf = PortalProWebUtility.BuscarArchivoCargado("PortalPro", userId, "Pedido", "PDF");
                if (fPdf == "")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Se necesita un fichero PDF asociado a la pedido (Pedido)"));
                }
                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (pedido.Proveedor != null)
                {
                    proveedorId = pedido.Proveedor.ProveedorId;
                    pedido.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (pedido.DocumentoPdf != null)
                {
                    documentoPdfId = pedido.DocumentoPdf.DocumentoId;
                    pedido.DocumentoPdf = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(pedido);
                if (proveedorId != 0)
                {
                    pedido.Proveedor = (from p in ctx.Proveedors
                                        where p.ProveedorId == proveedorId
                                        select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    pedido.DocumentoPdf = (from d in ctx.Documentos
                                           where d.DocumentoId == documentoPdfId
                                           select d).FirstOrDefault<Documento>();
                }
                if (fPdf != "")
                {
                    pedido.DocumentoPdf = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(fPdf, ctx);
                }
                pedido.FechaAlta = DateTime.Now;
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Pedido>(pedido, x => x.Proveedor, x => x.DocumentoPdf);
            }
        }

        public virtual Pedido Put(int id, Pedido pedido, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
                // comprobar los formatos
                if (pedido == null || id != pedido.PedidoId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un factura con ese id existe
                Pedido ped = (from p in ctx.Pedidos
                              where p.PedidoId == id
                              select p).FirstOrDefault<Pedido>();
                // existe?
                if (ped == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una factura con el id proporcionado (Pedido)"));
                }
                // En la actualización a lo mejor no han cargado ningún archivo
                string fPdf = PortalProWebUtility.BuscarArchivoCargado("PortalPro", userId, "Factura", "PDF");
                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (pedido.Proveedor != null)
                {
                    proveedorId = pedido.Proveedor.ProveedorId;
                    pedido.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (pedido.DocumentoPdf != null)
                {
                    documentoPdfId = pedido.DocumentoPdf.DocumentoId;
                    pedido.DocumentoPdf = null;
                }
                int documentoXmlId = 0;
                // modificar el objeto
                ctx.AttachCopy<Pedido>(pedido);
                // volvemos a leer el objecto para que lo maneje este contexto.
                pedido = (from p in ctx.Pedidos
                          where p.PedidoId == id
                          select p).FirstOrDefault<Pedido>();
                if (proveedorId != 0)
                {
                    pedido.Proveedor = (from p in ctx.Proveedors
                                        where p.ProveedorId == proveedorId
                                        select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    pedido.DocumentoPdf = (from d in ctx.Documentos
                                           where d.DocumentoId == documentoPdfId
                                           select d).FirstOrDefault<Documento>();
                }
                Documento doc = null; // para cargar temporalmente documentos
                // si se cumplen estas condiciones es que han cambiado el archivo asociado.
                if (fPdf != "")
                {
                    doc = pedido.DocumentoPdf;
                    pedido.DocumentoPdf = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(fPdf, ctx);
                    PortalProWebUtility.EliminarDocumento(doc, ctx);
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Pedido>(pedido, x => x.Proveedor, x => x.DocumentoPdf);
            }
        }

        /// <summary>
        /// Modificar una cabecera de pedido. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la cabecera de factura </param>
        /// <param name="pedido">Cabecera de pedido los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Pedido Put(int id, Pedido pedido, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
                // comprobar los formatos
                if (pedido == null || id != pedido.PedidoId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un factura con ese id existe
                Pedido ped = (from p in ctx.Pedidos
                              where p.PedidoId == id
                              select p).FirstOrDefault<Pedido>();
                // existe?
                if (ped == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un pedido con el id proporcionado (Pedido)"));
                }
                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (pedido.Proveedor != null)
                {
                    proveedorId = pedido.Proveedor.ProveedorId;
                    pedido.Proveedor = null;
                }
                int documentoPdfId = 0;
                if (pedido.DocumentoPdf != null)
                {
                    documentoPdfId = pedido.DocumentoPdf.DocumentoId;
                    pedido.DocumentoPdf = null;
                }
                // modificar el objeto
                ctx.AttachCopy<Pedido>(pedido);
                // volvemos a leer el objecto para que lo maneje este contexto.
                pedido = (from p in ctx.Pedidos
                          where p.PedidoId == id
                          select p).FirstOrDefault<Pedido>();
                if (proveedorId != 0)
                {
                    pedido.Proveedor = (from p in ctx.Proveedors
                                        where p.ProveedorId == proveedorId
                                        select p).FirstOrDefault<Proveedor>();
                }
                if (documentoPdfId != 0)
                {
                    pedido.DocumentoPdf = (from d in ctx.Documentos
                                           where d.DocumentoId == documentoPdfId
                                           select d).FirstOrDefault<Documento>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Pedido>(pedido, x => x.Proveedor, x => x.DocumentoPdf);
            }
        }

        /// <summary>
        /// Elimina el pedido que coincide con el id pasado
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
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Pedido)"));
                }
                // primero buscamos si una factura con ese id existe
                Pedido ped = (from f in ctx.Pedidos
                              where f.PedidoId == id
                              select f).FirstOrDefault<Pedido>();
                // existe?
                if (ped == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay pedido con el id proporcionado (Pedido)"));
                }
                ctx.Delete(ped);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}