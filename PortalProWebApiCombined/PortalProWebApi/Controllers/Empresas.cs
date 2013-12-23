using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class EmpresasController : ApiController
    {
        /// <summary>
        /// Obtiene todos los empresas de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Empresa> Get(string tk)
        {
            
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk=="solicitud")
                {
                    IEnumerable<Empresa> empresas = (from gu in ctx.Empresas
                                                                select gu).ToList<Empresa>();
                    empresas = ctx.CreateDetachedCopy<IEnumerable<Empresa>>(empresas);
                    return empresas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Empresas)"));
                }
            }
        }

        /// <summary>
        /// Obtiene un Empresas cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la empresa</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Empresa Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Empresa empresa = (from gu in ctx.Empresas
                                                 where gu.EmpresaId == id
                                                 select gu).FirstOrDefault<Empresa>();
                    if (empresa != null)
                    {
                        empresa = ctx.CreateDetachedCopy<Empresa>(empresa);
                        return empresa;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Empresas)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Empresas)"));
                }
            }
        }

        /// <summary>
        /// Crear un nuevo Empresas
        /// </summary>
        /// <param name="empresa">Objeto a crear, el atributo EmpresaId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Empresa Post(Empresa empresa, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk,ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Empresas)"));
                }
                // comprobar las precondiciones
                if (empresa == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(empresa);
                ctx.SaveChanges();
                return empresa;
            }
        }

        /// <summary>
        /// Modificar una empresa. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la empresa </param>
        /// <param name="empresa">Grupo de usuario con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Empresa Put(int id, Empresa empresa, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Empresas)"));
                }
                // comprobar los formatos
                if (empresa == null || id != empresa.EmpresaId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un grupo con ese id existe
                Empresa gu = (from g in ctx.Empresas
                                   where g.EmpresaId == id
                                   select g).FirstOrDefault<Empresa>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Empresas)"));
                }
                // modificar el objeto
                ctx.AttachCopy<Empresa>(empresa);
                ctx.SaveChanges();
                return empresa;
            }
        }

        /// <summary>
        /// Elimina la empresa que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador de la empresa a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Empresas)"));
                }
                // primero buscamos si un grupo con ese id existe
                Empresa gu = (from g in ctx.Empresas
                                   where g.EmpresaId == id
                                   select g).FirstOrDefault<Empresa>();
                // existe?
                if (gu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Empresas)"));
                }
                ctx.Delete(gu);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
