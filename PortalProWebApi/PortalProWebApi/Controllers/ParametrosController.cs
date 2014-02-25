using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class ParametrosController : ApiController
    {
        /// <summary>
        /// Obtiene todos las Parametros de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Parametro> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Parametro> parametros = (from pl in ctx.Parametros1
                                                                     select pl).ToList<Parametro>();
                    parametros = ctx.CreateDetachedCopy<IEnumerable<Parametro>>(parametros);
                    return parametros;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Parametros)"));
                }
            }
        }

        /// <summary>
        /// Obtiene una Parametro cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la Parametro</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Parametro Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    Parametro Parametro = (from pl in ctx.Parametros1
                                                     where pl.ParametroId == id
                                                     select pl).FirstOrDefault<Parametro>();
                    if (Parametro != null)
                    {
                        Parametro = ctx.CreateDetachedCopy<Parametro>(Parametro);
                        return Parametro;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un Parametro con el id proporcionado (Parametros)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Parametros)"));
                }
            }
        }

        /// <summary>
        /// Obtiene una Parametro cuyo ID corresponde con el pasado
        /// en este caso no necesita tique porque es para solicitudes.
        /// </summary>
        /// <param name="id">Identificador único de la Parametro</param>
        /// <returns></returns>
        public virtual Parametro Get(int id)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                Parametro Parametro = (from pl in ctx.Parametros1
                                                 where pl.ParametroId == id
                                                 select pl).FirstOrDefault<Parametro>();
                if (Parametro != null)
                {
                    Parametro = ctx.CreateDetachedCopy<Parametro>(Parametro);
                    return Parametro;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un Parametro con el id proporcionado (Parametros)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo Parametro de proveedores
        /// </summary>
        /// <param name="parametro">Objeto a crear, el atributo ParametroId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Parametro Post(Parametro parametro, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Parametros)"));
                }
                // comprobar las precondiciones
                if (parametro == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(parametro);
                ctx.SaveChanges();
                return parametro;
            }
        }

        /// <summary>
        /// Modificar un Parametro de proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la Parametro </param>
        /// <param name="parametro">Grupo de poveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Parametro Put(int id, Parametro parametro, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Parametros)"));
                }
                // comprobar los formatos
                if (parametro == null || id != parametro.ParametroId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un Parametro con ese id existe
                Parametro pl = (from p in ctx.Parametros1
                                     where p.ParametroId == id
                                     select p).FirstOrDefault<Parametro>();
                // existe?
                if (pl == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un Parametro con el id proporcionado (Parametros)"));
                }
                // modificar el objeto
                ctx.AttachCopy<Parametro>(parametro);
                ctx.SaveChanges();
                return parametro;
            }
        }

        /// <summary>
        /// Elimina el Parametro que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la Parametro a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Parametros)"));
                }
                // primero buscamos si un Parametro con ese id existe
                Parametro gu = (from g in ctx.Parametros1
                                     where g.ParametroId == id
                                     select g).FirstOrDefault<Parametro>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un Parametro con el id proporcionado (Parametros)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}