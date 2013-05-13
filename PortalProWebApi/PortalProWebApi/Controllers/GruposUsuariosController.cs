using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class GruposUsuariosController : ApiController
    {
        /// <summary>
        /// Obtiene todos los grupos de usarios de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<GrupoUsuario> Get(string tk)
        {
            
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<GrupoUsuario> gruposUsuarios = (from gu in ctx.GrupoUsuarios
                                                                select gu).ToList<GrupoUsuario>();
                    gruposUsuarios = ctx.CreateDetachedCopy<IEnumerable<GrupoUsuario>>(gruposUsuarios);
                    return gruposUsuarios;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de usuarios)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un grupo de usuarios cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual GrupoUsuario Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    GrupoUsuario grupoUsuario = (from gu in ctx.GrupoUsuarios
                                                 where gu.GrupoUsuarioId == id
                                                 select gu).FirstOrDefault<GrupoUsuario>();
                    if (grupoUsuario != null)
                    {
                        grupoUsuario = ctx.CreateDetachedCopy<GrupoUsuario>(grupoUsuario);
                        return grupoUsuario;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de usuarios)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de usuarios)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo grupo de usuarios
        /// </summary>
        /// <param name="grupoUsuario">Objeto a crear, el atributo GrupoUsuarioId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual GrupoUsuario Post(GrupoUsuario grupoUsuario, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk,ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de usuarios)"));
                }
                // comprobar las precondiciones
                if (grupoUsuario == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(grupoUsuario);
                ctx.SaveChanges();
                return grupoUsuario;
            }
        }

        /// <summary>
        /// Modificar un grupo de usuario. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="grupoUsuario">Grupo de usuario con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual GrupoUsuario Put(int id, GrupoUsuario grupoUsuario, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de usuarios)"));
                }
                // comprobar los formatos
                if (grupoUsuario == null || id != grupoUsuario.GrupoUsuarioId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un grupo con ese id existe
                GrupoUsuario gu = (from g in ctx.GrupoUsuarios
                                   where g.GrupoUsuarioId == id
                                   select g).FirstOrDefault<GrupoUsuario>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de usuarios)"));
                }
                // modificar el objeto
                ctx.AttachCopy<GrupoUsuario>(grupoUsuario);
                ctx.SaveChanges();
                return grupoUsuario;
            }
        }

        /// <summary>
        /// Elimina el grupo de usuario que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del grupo a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual HttpResponseMessage Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Grupo de usuarios)");
                }
                // primero buscamos si un grupo con ese id existe
                GrupoUsuario gu = (from g in ctx.GrupoUsuarios
                                   where g.GrupoUsuarioId == id
                                   select g).FirstOrDefault<GrupoUsuario>();
                // existe?
                if (gu == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Grupo de usuarios)");
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
