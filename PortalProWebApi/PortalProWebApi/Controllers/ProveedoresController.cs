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
    public class ProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos los proveedores de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Proveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Proveedor> proveedores = (from pr in ctx.Proveedors
                                                     select pr).ToList<Proveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Proveedor>(x => x.GrupoProveedor);
                    proveedores = ctx.CreateDetachedCopy<IEnumerable<Proveedor>>(proveedores, fs);
                    return proveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene el proveedor cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del proveedor</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Proveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Proveedor proveedor = (from p in ctx.Proveedors
                                       where p.ProveedorId == id
                                       select p).FirstOrDefault<Proveedor>();
                    if (proveedor != null)
                    {
                        proveedor = ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
                        return proveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo proveedor
        /// </summary>
        /// <param name="Proveedor">Objeto a crear, el atributo ProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Proveedor Post(Proveedor proveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // comprobar las precondiciones
                if (proveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (proveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = proveedor.GrupoProveedor.GrupoProveedorId;
                    proveedor.GrupoProveedor = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(proveedor);
                if (grupoProveedorId != 0)
                {
                    proveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                            where gp.GrupoProveedorId == grupoProveedorId
                                            select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
            }
        }

        /// <summary>
        /// Modificar un proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="proveedor">Grupo de proveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Proveedor Put(int id, Proveedor proveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // comprobar los formatos
                if (proveedor == null || id != proveedor.ProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un proveedor con ese id existe
                Proveedor pro = (from p in ctx.Proveedors
                               where p.ProveedorId == id
                               select p).FirstOrDefault<Proveedor>();
                // existe?
                if (pro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (proveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = proveedor.GrupoProveedor.GrupoProveedorId;
                    proveedor.GrupoProveedor = null;
                }
                // modificar el objeto
                ctx.AttachCopy<Proveedor>(proveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                proveedor = (from u in ctx.Proveedors
                           where u.ProveedorId == id
                           select u).FirstOrDefault<Proveedor>();
                if (grupoProveedorId != 0)
                {
                    proveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                            where gp.GrupoProveedorId == grupoProveedorId
                                            select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
            }
        }

        /// <summary>
        /// Elimina el proveedor que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del proveedor a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // primero buscamos si un grupo con ese id existe
                Proveedor pro = (from u in ctx.Proveedors
                               where u.ProveedorId == id
                               select u).FirstOrDefault<Proveedor>();
                // existe?
                if (pro == null)
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Proveedores)"));
                }
                ctx.Delete(pro);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}