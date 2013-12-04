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
                                                        orderby f.FechaEmision descending
                                                        select f).ToList<CabFactura>();
                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    fs.LoadWith<CabFactura>(x => x.Empresa);
                    fs.LoadWith<CabFactura>(x => x.Responsable);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        public virtual IEnumerable<CabFactura> GetPorEstado(string estado, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        where f.Estado == estado
                                                        orderby f.FechaEmision descending
                                                        select f).ToList<CabFactura>();
                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    fs.LoadWith<CabFactura>(x => x.Empresa);
                    fs.LoadWith<CabFactura>(x => x.Responsable);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        public virtual IEnumerable<CabFactura> Get(string proveedorId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    int pId = int.Parse(proveedorId);
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        where f.Proveedor.ProveedorId == pId
                                                        orderby f.FechaEmision descending
                                                        select f).ToList<CabFactura>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    fs.LoadWith<CabFactura>(x => x.Empresa);
                    fs.LoadWith<CabFactura>(x => x.Responsable);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        public virtual IEnumerable<CabFactura> GetHistoricas(string proveedorHId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    int pId = int.Parse(proveedorHId);
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        where f.Proveedor.ProveedorId == pId &&
                                                              (f.Estado == "PAGADA")
                                                        orderby f.FechaEmision descending
                                                        select f).ToList<CabFactura>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    fs.LoadWith<CabFactura>(x => x.Empresa);
                    fs.LoadWith<CabFactura>(x => x.Responsable);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        public virtual IEnumerable<CabFactura> GetEnGestion(string proveedorGId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    int pId = int.Parse(proveedorGId);
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        where f.Proveedor.ProveedorId == pId &&
                                                              (f.Estado != "PAGADA")
                                                        orderby f.FechaEmision descending
                                                        select f).ToList<CabFactura>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    fs.LoadWith<CabFactura>(x => x.Empresa);
                    fs.LoadWith<CabFactura>(x => x.Responsable);
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
        public virtual CabFactura Get(int id, string application, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    CabFactura factura = (from f in ctx.CabFacturas
                                          where f.CabFacturaId == id
                                          orderby f.FechaEmision descending
                                          select f).FirstOrDefault<CabFactura>();
                    if (factura != null)
                    {
                        //
                        if (factura.DocumentoPdf != null)
                            factura.DocumentoPdf.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(application, factura.DocumentoPdf, tk);
                        if (factura.DocumentoXml != null)
                            factura.DocumentoXml.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(application, factura.DocumentoXml, tk);
                        factura = ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoXml, x => x.DocumentoPdf, x=> x.Empresa, x=>x.Responsable);
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
                // comprobamos que no hay una factura para el mismo proveedor en ese año y con
                // el mismo número de factura
                CabFactura f = PortalProWebUtility.YaExisteUnaFacturaComoEsta(factura, ctx);
                if (f != null)
                {
                    string m = String.Format("Ya hay una factura del proveedor para este año {0:yyyy} con el mismo número {1}", f.FechaEmision, f.NumFactura);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, m));
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
                int empresaId = 0;
                if (factura.Empresa != null) 
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                // las facturas por defecto tienen el estado recibida
                factura.Estado = "ACEPTADA";
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
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                factura.FechaAlta = DateTime.Now;
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido creada con estado {3} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado);
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura Post(CabFactura factura, string userId, string tk)
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
                // comprobamos que no hay una factura para el mismo proveedor en ese año y con
                // el mismo número de factura
                CabFactura f = PortalProWebUtility.YaExisteUnaFacturaComoEsta(factura, ctx);
                if (f != null)
                {
                    string m = String.Format("Ya hay una factura del proveedor para este año {0:yyyy} con el mismo número {1}", f.FechaEmision, f.NumFactura);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, m));
                }
                // La aplicación ahora depende del comienzo del usuario
                string application = "PortalPro";
                switch (userId.Substring(0, 1))
                {
                    case "U":
                        application = "PortalPro2";
                        break;
                    case "G":
                        application = "PortalPro";
                        break;
                }
                // comprobamos si existen los ficheros que necesitamos
                string fPdf = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "PDF");
                if (fPdf == "")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Se necesita un fichero PDF asociado a la factura (CabFactura)"));
                }
                // el archivo Xml no es obligatorio, pero si lo han subido cargamos el fichero
                string fXml = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "XML");
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
                int empresaId = 0;
                if (factura.Empresa != null)
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                factura.Estado = "ACEPTADA";
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
                if (fPdf != "")
                {
                    factura.DocumentoPdf = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fPdf, ctx);
                }
                if (fXml != "")
                {
                    factura.DocumentoXml = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fXml, ctx);
                }
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                factura.FechaAlta = DateTime.Now;
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido creada con estado {3} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado);
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura Post(string numPed, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // comprobamos que hay un pedido que se corresopnden con el número pasado
                Pedido ped = (from p in ctx.Pedidos
                              where p.NumPedido == numPed
                              select p).FirstOrDefault<Pedido>();
                if (ped == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay pedido con el identificador pasado (generar factura) (CabFactura)"));
                }
                CabFactura factura = PortalProWebUtility.GenerarFacturaDesdePedido(ped, ctx);
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido generada con estado {3} a partir del pedido {4} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado, numPed);
                factura.Generada = true;
                ctx.SaveChanges();

                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura Put(int id, CabFactura factura, string userId, string tk)
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
                if (factura.Estado == "PROCESADA")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Las facturas procesadas no pueden ser modificadas (CabFactura)"));
                }
                // comprobamos que no hay una factura para el mismo proveedor en ese año y con
                // el mismo número de factura
                CabFactura fc = PortalProWebUtility.YaExisteUnaFacturaComoEsta(factura, ctx);
                if (fc != null)
                {
                    string m = String.Format("Ya hay una factura del proveedor para este año {0:yyyy} con el mismo número {1}", fc.FechaEmision, fc.NumFactura);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, m));
                }
                // La aplicación ahora depende del comienzo del usuario
                string application = "PortalPro";
                switch(userId.Substring(0, 1))
                {
                    case "U":
                        application = "PortalPro2";
                        break;
                    case "G":
                        application = "PortalPro";
                        break;
                }
                // En la actualización a lo mejor no han cargado ningún archivo
                string fPdf = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "PDF");
                // el archivo Xml no es obligatorio, pero si lo han subido cargamos el fichero
                string fXml = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "XML");
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
                int empresaId = 0;
                if (factura.Empresa != null)
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                if (factura.Estado == null)
                    factura.Estado = "ACEPTADA";
                // modificar el objeto
                ctx.AttachCopy<CabFactura>(factura);
                // volvemos a leer el objecto para que lo maneje este contexto.
                factura = (from f in ctx.CabFacturas
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
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                Documento doc = null; // para cargar temporalmente documentos
                // si se cumplen estas condiciones es que han cambiado el archivo asociado.
                if (fPdf != "")
                {
                    doc = factura.DocumentoPdf;
                    factura.DocumentoPdf = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fPdf, ctx);
                    PortalProWebUtility.EliminarDocumento(doc, ctx);
                }
                if (fXml != "")
                {
                    doc = factura.DocumentoXml;
                    factura.DocumentoXml = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fXml, ctx);
                    PortalProWebUtility.EliminarDocumento(doc, ctx);
                }
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido modificada con estado {3} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado);
                factura.Generada = false; ;
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura Put(int id, CabFactura factura, string userId, string tk, string gen)
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
                if (factura.Estado == "PROCESADA")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Las facturas procesadas no pueden ser modificadas (CabFactura)"));
                }
                // comprobamos que no hay una factura para el mismo proveedor en ese año y con
                // el mismo número de factura
                CabFactura fc = PortalProWebUtility.YaExisteUnaFacturaComoEsta(factura, ctx);
                if (fc != null)
                {
                    string m = String.Format("Ya hay una factura del proveedor para este año {0:yyyy} con el mismo número {1}", fc.FechaEmision, fc.NumFactura);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, m));
                }

                string application = "PortalPro";
                switch (userId.Substring(0, 1))
                {
                    case "U":
                        application = "PortalPro2";
                        break;
                    case "G":
                        application = "PortalPro";
                        break;
                }
                // En la actualización a lo mejor no han cargado ningún archivo
                string fPdf = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "PDF");
                if (fPdf == "")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Se necesita un fichero PDF asociado a la factura (CabFactura)"));
                }
                // el archivo Xml no es obligatorio, pero si lo han subido cargamos el fichero
                string fXml = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Factura", "XML");
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
                int empresaId = 0;
                if (factura.Empresa != null)
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                if (factura.Estado == null)
                    factura.Estado = "ACEPTADA";
                // modificar el objeto
                ctx.AttachCopy<CabFactura>(factura);
                // volvemos a leer el objecto para que lo maneje este contexto.
                factura = (from f in ctx.CabFacturas
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
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                Documento doc = null; // para cargar temporalmente documentos
                // si se cumplen estas condiciones es que han cambiado el archivo asociado.
                if (fPdf != "")
                {
                    doc = factura.DocumentoPdf;
                    factura.DocumentoPdf = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fPdf, ctx);
                    PortalProWebUtility.EliminarDocumento(doc, ctx);
                }
                if (fXml != "")
                {
                    doc = factura.DocumentoXml;
                    factura.DocumentoXml = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fXml, ctx);
                    PortalProWebUtility.EliminarDocumento(doc, ctx);
                }
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido modificada con estado {3} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado);
                factura.Generada = false; ;
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura PutCambioEstado(int id, CabFactura factura,string estado, string motivo, string userId, string tk)
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
                if (factura.Estado == "PROCESADA")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Las facturas procesadas no pueden ser modificadas (CabFactura)"));
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
                int empresaId = 0;
                if (factura.Empresa != null)
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                ctx.AttachCopy<CabFactura>(factura);
                // volvemos a leer el objecto para que lo maneje este contexto.
                factura = (from f in ctx.CabFacturas
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
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                factura.Estado = estado;
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € ha pasado al estado {3} por el usuario {4} con el motivo '{5}' <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado, userId, motivo);
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual CabFactura PutCambioRevison(int id, string estado, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // comprobar los formatos
                // primero buscamos si un factura con ese id existe
                CabFactura factura = (from f in ctx.CabFacturas
                                   where f.CabFacturaId == id
                                   select f).FirstOrDefault<CabFactura>();
                // existe?
                if (factura == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una factura con el id proporcionado (CabFactura)"));
                }
                factura.Estado = estado;
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € ha pasado al estado {3} por el usuario {4} con el motivo '{5}' <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado, userId, "Revisión previa");
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoPdf, x => x.DocumentoXml);
            }
        }

        public virtual bool PutAdmisionProceso(string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // se trata de pasar todas la faturas de aceptada a procesada
                var rs = (from f in ctx.CabFacturas
                          where f.Estado == "ACEPTADA"
                          select f);
                foreach (CabFactura fac in rs)
                {
                    // las cambiamos individualmente de estado y las grabamos
                    fac.Estado = "PROCESADA";
                    fac.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € ha pasado al estado {3} por el usuario {4} <br/>",
                                        DateTime.Now, fac.NumFactura, fac.TotalFactura, fac.Estado, userId);
                    ctx.SaveChanges();
                }
                return true;
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
                if (factura.Estado == "PROCESADA")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Las facturas procesadas no pueden ser modificadas (CabFactura)"));
                }
                // comprobamos que no hay una factura para el mismo proveedor en ese año y con
                // el mismo número de factura
                CabFactura fc = PortalProWebUtility.YaExisteUnaFacturaComoEsta(factura, ctx);
                if (fc != null)
                {
                    string m = String.Format("Ya hay una factura del proveedor para este año {0:yyyy} con el mismo número {1}", fc.FechaEmision, fc.NumFactura);
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, m));
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
                int empresaId = 0;
                if (factura.Empresa != null)
                {
                    empresaId = factura.Empresa.EmpresaId;
                    factura.Empresa = null;
                }
                int responsableId = 0;
                if (factura.Responsable != null)
                {
                    responsableId = factura.Responsable.ResponsableId;
                    factura.Responsable = null;
                }
                // modificar el objeto
                if (factura.Estado == null)
                    factura.Estado = "ACEPTADA";
                ctx.AttachCopy<CabFactura>(factura);
                // volvemos a leer el objecto para que lo maneje este contexto.
                factura = (from f in ctx.CabFacturas
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
                if (empresaId != 0)
                {
                    factura.Empresa = (from e in ctx.Empresas
                                       where e.EmpresaId == empresaId
                                       select e).FirstOrDefault<Empresa>();
                }
                if (responsableId != 0)
                {
                    factura.Responsable = (from r in ctx.Responsables
                                           where r.ResponsableId == responsableId
                                           select r).FirstOrDefault<Responsable>();
                }
                factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} con Total {2:0.0} € has sido modificada con estado {3} <br/>",
                    DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado);
                factura.Generada = false;
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
                if (cfac.Estado == "PROCESADA")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Las facturas procesadas no se pueden borrar (CabFactura)"));
                }
                // primero borarremos todas las líneas (con lo que se actualizan los pedidos asociados)
                PortalProWebUtility.EliminarLineasFactura(cfac.LinFacturas, ctx);
                Documento df = cfac.DocumentoPdf;
                Documento dx = cfac.DocumentoXml;
                ctx.Delete(cfac);
                PortalProWebUtility.EliminarDocumento(df, ctx);
                PortalProWebUtility.EliminarDocumento(dx, ctx);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}