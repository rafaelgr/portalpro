using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class GruposProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos los grupos de proveedores de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<GrupoProveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<GrupoProveedor> gruposProveedores = (from gp in ctx.GrupoProveedors
                                                                     select gp).ToList<GrupoProveedor>();
                    gruposProveedores = ctx.CreateDetachedCopy<IEnumerable<GrupoProveedor>>(gruposProveedores);
                    return gruposProveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un grupo de proveedores cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login"), admite "solicitud"</param>
        /// <returns></returns>
        public virtual GrupoProveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    GrupoProveedor grupoProveedor = (from gp in ctx.GrupoProveedors
                                                     where gp.GrupoProveedorId == id
                                                     select gp).FirstOrDefault<GrupoProveedor>();
                    if (grupoProveedor != null)
                    {
                        grupoProveedor = ctx.CreateDetachedCopy<GrupoProveedor>(grupoProveedor);
                        return grupoProveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un grupo de proveedores cuyo ID corresponde con el pasado
        /// en este caso no necesita tique porque es para solicitudes.
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <returns></returns>
        public virtual GrupoProveedor Get(int id)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                GrupoProveedor grupoProveedor = (from gp in ctx.GrupoProveedors
                                                 where gp.GrupoProveedorId == id
                                                 select gp).FirstOrDefault<GrupoProveedor>();
                if (grupoProveedor != null)
                {
                    grupoProveedor = ctx.CreateDetachedCopy<GrupoProveedor>(grupoProveedor);
                    return grupoProveedor;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de proveedores)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo grupo de proveedores
        /// </summary>
        /// <param name="grupoProveedor">Objeto a crear, el atributo GrupoProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual GrupoProveedor Post(GrupoProveedor grupoProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de proveedores)"));
                }
                // comprobar las precondiciones
                if (grupoProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(grupoProveedor);
                ctx.SaveChanges();
                return grupoProveedor;
            }
        }

        /// <summary>
        /// Modificar un grupo de proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="grupoProveedor">Grupo de poveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual GrupoProveedor Put(int id, GrupoProveedor grupoProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de proveedores)"));
                }
                // comprobar los formatos
                if (grupoProveedor == null || id != grupoProveedor.GrupoProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un grupo con ese id existe
                GrupoProveedor gp = (from g in ctx.GrupoProveedors
                                     where g.GrupoProveedorId == id
                                     select g).FirstOrDefault<GrupoProveedor>();
                // existe?
                if (gp == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de proveedores)"));
                }
                // modificar el objeto
                ctx.AttachCopy<GrupoProveedor>(grupoProveedor);
                ctx.SaveChanges();
                return grupoProveedor;
            }
        }

        /// <summary>
        /// Elimina el grupo de proveedor que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del grupo a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de proveedores)"));
                }
                // primero buscamos si un grupo con ese id existe
                GrupoProveedor gu = (from g in ctx.GrupoProveedors
                                     where g.GrupoProveedorId == id
                                     select g).FirstOrDefault<GrupoProveedor>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de proveedores)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}