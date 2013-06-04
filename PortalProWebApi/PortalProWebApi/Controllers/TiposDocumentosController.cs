using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class TiposDocumentosController : ApiController
    {
        /// <summary>
        /// Obtiene todos los tipos de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<TipoDocumento> Get(string tk)
        {
            
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<TipoDocumento> tiposDocumentos = (from gu in ctx.TipoDocumentos
                                                                select gu).ToList<TipoDocumento>();
                    tiposDocumentos = ctx.CreateDetachedCopy<IEnumerable<TipoDocumento>>(tiposDocumentos);
                    return tiposDocumentos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
            }
        }

        /// <summary>
        /// Devuelve la lista de los tipos de documentos asociados al grupo de usuarios cuyo id correspone a la cadena pasada
        /// </summary>
        /// <param name="tk">Tique obtienido en el login</param>
        /// <param name="grupoCode">Identificador de grupo de proveedor pasado como cadena</param>
        /// <returns></returns>
        public virtual IList<TipoDocumento> Get(string tk, string grupoCode)
        {

            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    int id = 0;
                    bool res = int.TryParse(grupoCode, out id);
                    GrupoProveedor grupoProveedor = (from gp in ctx.GrupoProveedors
                                                     where gp.GrupoProveedorId == id
                                                     select gp).FirstOrDefault<GrupoProveedor>();
                    if (grupoProveedor == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Debe proporcionar un grupo de proveedores existente"));
                    }
                    IList<TipoDocumento> tiposDocumentos = new List<TipoDocumento>();
                    // cargamos las asociaciones
                    var rs = (from tdgp in ctx.TipoDocumentoGrupoProveedors
                              where tdgp.GrupoProveedor.GrupoProveedorId == grupoProveedor.GrupoProveedorId
                              select tdgp);
                    foreach (TipoDocumentoGrupoProveedor t in rs)
                    {
                        tiposDocumentos.Add(t.TipoDocumento);
                    }
                    tiposDocumentos = ctx.CreateDetachedCopy<IList<TipoDocumento>>(tiposDocumentos);
                    return tiposDocumentos;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un tipo de documento cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del tipo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual TipoDocumento Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    TipoDocumento tipoDocumento = (from td in ctx.TipoDocumentos
                                                 where td.TipoDocumentoId == id
                                                 select td).FirstOrDefault<TipoDocumento>();
                    if (tipoDocumento != null)
                    {
                        tipoDocumento = ctx.CreateDetachedCopy<TipoDocumento>(tipoDocumento);
                        return tipoDocumento;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un tipo con el id proporcionado (Tipos de documentos)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo tipo de documento
        /// </summary>
        /// <param name="tipoDocumento">Objeto a crear, el atributo TipoDocumentoId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual TipoDocumento Post(TipoDocumento tipoDocumento, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk,ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
                // comprobar las precondiciones
                if (tipoDocumento == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(tipoDocumento);
                ctx.SaveChanges();
                return tipoDocumento;
            }
        }

        public virtual bool Post(IList<int> tipos, string tk, string grupoCode)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    int id = 0;
                    bool res = int.TryParse(grupoCode, out id);
                    GrupoProveedor grupoProveedor = (from gp in ctx.GrupoProveedors
                                                     where gp.GrupoProveedorId == id
                                                     select gp).FirstOrDefault<GrupoProveedor>();
                    if (grupoProveedor == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Debe proporcionar un grupo de proveedores existente"));
                    }
                    // eliminamos todos os asociados posibles, que serán sustituidos por los nuevos
                    ctx.Delete(grupoProveedor.TipoDocumentoGrupoProveedors);
                    // vamos a comprobar todos los id pasados
                    foreach (int i in tipos)
                    {
                        // ya está en la lista de este grupo
                        TipoDocumentoGrupoProveedor tdgp = new TipoDocumentoGrupoProveedor();
                        tdgp.GrupoProveedor = grupoProveedor;
                        TipoDocumento td = (from t in ctx.TipoDocumentos
                                            where t.TipoDocumentoId == i
                                            select t).FirstOrDefault<TipoDocumento>();
                        if (td != null)
                        {
                            tdgp.TipoDocumento = td;
                            ctx.Add(tdgp);
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            return true;
        }

        /// <summary>
        /// Modificar un tipo de documento. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del tipo </param>
        /// <param name="tipoDocumento">Tipo de documento con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual TipoDocumento Put(int id, TipoDocumento tipoDocumento, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
                // comprobar los formatos
                if (tipoDocumento == null || id != tipoDocumento.TipoDocumentoId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un grupo con ese id existe
                TipoDocumento gu = (from g in ctx.TipoDocumentos
                                   where g.TipoDocumentoId == id
                                   select g).FirstOrDefault<TipoDocumento>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un tipo con el id proporcionado (Tipos de documentos)"));
                }
                // modificar el objeto
                ctx.AttachCopy<TipoDocumento>(tipoDocumento);
                ctx.SaveChanges();
                return tipoDocumento;
            }
        }

        /// <summary>
        /// Elimina el tipo de docuemnto que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del tipo a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Tipos de documentos)"));
                }
                // primero buscamos si un grupo con ese id existe
                TipoDocumento gu = (from g in ctx.TipoDocumentos
                                   where g.TipoDocumentoId == id
                                   select g).FirstOrDefault<TipoDocumento>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Tipos de documentos)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
