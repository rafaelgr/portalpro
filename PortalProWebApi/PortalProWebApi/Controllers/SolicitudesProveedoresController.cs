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
    public class SolicitudesProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos las solicitudes de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<SolicitudProveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<SolicitudProveedor> solProveedores = (from pr in ctx.SolicitudProveedors
                                                                      select pr).ToList<SolicitudProveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<SolicitudProveedor>(x => x.GrupoProveedor);
                    solProveedores = ctx.CreateDetachedCopy<IEnumerable<SolicitudProveedor>>(solProveedores, fs);
                    return solProveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene la solicitud cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador únicode la solicitud</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    SolicitudProveedor solProveedor = (from sp in ctx.SolicitudProveedors
                                                       where sp.SolicitudProveedorId == id
                                                       select sp).FirstOrDefault<SolicitudProveedor>();
                    if (solProveedor != null)
                    {
                        solProveedor = ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor);
                        return solProveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
            }
        }

        /// <summary>
        /// Crear un nueva solicitud
        /// </summary>
        /// <param name="Proveedor">Objeto a crear, el atributo SolicitudProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login). Caso especial "solicitud"</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Post(SolicitudProveedor solProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                // se permite el que lleva el texto "solicitud" siempre
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // comprobar las precondiciones
                if (solProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (solProveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = solProveedor.GrupoProveedor.GrupoProveedorId;
                    solProveedor.GrupoProveedor = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(solProveedor);
                if (grupoProveedorId != 0)
                {
                    solProveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                   where gp.GrupoProveedorId == grupoProveedorId
                                                   select gp).FirstOrDefault<GrupoProveedor>();
                }
                var webRoot = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
                var res = PortalProWebUtility.ComprobarCargarFicherosProveedor(webRoot, tk, solProveedor, ctx);
                if (res != "")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res));
                }
                else
                {
                    ctx.SaveChanges();
                    // preparamos y enviamos el correo de confirmación.
                    PortalProMailController.SendEmail(solProveedor.Email, "[PortalPro] Recibida solicitud", String.Format("Su solicitud con ID:{0} ha sido recibida. No responda este mensaje", solProveedor.SolicitudProveedorId));
                    return ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor);
                }
            }
        }

        /// <summary>
        /// Modificar una solicitud. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la solicitud </param>
        /// <param name="solProveedor">Solicitud de proveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login'). Caso "solicitud"</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Put(int id, SolicitudProveedor solProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                // se permite si el valor es "solicitud" siempre
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // comprobar los formatos
                if (solProveedor == null || id != solProveedor.SolicitudProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si una solicitud con ese id existe
                SolicitudProveedor solPro = (from sp in ctx.SolicitudProveedors
                                             where sp.SolicitudProveedorId == id
                                             select sp).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (solPro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (solProveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = solProveedor.GrupoProveedor.GrupoProveedorId;
                    solProveedor.GrupoProveedor = null;
                }
                // modificar el objeto
                ctx.AttachCopy<SolicitudProveedor>(solProveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                solProveedor = (from sp in ctx.SolicitudProveedors
                                where sp.SolicitudProveedorId == id
                                select sp).FirstOrDefault<SolicitudProveedor>();
                if (grupoProveedorId != 0)
                {
                    solProveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                   where gp.GrupoProveedorId == grupoProveedorId
                                                   select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor);
            }
        }

        /// <summary>
        /// Elimina la solicitud que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificadorde la solicitud a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // primero buscamos si un grupo con ese id existe
                SolicitudProveedor solPro = (from sp in ctx.SolicitudProveedors
                                             where sp.SolicitudProveedorId == id
                                             select sp).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (solPro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                ctx.Delete(solPro);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}