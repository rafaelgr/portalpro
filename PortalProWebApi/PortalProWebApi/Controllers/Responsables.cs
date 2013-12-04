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
    public class ResponsablesController : ApiController
    {
        /// <summary>
        /// Obtiene todos los Responsables de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Responsable> Get(string tk)
        {
            
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk=="solicitud")
                {
                    IEnumerable<Responsable> responsables = (from gu in ctx.Responsables
                                                                select gu).ToList<Responsable>();
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Responsable>(x => x.Usuario);
                    responsables = ctx.CreateDetachedCopy<IEnumerable<Responsable>>(responsables,fs);
                    return responsables;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Responsables)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un Responsables cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la Responsable</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Responsable Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Responsable responsable = (from gu in ctx.Responsables
                                                 where gu.ResponsableId == id
                                                 select gu).FirstOrDefault<Responsable>();
                    if (responsable != null)
                    {
                        responsable = ctx.CreateDetachedCopy<Responsable>(responsable, x => x.Usuario);
                        return responsable;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Responsables)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Responsables)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo Responsables
        /// </summary>
        /// <param name="responsable">Objeto a crear, el atributo ResponsableId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Responsable Post(Responsable responsable, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk,ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Responsables)"));
                }
                // comprobar las precondiciones
                if (responsable == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int usuarioId = 0;
                if (responsable.Usuario != null)
                {
                    usuarioId = responsable.Usuario.UsuarioId;
                    responsable.Usuario = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(responsable);
                if (usuarioId != 0)
                {
                    responsable.Usuario = (from u in ctx.Usuarios
                                           where u.UsuarioId == usuarioId
                                           select u).FirstOrDefault<Usuario>();
                }
                ctx.SaveChanges();
                return responsable;
            }
        }

        /// <summary>
        /// Modificar una Responsable. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la Responsable </param>
        /// <param name="responsable">Grupo de usuario con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Responsable Put(int id, Responsable responsable, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Responsables)"));
                }
                // comprobar los formatos
                if (responsable == null || id != responsable.ResponsableId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un grupo con ese id existe
                Responsable re = (from g in ctx.Responsables
                                   where g.ResponsableId == id
                                   select g).FirstOrDefault<Responsable>();
                // existe?
                if (re == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Responsables)"));
                }
                int usuarioId = 0;
                if (responsable.Usuario != null)
                {
                    usuarioId = responsable.Usuario.UsuarioId;
                    responsable.Usuario = null;
                }
                // hay que volver a leer el objeto para que lo maneje este contexto.
                responsable = (from r in ctx.Responsables
                               where r.ResponsableId == id
                               select r).FirstOrDefault<Responsable>();
                // modificar el objeto
                ctx.AttachCopy<Responsable>(responsable);
                if (usuarioId != 0)
                {
                    responsable.Usuario = (from u in ctx.Usuarios
                                           where u.UsuarioId == usuarioId
                                           select u).FirstOrDefault<Usuario>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Responsable>(responsable, x => x.Usuario);
            }
        }

        /// <summary>
        /// Elimina la Responsable que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la Responsable a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Responsables)"));
                }
                // primero buscamos si un grupo con ese id existe
                Responsable gu = (from g in ctx.Responsables
                                   where g.ResponsableId == id
                                   select g).FirstOrDefault<Responsable>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Responsables)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
