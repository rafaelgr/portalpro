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
                    fs.LoadWith<SolicitudProveedor>(x => x.SolicitudStatus);
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
                        solProveedor = ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor, x=>x.SolicitudStatus);
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
                int solicitudStatusId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (solProveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = solProveedor.GrupoProveedor.GrupoProveedorId;
                    solProveedor.GrupoProveedor = null;
                }
                if (solProveedor.SolicitudStatus != null)
                {
                    solicitudStatusId = solProveedor.SolicitudStatus.SolicitudStatusId;
                    solProveedor.SolicitudStatus = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(solProveedor);
                if (grupoProveedorId != 0)
                {
                    solProveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                   where gp.GrupoProveedorId == grupoProveedorId
                                                   select gp).FirstOrDefault<GrupoProveedor>();
                }
                if (solicitudStatusId != 0)
                {
                    solProveedor.SolicitudStatus = (from ss in ctx.SolicitudStatus
                                                    where ss.SolicitudStatusId == solicitudStatusId
                                                    select ss).FirstOrDefault<SolicitudStatus>();
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
                    // preparamos y enviamos el correo de confirmación por defecto (por si falla la plantilla).
                    string asunto = "[PortalPro] Recibida solicitud";
                    string cuerpo = String.Format("Su solicitud con ID:{0} ha sido recibida. No responda este mensaje", solProveedor.SolicitudProveedorId);
                    // El primer paso es obtener la plantilla ID=1
                    Plantilla plantilla = (from pl in ctx.Plantillas
                                           where pl.PlantillaId == 1
                                           select pl).FirstOrDefault<Plantilla>();
                    if (plantilla != null)
                    {
                        asunto = String.Format(plantilla.Asunto, solProveedor.SolicitudProveedorId, solProveedor.Nombre, solProveedor.Calle, solProveedor.Ciudad,
                            solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                            solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif);
                        cuerpo = String.Format(plantilla.Cuerpo, solProveedor.SolicitudProveedorId, solProveedor.Nombre, solProveedor.Calle, solProveedor.Ciudad,
                            solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                            solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif);
                    }
                    PortalProMailController.SendEmail(solProveedor.Email, asunto, cuerpo);
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
                int solicitudStatusId = 0;
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
                if (solicitudStatusId != 0)
                {
                    solProveedor.SolicitudStatus = (from ss in ctx.SolicitudStatus
                                                    where ss.SolicitudStatusId == solicitudStatusId
                                                    select ss).FirstOrDefault<SolicitudStatus>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor, x=>x.SolicitudStatus);
            }
        }

        /// <summary>
        /// Cambia el estado de una solicitud y realiza la grabación 
        /// correspondiente en el log
        /// </summary>
        /// <param name="id">Identificador de la solicitud a procesar</param>
        /// <param name="tk">Tique de autorización (ver Login)</param>
        /// <param name="status">Código del estado al que se quiere pasar (2=Aceptada / 3=Rechazada)</param>
        /// <param name="userId">Identificador del usuario que avala el cambio</param>
        /// <param name="comentarios">Comentarios adicionales</param>
        /// <returns></returns>
        public virtual bool PutStatus(int id, string tk, int status, int userId, string comentarios)
        {
            bool res = false;
            using (PortalProContext ctx = new PortalProContext())
            {
                // Comprobamos la solicitud
                SolicitudProveedor solicitudProveedor = (from sp in ctx.SolicitudProveedors
                                                         where sp.SolicitudProveedorId == id
                                                         select sp).FirstOrDefault<SolicitudProveedor>();
                if (solicitudProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                if (solicitudProveedor.SolicitudStatus == null || solicitudProveedor.SolicitudStatus.SolicitudStatusId != 1)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "La solicitud ya ha sido procesada"));
                }
                // ya nos hemos asegurado que la soilictud existe ahora creamos el registro de procesamiento.
                SolicitudStatus st = (from s in ctx.SolicitudStatus
                                      where s.SolicitudStatusId == status
                                      select s).FirstOrDefault<SolicitudStatus>();
                if (st == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una estatus con el id proporcionado (Solicitudes proveedores)"));
                }
                // obtención de los usuarios
                Usuario usu = (from u in ctx.Usuarios
                               where u.UsuarioId == userId
                               select u).FirstOrDefault<Usuario>();
                if (usu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "El usuario es incorrecto (Solicitudes proveedores)"));
                }
                // y ahora el regsitro de procesamiento
                SolicitudLog slg = new SolicitudLog();
                slg.Sello = DateTime.Now;
                slg.Comentarios = comentarios;
                slg.Usuario = usu;
                slg.SolicitudStatusInicial = solicitudProveedor.SolicitudStatus;
                slg.SolicitudStatusFinal = st;
                ctx.Add(slg);
                // cambiamos el estado de la solicitud
                solicitudProveedor.SolicitudStatus = st;
                // y salvamos todo
                ctx.SaveChanges();
            }
            return res;
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