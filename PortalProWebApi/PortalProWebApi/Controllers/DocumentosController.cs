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
        /// Obtiene todos los documentos de la base de datos
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
        /// Obtiene el documento cuyo ID corresponde con el pasado
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
                        documento = ctx.CreateDetachedCopy<Documento>(documento, x => x.Proveedor, x => x.TipoDocumento);
                        return documento;
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