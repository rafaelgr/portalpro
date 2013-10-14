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
    public class CabFacturaController : ApiController
    {
        /// <summary>
        /// Obtiene todos las cabeceras de factura de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<CabFactura> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<CabFactura> facturas = (from f in ctx.CabFacturas
                                                        select f).ToList<CabFactura>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<CabFactura>(x => x.Proveedor);
                    fs.LoadWith<CabFactura>(x => x.DocumentoPdf);
                    fs.LoadWith<CabFactura>(x => x.DocumentoXml);
                    facturas = ctx.CreateDetachedCopy<IEnumerable<CabFactura>>(facturas, fs);
                    return facturas;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        /// <summary>
        /// Obtiene la cabecera de factura cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único de la factura</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual CabFactura Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    CabFactura factura = (from f in ctx.CabFacturas
                                          where f.CabFacturaId == id
                                          select f).FirstOrDefault<CabFactura>();
                    if (factura != null)
                    {
                        factura = ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor, x => x.DocumentoXml, x => x.DocumentoPdf);
                        return factura;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un factura con el id proporcionado (CabFactura)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
            }
        }

        /// <summary>
        /// Crear un nueva cabecera de factura
        /// </summary>
        /// <param name="CabFactura">Objeto a crear, el atributo CabFacturaId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual CabFactura Post(CabFactura factura, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (CabFactura)"));
                }
                // comprobar las precondiciones
                if (factura == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }

                // Controlamos las propiedades que son en realidad objetos.
                int proveedorId = 0;
                if (factura.Proveedor != null)
                {
                    proveedorId = factura.Proveedor.ProveedorId;
                    factura.Proveedor = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(factura);
                if (proveedorId != 0)
                {
                    factura.Proveedor = (from p in ctx.Proveedors
                                         where p.ProveedorId == proveedorId
                                         select p).FirstOrDefault<Proveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<CabFactura>(factura, x => x.Proveedor);
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
                // controlar la contraseña.
                if (usuario.Password != null && usuario.Password != "" && usuario.Password != usu.Password)
                {
                    // se guarda la contraseña encriptada
                    usuario.Password = CntWebApiSeguridad.GetHashCode(usuario.Password);
                }
                int grupoUsuarioId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (usuario.GrupoUsuario != null)
                {
                    grupoUsuarioId = usuario.GrupoUsuario.GrupoUsuarioId;
                    usuario.GrupoUsuario = null;
                }
                // modificar el objeto
                ctx.AttachCopy<Usuario>(usuario);
                // volvemos a leer el objecto para que lo maneje este contexto.
                usuario = (from u in ctx.Usuarios
                           where u.UsuarioId == id
                           select u).FirstOrDefault<Usuario>();
                if (grupoUsuarioId != 0)
                {
                    usuario.GrupoUsuario = (from g in ctx.GrupoUsuarios
                                            where g.GrupoUsuarioId == grupoUsuarioId
                                            select g).FirstOrDefault<GrupoUsuario>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Usuario>(usuario, x => x.GrupoUsuario);
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