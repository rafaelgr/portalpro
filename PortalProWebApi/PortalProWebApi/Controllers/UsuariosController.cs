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
    public class UsuariosController : ApiController
    {
        /// <summary>
        /// Obtiene todos los usuarios de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Usuario> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Usuario> usuarios = (from gu in ctx.Usuarios
                                                     select gu).ToList<Usuario>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Usuario>(x => x.GrupoUsuario);
                    usuarios = ctx.CreateDetachedCopy<IEnumerable<Usuario>>(usuarios, fs);
                    return usuarios;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Usuarios)"));
                }
            }
        }

        /// <summary>
        /// Obtiene el usuario cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del grupo</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Usuario Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Usuario usuario = (from u in ctx.Usuarios
                                       where u.UsuarioId == id
                                       select u).FirstOrDefault<Usuario>();
                    if (usuario != null)
                    {
                        usuario = ctx.CreateDetachedCopy<Usuario>(usuario, x => x.GrupoUsuario);
                        return usuario;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un usuario con el id proporcionado (Usuarios)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Usuarios)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        /// <param name="Usuario">Objeto a crear, el atributo UsuarioId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Usuario Post(Usuario usuario, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Usuarios)"));
                }
                // comprobar las precondiciones
                if (usuario == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // Controlamos las propiedades que son en realidad objetos.
                if (usuario.GrupoUsuario != null)
                {
                    usuario.GrupoUsuario = (from g in ctx.GrupoUsuarios
                                            where g.GrupoUsuarioId == usuario.GrupoUsuario.GrupoUsuarioId
                                            select g).FirstOrDefault<GrupoUsuario>();
                }
                // controlar la contraseña.
                if (usuario.Password != null && usuario.Password != "")
                {
                    // se guarda la contraseña encriptada
                    usuario.Password = CntWebApiSeguridad.GetHashCode(usuario.Password);
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(usuario);
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Usuario>(usuario, x => x.GrupoUsuario);
            }
        }

        /// <summary>
        /// Modificar un usuario. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="usuario">Grupo de usuario con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Usuario Put(int id, Usuario usuario, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Usuarios)"));
                }
                // comprobar los formatos
                if (usuario == null || id != usuario.UsuarioId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un usuario con ese id existe
                Usuario usu = (from u in ctx.Usuarios
                               where u.UsuarioId == id
                               select u).FirstOrDefault<Usuario>();
                // existe?
                if (usu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un usuario con el id proporcionado (Usuarios)"));
                }
                // Controlamos las propiedades que son en realidad objetos.
                if (usuario.GrupoUsuario != null)
                {
                    usuario.GrupoUsuario = (from g in ctx.GrupoUsuarios
                                            where g.GrupoUsuarioId == usuario.GrupoUsuario.GrupoUsuarioId
                                            select g).FirstOrDefault<GrupoUsuario>();
                }
                // controlar la contraseña.
                if (usuario.Password != null && usuario.Password != "")
                {
                    // se guarda la contraseña encriptada
                    usuario.Password = CntWebApiSeguridad.GetHashCode(usuario.Password);
                }
                // modificar el objeto
                ctx.AttachCopy<Usuario>(usuario);
                ctx.SaveChanges();
                return usuario;
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
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Usuarios)"));
                }
                // primero buscamos si un grupo con ese id existe
                Usuario usu = (from u in ctx.Usuarios
                               where u.UsuarioId == id
                               select u).FirstOrDefault<Usuario>();
                // existe?
                if (usu == null)
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Usuarios)"));
                }
                ctx.Delete(usu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}