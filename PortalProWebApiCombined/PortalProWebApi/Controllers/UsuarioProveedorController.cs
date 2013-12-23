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
    public class UsuarioProveedorController : ApiController
    {
        /// <summary>
        /// Obtiene todos los usuarios ligados a proveedores de la base de  datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<UsuarioProveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<UsuarioProveedor> UsuarioProveedor = (from up in ctx.UsuarioProveedors
                                                                      select up).ToList<UsuarioProveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<UsuarioProveedor>(x => x.Proveedor);
                    UsuarioProveedor = ctx.CreateDetachedCopy<IEnumerable<UsuarioProveedor>>(UsuarioProveedor, fs);
                    return UsuarioProveedor;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (UsuarioProveedor)"));
                }
            }
        }

        /// <summary>
        /// Obtiene el usuario de proveedor cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual UsuarioProveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    UsuarioProveedor usuarioProveedor = (from up in ctx.UsuarioProveedors
                                       where up.UsuarioProveedorId == id
                                       select up).FirstOrDefault<UsuarioProveedor>();
                    if (usuarioProveedor != null)
                    {
                        usuarioProveedor = ctx.CreateDetachedCopy<UsuarioProveedor>(usuarioProveedor, x => x.Proveedor);
                        return usuarioProveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un usuario con el id proporcionado (UsuarioProveedor)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (UsuarioProveedor)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo usuario de proveedor
        /// </summary>
        /// <param name="Usuario">Objeto a crear, el atributo UsuarioPorveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual UsuarioProveedor Post(UsuarioProveedor usuarioProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (UsuarioProveedor)"));
                }
                // comprobar las precondiciones
                if (usuarioProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // controlar la contraseña.
                if (usuarioProveedor.Password != null && usuarioProveedor.Password != "")
                {
                    // se guarda la contraseña encriptada
                    usuarioProveedor.Password = CntWebApiSeguridad.GetHashCode(usuarioProveedor.Password);
                }
                int proveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (usuarioProveedor.Proveedor != null)
                {
                    proveedorId = usuarioProveedor.Proveedor.ProveedorId;
                    usuarioProveedor.Proveedor = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(usuarioProveedor);
                if (proveedorId != 0)
                {
                    usuarioProveedor.Proveedor = (from p in ctx.Proveedors
                                            where p.ProveedorId == proveedorId
                                            select p).FirstOrDefault<Proveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<UsuarioProveedor>(usuarioProveedor, x => x.Proveedor);
            }
        }

        /// <summary>
        /// Modificar un usuario ligado a un proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del usuario </param>
        /// <param name="usuarioProveedor">Datos del usario ligado a proveedor</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual UsuarioProveedor Put(int id, UsuarioProveedor usuarioProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (UsuarioProveedor)"));
                }
                // comprobar los formatos
                if (usuarioProveedor == null || id != usuarioProveedor.UsuarioProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un usuario con ese id existe
                UsuarioProveedor usup = (from up in ctx.UsuarioProveedors
                               where up.UsuarioProveedorId == id
                               select up).FirstOrDefault<UsuarioProveedor>();
                // existe?
                if (usup == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un usuario con el id proporcionado (UsuarioProveedor)"));
                }
                // controlar la contraseña.
                if (usuarioProveedor.Password != null && usuarioProveedor.Password != "" && usuarioProveedor.Password != usup.Password)
                {
                    // se guarda la contraseña encriptada
                    usuarioProveedor.Password = CntWebApiSeguridad.GetHashCode(usuarioProveedor.Password);
                }
                int proveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (usuarioProveedor.Proveedor != null)
                {
                    proveedorId = usuarioProveedor.Proveedor.ProveedorId;
                    usuarioProveedor.Proveedor = null;
                }
                // modificar el objeto
                ctx.AttachCopy<UsuarioProveedor>(usuarioProveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                usuarioProveedor = (from up in ctx.UsuarioProveedors
                           where up.UsuarioProveedorId == id
                           select up).FirstOrDefault<UsuarioProveedor>();
                if (proveedorId != 0)
                {
                    usuarioProveedor.Proveedor = (from p in ctx.Proveedors
                                            where p.ProveedorId == proveedorId
                                            select p).FirstOrDefault<Proveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<UsuarioProveedor>(usuarioProveedor, x => x.Proveedor );
            }
        }

        /// <summary>
        /// Elimina el usuario que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del usuario a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (UsuarioProveedor)"));
                }
                // primero buscamos si un grupo con ese id existe
                UsuarioProveedor usu = (from u in ctx.UsuarioProveedors
                               where u.UsuarioProveedorId == id
                               select u).FirstOrDefault<UsuarioProveedor>();
                // existe?
                if (usu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un usuario con el id proporcionado (UsuarioProveedor)"));
                }
                ctx.Delete(usu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}