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
    }
}
