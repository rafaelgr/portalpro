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
    public class SolicitudesLogController : ApiController
    {
        /// <summary>
        /// Obtiene todos los log de solicitudes de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<SolicitudLog> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<SolicitudLog> solog = (from gu in ctx.SolicitudLogs
                                                       select gu).ToList<SolicitudLog>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<SolicitudLog>(x => x.Usuario);
                    fs.LoadWith<SolicitudLog>(x => x.SolicitudProveedor);
                    fs.LoadWith<SolicitudLog>(x => x.SolicitudStatusInicial);
                    fs.LoadWith<SolicitudLog>(x => x.SolicitudStatusFinal);
                    solog = ctx.CreateDetachedCopy<IEnumerable<SolicitudLog>>(solog, fs);
                    return solog;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
            }
        }

        /// <summary>
        /// Obtiene log de solicitud que corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del log</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual SolicitudLog Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    SolicitudLog solog = (from u in ctx.SolicitudLogs
                                          where u.SolicitudLogId == id
                                          select u).FirstOrDefault<SolicitudLog>();
                    if (solog != null)
                    {
                        solog = ctx.CreateDetachedCopy<SolicitudLog>(solog, x => x.Usuario, x => x.SolicitudProveedor, x => x.SolicitudStatusInicial, x => x.SolicitudStatusFinal);
                        return solog;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un log de solcitud con el id proporcionado (SolicitudLogs)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo solog
        /// </summary>
        /// <param name="SolicitudLog">Objeto a crear, el atributo SolicitudLogId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual SolicitudLog Post(SolicitudLog solog, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
                // comprobar las precondiciones
                if (solog == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int usuarioId = 0;
                int solicitudProveedorId = 0;
                int solStatusFinId = 0;
                int solStatusInicioId = 0;

                // Controlamos las propiedades que son en realidad objetos.
                if (solog.Usuario != null)
                {
                    usuarioId = solog.Usuario.UsuarioId;
                    solog.Usuario = null;
                }
                if (solog.SolicitudProveedor != null)
                {
                    solicitudProveedorId = solog.SolicitudProveedor.SolicitudProveedorId;
                    solog.SolicitudProveedor = null;
                }
                if (solog.SolicitudStatusInicial != null)
                {
                    solStatusInicioId = solog.SolicitudStatusInicial.SolicitudStatusId;
                    solog.SolicitudStatusInicial = null;
                }
                if (solog.SolicitudStatusFinal != null)
                {
                    solStatusFinId = solog.SolicitudStatusFinal.SolicitudStatusId;
                    solog.SolicitudStatusFinal = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(solog);
                if (usuarioId != 0)
                {
                    solog.Usuario = (from u in ctx.Usuarios
                                     where u.UsuarioId == usuarioId
                                     select u).FirstOrDefault<Usuario>();
                }
                if (solicitudProveedorId != 0)
                {
                    solog.SolicitudProveedor = (from s in ctx.SolicitudProveedors
                                                select s).FirstOrDefault<SolicitudProveedor>();
                }
                if (solStatusInicioId != 0)
                {
                    solog.SolicitudStatusInicial = (from s in ctx.SolicitudStatus
                                                    select s).FirstOrDefault<SolicitudStatus>();
                }
                if (solStatusFinId != 0)
                {
                    solog.SolicitudStatusFinal = (from s in ctx.SolicitudStatus
                                                  select s).FirstOrDefault<SolicitudStatus>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudLog>(solog, x => x.Usuario, x => x.SolicitudProveedor, x => x.SolicitudStatusInicial, x => x.SolicitudStatusFinal);
            }
        }

        /// <summary>
        /// Modificar un log de solicitud. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del log </param>
        /// <param name="solog">Log de solicitud con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual SolicitudLog Put(int id, SolicitudLog solog, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
                // comprobar los formatos
                if (solog == null || id != solog.SolicitudLogId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un solog con ese id existe
                SolicitudLog slg = (from u in ctx.SolicitudLogs
                                    where u.SolicitudLogId == id
                                    select u).FirstOrDefault<SolicitudLog>();
                // existe?
                if (slg == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un solog con el id proporcionado (SolicitudLogs)"));
                }
                //
                int usuarioId = 0;
                int solicitudProveedorId = 0;
                int solStatusFinId = 0;
                int solStatusInicioId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (solog.Usuario != null)
                {
                    usuarioId = solog.Usuario.UsuarioId;
                    solog.Usuario = null;
                }
                if (solog.SolicitudProveedor != null)
                {
                    solicitudProveedorId = solog.SolicitudProveedor.SolicitudProveedorId;
                    solog.SolicitudProveedor = null;
                }
                if (solog.SolicitudStatusInicial != null)
                {
                    solStatusInicioId = solog.SolicitudStatusInicial.SolicitudStatusId;
                    solog.SolicitudStatusInicial = null;
                }
                if (solog.SolicitudStatusFinal != null)
                {
                    solStatusFinId = solog.SolicitudStatusFinal.SolicitudStatusId;
                    solog.SolicitudStatusFinal = null;
                }
                // modificar el objeto
                ctx.AttachCopy<SolicitudLog>(solog);
                // volvemos a leer el objecto para que lo maneje este contexto.
                solog = (from s in ctx.SolicitudLogs
                         where s.SolicitudLogId == id
                         select s).FirstOrDefault<SolicitudLog>();
                if (usuarioId != 0)
                {
                    solog.Usuario = (from u in ctx.Usuarios
                                     where u.UsuarioId == usuarioId
                                     select u).FirstOrDefault<Usuario>();
                }
                if (solicitudProveedorId != 0)
                {
                    solog.SolicitudProveedor = (from s in ctx.SolicitudProveedors
                                                select s).FirstOrDefault<SolicitudProveedor>();
                }
                if (solStatusInicioId != 0)
                {
                    solog.SolicitudStatusInicial = (from s in ctx.SolicitudStatus
                                                    select s).FirstOrDefault<SolicitudStatus>();
                }
                if (solStatusFinId != 0)
                {
                    solog.SolicitudStatusFinal = (from s in ctx.SolicitudStatus
                                                  select s).FirstOrDefault<SolicitudStatus>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudLog>(solog, x => x.Usuario, x => x.SolicitudProveedor, x => x.SolicitudStatusInicial, x => x.SolicitudStatusFinal);
            }
        }

        /// <summary>
        /// Elimina el log que coincide con el id pasado. La solicitud a la que pertenece
        /// pasa al estado de pendiente.
        /// </summary>
        /// <param name="id">Identificador del log a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
                // primero buscamos si un grupo con ese id existe
                SolicitudLog slg = (from s in ctx.SolicitudLogs
                                    where s.SolicitudLogId == id
                                    select s).FirstOrDefault<SolicitudLog>();
                // existe?
                if (slg == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un log con el id proporcionado (SolicitudLogs)"));
                }
                // salvamos la solicitud que habrá que actualizar
                SolicitudProveedor sp = slg.SolicitudProveedor;
                if (sp != null)
                {
                    // Estado 1 = Pendiente
                    sp.SolicitudStatus = (from s in ctx.SolicitudStatus
                                          where s.SolicitudStatusId == 1
                                          select s).FirstOrDefault<SolicitudStatus>();
                }
                ctx.Delete(slg);
                
                ctx.SaveChanges();
                return true;
            }
        }
    }
}