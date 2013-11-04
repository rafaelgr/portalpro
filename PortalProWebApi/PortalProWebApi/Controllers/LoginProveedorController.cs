using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class LoginProveedorController : ApiController
    {
        /// <summary>
        /// (usuarios de proveedor)
        /// Tras comprobar que un usuario con el login y password dados 
        /// existe, crea un tique que devuelve en el cuerpo del mensaje
        /// </summary>
        /// <param name="login">Login del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Un objeto que representa un tique</returns>
        /// <remarks>Este es un comentario adicional</remarks>
        public virtual WebApiTicket GetLoginProveedor(string login, string password)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                WebApiTicket tk = CntWebApiSeguridad.LoginProveedor(login, password, 30, ctx);
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

        /// <summary>
        /// Renueva el tique por 30 minutos adicionales
        /// </summary>
        /// <param name="tk">Tique que se desea renovar</param>
        /// <returns></returns>
        public virtual WebApiTicket PutLoginProveedor(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprueba el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LoginProveedor)"));
                }
                // renueva el tique por 30 minutos más
                // en el futuro todo esto debería ser parametrizable
                WebApiTicket wtck = (from t in ctx.WebApiTickets
                                     where t.Codigo == tk
                                     select t).FirstOrDefault<WebApiTicket>();
                // dado que ha habido una comprobación previa del tique éste 
                // debería existir.
                wtck.Fin = DateTime.Now.AddMinutes(30);
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<WebApiTicket>(wtck, x => x.Usuario);
            }
        }
    }
}
