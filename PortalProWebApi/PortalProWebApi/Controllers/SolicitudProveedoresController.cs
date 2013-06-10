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
    public class SolicitudProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos las solicitudes de proveedores de la base de datos
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SolicitudProveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                IEnumerable<SolicitudProveedor> proveedores = (from pr in ctx.SolicitudProveedors
                                                               select pr).ToList<SolicitudProveedor>();

                // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                FetchStrategy fs = new FetchStrategy();
                fs.LoadWith<SolicitudProveedor>(x => x.GrupoProveedor);
                proveedores = ctx.CreateDetachedCopy<IEnumerable<SolicitudProveedor>>(proveedores, fs);
                return proveedores;
            }
        }

        /// <summary>
        /// Obtiene la solicitud cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la solicitud</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Get(int id)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                    SolicitudProveedor proveedor = (from p in ctx.SolicitudProveedors
                                                    where p.SolicitudProveedorId == id
                                                    select p).FirstOrDefault<SolicitudProveedor>();
                    if (proveedor != null)
                    {
                        proveedor = ctx.CreateDetachedCopy<SolicitudProveedor>(proveedor, x => x.GrupoProveedor);
                        return proveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (SolicitudProveedores)"));
                    }
            }
        }

        /// <summary>
        /// Crear una nueva solicitud
        /// </summary>
        /// <param name="SolicitudProveedor">Objeto a crear, el atributo SolicitudProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Post(SolicitudProveedor proveedor)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
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
                var webRoot = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
                var res = PortalProWebUtility.ComprobarCargarFicherosSolicitudProveedor(webRoot, proveedor, ctx);
                if (res != "")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res));
                }
                else
                {
                    ctx.SaveChanges();
                    return ctx.CreateDetachedCopy<SolicitudProveedor>(proveedor, x => x.GrupoProveedor);
                }
            }
        }

        /// <summary>
        /// Modificar una solicitud. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="proveedor">Grupo de proveedor con los valores que se desean en sus atributos</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Put(int id, SolicitudProveedor proveedor)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar los formatos
                if (proveedor == null || id != proveedor.SolicitudProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un proveedor con ese id existe
                SolicitudProveedor pro = (from p in ctx.SolicitudProveedors
                                          where p.SolicitudProveedorId == id
                                          select p).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (pro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (SolicitudProveedores)"));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (proveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = proveedor.GrupoProveedor.GrupoProveedorId;
                    proveedor.GrupoProveedor = null;
                }
                // modificar el objeto
                ctx.AttachCopy<SolicitudProveedor>(proveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                proveedor = (from u in ctx.SolicitudProveedors
                             where u.SolicitudProveedorId == id
                             select u).FirstOrDefault<SolicitudProveedor>();
                if (grupoProveedorId != 0)
                {
                    proveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                where gp.GrupoProveedorId == grupoProveedorId
                                                select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudProveedor>(proveedor, x => x.GrupoProveedor);
            }
        }

        /// <summary>
        /// Elimina la solicitud que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del proveedor a eliminar</param>
        /// <returns></returns>
        public virtual bool Delete(int id)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // primero buscamos si un grupo con ese id existe
                SolicitudProveedor pro = (from u in ctx.SolicitudProveedors
                                          where u.SolicitudProveedorId == id
                                          select u).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (pro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (SolicitudProveedores)"));
                }
                ctx.Delete(pro);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}