using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// Tras comprobar que un usuario con el login y password dados 
        /// existe, crea un tique que devuelve en el cuerpo del mensaje
        /// </summary>
        /// <param name="login">Login del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Un objeto que representa un tique</returns>
        /// <remarks>Este es un comentario adicional</remarks>
        public virtual WebApiTicket GetLogin(string login, string password)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                WebApiTicket tk = CntWebApiSeguridad.Login(login, password, 30, ctx);
                if (tk == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Nombre de usuario o contraseña incorrecto"));
                }
                else
                {
                    // agregamos el tique recién creado
                    ctx.Add(tk);
                    ctx.SaveChanges();
                    tk = ctx.CreateDetachedCopy<WebApiTicket>(tk, x => x.Usuario);
                    return tk;
                }
            }
        }
    }
}
