using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class PlantillasController : ApiController
    {
        /// <summary>
        /// Obtiene todos las plantillas de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Plantilla> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Plantilla> plantillas = (from pl in ctx.Plantillas
                                                                     select pl).ToList<Plantilla>();
                    plantillas = ctx.CreateDetachedCopy<IEnumerable<Plantilla>>(plantillas);
                    return plantillas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Plantillas)"));
                }
            }
        }

        /// <summary>
        /// Obtiene una plantilla cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la plantilla</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Plantilla Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Plantilla plantilla = (from pl in ctx.Plantillas
                                                     where pl.PlantillaId == id
                                                     select pl).FirstOrDefault<Plantilla>();
                    if (plantilla != null)
                    {
                        plantilla = ctx.CreateDetachedCopy<Plantilla>(plantilla);
                        return plantilla;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un plantilla con el id proporcionado (Plantillas)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Plantillas)"));
                }
            }
        }

        /// <summary>
        /// Obtiene una plantilla cuyo ID corresponde con el pasado
        /// en este caso no necesita tique porque es para solicitudes.
        /// </summary>
        /// <param name="id">Identificador único de la plantilla</param>
        /// <returns></returns>
        public virtual Plantilla Get(int id)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                Plantilla plantilla = (from pl in ctx.Plantillas
                                                 where pl.PlantillaId == id
                                                 select pl).FirstOrDefault<Plantilla>();
                if (plantilla != null)
                {
                    plantilla = ctx.CreateDetachedCopy<Plantilla>(plantilla);
                    return plantilla;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un plantilla con el id proporcionado (Plantillas)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo plantilla de proveedores
        /// </summary>
        /// <param name="plantilla">Objeto a crear, el atributo PlantillaId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Plantilla Post(Plantilla plantilla, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Plantillas)"));
                }
                // comprobar las precondiciones
                if (plantilla == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(plantilla);
                ctx.SaveChanges();
                return plantilla;
            }
        }

        /// <summary>
        /// Modificar un plantilla de proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la plantilla </param>
        /// <param name="plantilla">Grupo de poveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Plantilla Put(int id, Plantilla plantilla, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Plantillas)"));
                }
                // comprobar los formatos
                if (plantilla == null || id != plantilla.PlantillaId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un plantilla con ese id existe
                Plantilla pl = (from p in ctx.Plantillas
                                     where p.PlantillaId == id
                                     select p).FirstOrDefault<Plantilla>();
                // existe?
                if (pl == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un plantilla con el id proporcionado (Plantillas)"));
                }
                // modificar el objeto
                ctx.AttachCopy<Plantilla>(plantilla);
                ctx.SaveChanges();
                return plantilla;
            }
        }

        /// <summary>
        /// Elimina el plantilla que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la plantilla a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Plantillas)"));
                }
                // primero buscamos si un plantilla con ese id existe
                Plantilla gu = (from g in ctx.Plantillas
                                     where g.PlantillaId == id
                                     select g).FirstOrDefault<Plantilla>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un plantilla con el id proporcionado (Plantillas)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}