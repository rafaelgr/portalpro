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
    public class DocumentosController : ApiController
    {
        /// <summary>
        /// Obtiene todos los documentos de la base de datos. Lo que se devuelven los objetos tal cual
        /// sin haber sido copiados a su directorio de descarga y sin haber propocionado el enlace para hacerla
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Documento> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Documento> documentos = (from gu in ctx.Documentos
                                                     select gu).ToList<Documento>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Documento>(x => x.Proveedor);
                    fs.LoadWith<Documento>(x => x.TipoDocumento);
                    documentos = ctx.CreateDetachedCopy<IEnumerable<Documento>>(documentos, fs);
                    return documentos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Documentos)"));
                }
            }
        }

        /// <summary>
        /// Se obtienen todos los documentos pertenecientes a un poveedor.
        /// Los documentos se copian al directorio de descarga y se proporciona su enlace en la propiedad "DescargaUrl"
        /// </summary>
        /// <param name="tk">Tique de autorización</param>
        /// <param name="codigoProveedor">Código del proveedor del que se solcitan los documentos.</param>
        /// <returns></returns>
        public virtual IEnumerable<Documento> Get(string tk, string codigoProveedor)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    // comprobamos que el proveedor pasado existe
                    int id = 0;
                    bool res = int.TryParse(codigoProveedor, out id);
                    Proveedor proveedor = (from p in ctx.Proveedors
                                                     where p.ProveedorId == id
                                                     select p).FirstOrDefault<Proveedor>();
                    if (proveedor == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Debe proporcionar un proveedor existente"));
                    }
                    IEnumerable<Documento> documentos = (from d in ctx.Documentos
                                                         where d.Proveedor.ProveedorId == id
                                                         select d).ToList<Documento>();
                    // por cada documento hay que copiarlo al directorio de descarga
                    // y proporcionar el enlace
                    foreach (Documento doc in documentos)
                    {
                        string root = System.Web.HttpContext.Current.Server.MapPath("~/downloads");
                        string resultado = PortalProWebUtility.ObtenerUrlDeDocumento(root, tk, doc, ctx);
                        if (resultado != "")
                        {
                            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado));
                        }
                    }
                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Documento>(x => x.Proveedor);
                    fs.LoadWith<Documento>(x => x.TipoDocumento);
                    documentos = ctx.CreateDetachedCopy<IEnumerable<Documento>>(documentos, fs);
                    return documentos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Documentos)"));
                }
            }
        }

        /// <summary>
        /// Obtiene el documento cuyo ID corresponde con el pasado.
        /// Cuando se devuelve el objeto documento en su url de descarga figura en enlace
        /// para visualizarlo.
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Documento Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Documento documento = (from u in ctx.Documentos
                                       where u.DocumentoId == id
                                       select u).FirstOrDefault<Documento>();
                    if (documento != null)
                    {
                        string root = System.Web.HttpContext.Current.Server.MapPath("~/downloads");
                        string resultado = PortalProWebUtility.ObtenerUrlDeDocumento(root, tk, documento, ctx);
                        if (resultado == "")
                        {
                            documento = ctx.CreateDetachedCopy<Documento>(documento, x => x.Proveedor, x => x.TipoDocumento);
                            return documento;
                        }else
                        {
                            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, resultado));
                        }
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un documento con el id proporcionado (Documentos)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Documentos)"));
                }
            }
        }

        /// <summary>
        /// Elimina el documento que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del documento a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Documentos)"));
                }
                // primero buscamos si un grupo con ese id existe
                Documento usu = (from u in ctx.Documentos
                               where u.DocumentoId == id
                               select u).FirstOrDefault<Documento>();
                // existe?
                if (usu == null)
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un documento con el id proporcionado (Documentos)"));
                }
                ctx.Delete(usu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}