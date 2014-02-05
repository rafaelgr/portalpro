using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PortalProModelo;
using Telerik.OpenAccess.FetchOptimization;

namespace PortalProWebApi.Controllers
{
    public class SolicitudesProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos las solicitudes de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<SolicitudProveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<SolicitudProveedor> solProveedores = (from pr in ctx.SolicitudProveedors
                                                                      select pr).ToList<SolicitudProveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<SolicitudProveedor>(x => x.GrupoProveedor);
                    fs.LoadWith<SolicitudProveedor>(x => x.SolicitudStatus);
                    fs.LoadWith<SolicitudProveedor>(x => x.ActividadPrincipal1);
                    fs.LoadWith<SolicitudProveedor>(x => x.Pais1);
                    fs.LoadWith<SolicitudProveedor>(x => x.Comunidad1);
                    fs.LoadWith<SolicitudProveedor>(x => x.Provincia1);
                    solProveedores = ctx.CreateDetachedCopy<IEnumerable<SolicitudProveedor>>(solProveedores, fs);
                    return solProveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
            }
        }

        /// <summary>
        /// Devuelve las solicitudes que se encuentran en un determinado estado
        /// </summary>
        /// <param name="tk">Tique de autorización</param>
        /// <param name="estado">Posibles estados (1=pendiente / 2=Aceptada / 3=Rechazada)</param>
        /// <returns></returns>
        public virtual IEnumerable<SolicitudProveedor> GetPorEstado(string tk, string estado)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<SolicitudProveedor> solProveedores = (from pr in ctx.SolicitudProveedors
                                                                      where pr.SolicitudStatus.SolicitudStatusId == int.Parse(estado)
                                                                      select pr).ToList<SolicitudProveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<SolicitudProveedor>(x => x.GrupoProveedor);
                    fs.LoadWith<SolicitudProveedor>(x => x.SolicitudStatus);
                    solProveedores = ctx.CreateDetachedCopy<IEnumerable<SolicitudProveedor>>(solProveedores, fs);
                    return solProveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene la solicitud cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador únicode la solicitud</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    SolicitudProveedor solProveedor = (from sp in ctx.SolicitudProveedors
                                                       where sp.SolicitudProveedorId == id
                                                       select sp).FirstOrDefault<SolicitudProveedor>();
                    if (solProveedor != null)
                    {
                        solProveedor = ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor, x => x.SolicitudStatus, x => x.SolicitudStatus, x => x.ActividadPrincipal1, x => x.Pais1, x => x.Comunidad1, x => x.Provincia1);
                        return solProveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
            }
        }

        public virtual IEnumerable<Documento> GetDocumentos(int idSolPro, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    SolicitudProveedor solPro = (from s in ctx.SolicitudProveedors
                                                 where s.SolicitudProveedorId == idSolPro
                                                 select s).FirstOrDefault<SolicitudProveedor>();
                    if (solPro != null)
                    {
                        IEnumerable<Documento> docs = (from d in ctx.Documentos
                                                       where d.SolicitudProveedor.SolicitudProveedorId == idSolPro
                                                       select d).ToList<Documento>();
                        //
                        HttpRequest request = HttpContext.Current.Request;
                        // hay que obtener la URL de descarga para cada uno de los documentos
                        foreach (Documento d in docs)
                        {
                            string item = String.Format("DOCF{0}", d.TipoDocumento.TipoDocumentoId);
                            d.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(d, userId, item, request);
                        }
                        FetchStrategy fs = new FetchStrategy();
                        fs.LoadWith<Documento>(x => x.TipoDocumento);
                        IEnumerable<Documento> documentos = ctx.CreateDetachedCopy<IEnumerable<Documento>>(docs, fs);
                        return documentos;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitud Proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitud Proveedores)"));
                }
            }
        }

        /// <summary>
        /// Crear un nueva solicitud
        /// </summary>
        /// <param name="Proveedor">Objeto a crear, el atributo SolicitudProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorización (se debe obtener con la accion Login). Caso especial "solicitud"</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Post(SolicitudProveedor solProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                // se permite el que lleva el texto "solicitud" siempre
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // comprobar las precondiciones
                if (solProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int grupoProveedorId = 0;
                int solicitudStatusId = 1; // cuando se crean las solicitudes su estado es pendiente.
                // Controlamos las propiedades que son en realidad objetos.
                if (solProveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = solProveedor.GrupoProveedor.GrupoProveedorId;
                    solProveedor.GrupoProveedor = null;
                }
                int actividadPrincipalId = 0;
                if (solProveedor.ActividadPrincipal1 != null)
                {
                    actividadPrincipalId = solProveedor.ActividadPrincipal1.ActividadPrincipalId;
                    solProveedor.ActividadPrincipal1 = null;
                }
                int paisId = 0;
                if (solProveedor.Pais1 != null)
                {
                    paisId = solProveedor.Pais1.PaisId;
                    solProveedor.Pais1 = null;
                }
                int comunidadId = 0;
                if (solProveedor.Comunidad1 != null)
                {
                    comunidadId = solProveedor.Comunidad1.ComunidadId;
                    solProveedor.Comunidad1 = null;
                }
                int provinciaId = 0;
                if (solProveedor.Provincia1 != null)
                {
                    provinciaId = solProveedor.Provincia1.ProvinciaId;
                    solProveedor.Provincia1 = null;
                }
                // justo antes de darlo de alta le ponemos el sello
                solProveedor.Sello = DateTime.Now;
                // por defecto su estado es pendiente ID=1
                solProveedor.SolicitudStatus = (from ss in ctx.SolicitudStatus
                                                where ss.SolicitudStatusId == 1
                                                select ss).FirstOrDefault<SolicitudStatus>();
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(solProveedor);

                if (grupoProveedorId != 0)
                {
                    solProveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                   where gp.GrupoProveedorId == grupoProveedorId
                                                   select gp).FirstOrDefault<GrupoProveedor>();
                }
                if (solicitudStatusId != 0)
                {
                    solProveedor.SolicitudStatus = (from ss in ctx.SolicitudStatus
                                                    where ss.SolicitudStatusId == solicitudStatusId
                                                    select ss).FirstOrDefault<SolicitudStatus>();
                }
                if (actividadPrincipalId != 0)
                {
                    solProveedor.ActividadPrincipal1 = (from ap in ctx.ActividadPrincipals
                                                        where ap.ActividadPrincipalId == actividadPrincipalId
                                                        select ap).FirstOrDefault<ActividadPrincipal>();
                }
                if (paisId != 0)
                {
                    solProveedor.Pais1 = (from p in ctx.Pais
                                          where p.PaisId == paisId
                                          select p).FirstOrDefault<Pais>();
                }
                if (comunidadId != 0)
                {
                    solProveedor.Comunidad1 = (from c in ctx.Comunidads
                                               where c.ComunidadId == comunidadId
                                               select c).FirstOrDefault<Comunidad>();
                }
                if (provinciaId != 0)
                {
                    solProveedor.Provincia1 = (from pr in ctx.Provincias
                                               where pr.ProvinciaId == provinciaId
                                               select pr).FirstOrDefault<Provincia>();
                }

                ctx.SaveChanges();
                // eliminamos los documentos asociados si los hay
                // los dará de alta otro proceso.
                foreach (Documento d in solProveedor.Documentos)
                {
                    PortalProWebUtility.EliminarDocumento(d, ctx);
                }
                // preparamos y enviamos el correo de confirmación por defecto (por si falla la plantilla).
                string asunto = "[PortalPro] Recibida solicitud";
                string cuerpo = String.Format("Su solicitud con ID:{0} ha sido recibida. No responda este mensaje", solProveedor.SolicitudProveedorId);
                // El primer paso es obtener la plantilla ID=1
                Plantilla plantilla = (from pl in ctx.Plantillas
                                       where pl.PlantillaId == 1
                                       select pl).FirstOrDefault<Plantilla>();
                if (plantilla != null)
                {
                    asunto = String.Format(plantilla.Asunto, solProveedor.SolicitudProveedorId, solProveedor.RazonSocial, solProveedor.Direccion, solProveedor.Localidad,
                        solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                        solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif);
                    cuerpo = String.Format(plantilla.Cuerpo, solProveedor.SolicitudProveedorId, solProveedor.RazonSocial, solProveedor.Direccion, solProveedor.Localidad,
                        solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                        solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif);
                }
                PortalProMailController.SendEmail(solProveedor.Email, asunto, cuerpo);
                return ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor, x => x.SolicitudStatus, x => x.ActividadPrincipal1, x => x.Pais1, x => x.Comunidad1, x => x.Provincia1);
            }
        }

        public virtual bool PostEnvioSolicitud(string email, string link, string comentarios, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // preparamos y enviamos el correo de confirmación por defecto (por si falla la plantilla).
                string asunto = "[PortalPro] Solicitud de alta";
                string cuerpo = String.Format("Use este enlace {0} copiándolo en su navegador para acceder al alta de  provedor", link);
                // El primer paso es obtener la plantilla 
                // en teoria la plantilla 4 es la que toca
                Plantilla plantilla = (from pl in ctx.Plantillas
                                       where pl.PlantillaId == 4
                                       select pl).FirstOrDefault<Plantilla>();
                if (plantilla != null)
                {
                    asunto = String.Format(plantilla.Asunto, link, comentarios);
                    cuerpo = String.Format(plantilla.Cuerpo, link, comentarios);
                }
                PortalProMailController.SendEmail(email, asunto, cuerpo);
            }
            return true;
        }

        /// <summary>
        /// Modificar una solicitud. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único de la solicitud </param>
        /// <param name="solProveedor">Solicitud de proveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login'). Caso "solicitud"</param>
        /// <returns></returns>
        public virtual SolicitudProveedor Put(int id, SolicitudProveedor solProveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                // se permite si el valor es "solicitud" siempre
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // comprobar los formatos
                if (solProveedor == null || id != solProveedor.SolicitudProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si una solicitud con ese id existe
                SolicitudProveedor solPro = (from sp in ctx.SolicitudProveedors
                                             where sp.SolicitudProveedorId == id
                                             select sp).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (solPro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                int grupoProveedorId = 0;
                int solicitudStatusId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (solProveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = solProveedor.GrupoProveedor.GrupoProveedorId;
                    solProveedor.GrupoProveedor = null;
                }
                if (solProveedor.SolicitudStatus != null)
                {
                    solicitudStatusId = solProveedor.SolicitudStatus.SolicitudStatusId;
                    solProveedor.SolicitudStatus = null;
                }
                int actividadPrincipalId = 0;
                if (solProveedor.ActividadPrincipal1 != null)
                {
                    actividadPrincipalId = solProveedor.ActividadPrincipal1.ActividadPrincipalId;
                    solProveedor.ActividadPrincipal1 = null;
                }
                int paisId = 0;
                if (solProveedor.Pais1 != null)
                {
                    paisId = solProveedor.Pais1.PaisId;
                    solProveedor.Pais1 = null;
                }
                int comunidadId = 0;
                if (solProveedor.Comunidad1 != null)
                {
                    comunidadId = solProveedor.Comunidad1.ComunidadId;
                    solProveedor.Comunidad1 = null;
                }
                int provinciaId = 0;
                if (solProveedor.Provincia1 != null)
                {
                    provinciaId = solProveedor.Provincia1.ProvinciaId;
                    solProveedor.Provincia1 = null;
                }
                // modificar el objeto
                ctx.AttachCopy<SolicitudProveedor>(solProveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                solProveedor = (from sp in ctx.SolicitudProveedors
                                where sp.SolicitudProveedorId == id
                                select sp).FirstOrDefault<SolicitudProveedor>();
                if (grupoProveedorId != 0)
                {
                    solProveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                                   where gp.GrupoProveedorId == grupoProveedorId
                                                   select gp).FirstOrDefault<GrupoProveedor>();
                }
                if (solicitudStatusId != 0)
                {
                    solProveedor.SolicitudStatus = (from ss in ctx.SolicitudStatus
                                                    where ss.SolicitudStatusId == solicitudStatusId
                                                    select ss).FirstOrDefault<SolicitudStatus>();
                }
                if (actividadPrincipalId != 0)
                {
                    solProveedor.ActividadPrincipal1 = (from ap in ctx.ActividadPrincipals
                                                        where ap.ActividadPrincipalId == actividadPrincipalId
                                                        select ap).FirstOrDefault<ActividadPrincipal>();
                }
                if (paisId != 0)
                {
                    solProveedor.Pais1 = (from p in ctx.Pais
                                          where p.PaisId == paisId
                                          select p).FirstOrDefault<Pais>();
                }
                if (comunidadId != 0)
                {
                    solProveedor.Comunidad1 = (from c in ctx.Comunidads
                                               where c.ComunidadId == comunidadId
                                               select c).FirstOrDefault<Comunidad>();
                }
                if (provinciaId != 0)
                {
                    solProveedor.Provincia1 = (from pr in ctx.Provincias
                                               where pr.ProvinciaId == provinciaId
                                               select pr).FirstOrDefault<Provincia>();
                }

                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<SolicitudProveedor>(solProveedor, x => x.GrupoProveedor, x => x.SolicitudStatus, x=> x.ActividadPrincipal1, x=> x.Pais1, x=>x.Comunidad1, x=>x.Provincia1);
            }
        }

        public virtual bool Put(int idSolPro, IEnumerable<Documento> documentos, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitud proveedor)"));
                }
                // comprobamos que los documentos no son nulos
                if (documentos == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un proveedor con ese id existe
                SolicitudProveedor solPro = (from s in ctx.SolicitudProveedors
                                             where s.SolicitudProveedorId == idSolPro
                                             select s).FirstOrDefault<SolicitudProveedor>();
                if (solPro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitud proveedores)"));
                }
                // primero eliminamos los posibles documentos anteriores
                foreach (Documento d in solPro.Documentos)
                {
                    PortalProWebUtility.EliminarDocumento(d, ctx);
                }

                // Ahora cargamos las lineas nuevas
                foreach (Documento doc in documentos)
                {
                    if (doc.TipoDocumento != null)
                    {
                        TipoDocumento tp = (from t in ctx.TipoDocumentos
                                            where t.TipoDocumentoId == doc.TipoDocumento.TipoDocumentoId
                                            select t).FirstOrDefault<TipoDocumento>();
                        if (tp != null)
                        {
                            Documento vDoc = PortalProWebUtility.CrearFicheroDocumento(userId, "DOCF" + doc.TipoDocumento.TipoDocumentoId, doc, ctx);
                            if (vDoc != null)
                            {
                                vDoc.TipoDocumento = tp;
                                vDoc.SolicitudProveedor = solPro;
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Cambia el estado de una solicitud y realiza la grabación 
        /// correspondiente en el log
        /// </summary>
        /// <param name="id">Identificador de la solicitud a procesar</param>
        /// <param name="tk">Tique de autorización (ver Login)</param>
        /// <param name="status">Código del estado al que se quiere pasar (2=Aceptada / 3=Rechazada)</param>
        /// <param name="userId">Identificador del usuario que avala el cambio</param>
        /// <param name="comentarios">Comentarios adicionales</param>
        /// <returns></returns>
        public virtual bool PutStatus(int id, string tk, int status, int userId, string comentarios)
        {
            bool res = false;
            using (PortalProContext ctx = new PortalProContext())
            {
                // Comprobamos la solicitud
                SolicitudProveedor solProveedor = (from sp in ctx.SolicitudProveedors
                                                   where sp.SolicitudProveedorId == id
                                                   select sp).FirstOrDefault<SolicitudProveedor>();
                if (solProveedor == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                if (solProveedor.SolicitudStatus == null || solProveedor.SolicitudStatus.SolicitudStatusId != 1)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "La solicitud ya ha sido procesada"));
                }
                // ya nos hemos asegurado que la soilictud existe ahora creamos el registro de procesamiento.
                SolicitudStatus st = (from s in ctx.SolicitudStatus
                                      where s.SolicitudStatusId == status
                                      select s).FirstOrDefault<SolicitudStatus>();
                if (st == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una estatus con el id proporcionado (Solicitudes proveedores)"));
                }
                // obtención de los usuarios
                Usuario usu = (from u in ctx.Usuarios
                               where u.UsuarioId == userId
                               select u).FirstOrDefault<Usuario>();
                if (usu == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "El usuario es incorrecto (Solicitudes proveedores)"));
                }
                // y ahora el regsitro de procesamiento
                SolicitudLog slg = new SolicitudLog();
                slg.Sello = DateTime.Now;
                slg.Comentarios = comentarios;
                slg.SolicitudProveedor = solProveedor;
                slg.Usuario = usu;
                slg.SolicitudStatusInicial = solProveedor.SolicitudStatus;
                slg.SolicitudStatusFinal = st;
                ctx.Add(slg);
                // cambiamos el estado de la solicitud
                solProveedor.SolicitudStatus = st;
                // y salvamos todo
                ctx.SaveChanges();
                // una vez hecho esto hay que informar por correo
                // preparamos y enviamos el correo de confirmación por defecto (por si falla la plantilla).
                string asunto = "[PortalPro] Recibida solicitud";
                string cuerpo = String.Format("Su solicitud con ID:{0} ha sido recibida. No responda este mensaje", solProveedor.SolicitudProveedorId);
                // El primer paso es obtener la plantilla en este caso su código coincide con el estatus
                Plantilla plantilla = (from pl in ctx.Plantillas
                                       where pl.PlantillaId == status
                                       select pl).FirstOrDefault<Plantilla>();
                if (plantilla != null)
                {
                    asunto = String.Format(plantilla.Asunto, solProveedor.SolicitudProveedorId, solProveedor.RazonSocial, solProveedor.Direccion, solProveedor.Localidad,
                        solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                        solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif, comentarios);
                    cuerpo = String.Format(plantilla.Cuerpo, solProveedor.SolicitudProveedorId, solProveedor.RazonSocial, solProveedor.Direccion, solProveedor.Localidad,
                        solProveedor.CodPostal, solProveedor.Provincia, solProveedor.Comunidad, solProveedor.Pais, solProveedor.Telefono, solProveedor.Fax,
                        solProveedor.Movil, solProveedor.Email, solProveedor.Url, solProveedor.Nif, comentarios);
                }
                PortalProMailController.SendEmail(solProveedor.Email, asunto, cuerpo);
            }
            return res;
        }

        /// <summary>
        /// Elimina la solicitud que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificadorde la solicitud a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Solicitudes proveedores)"));
                }
                // primero buscamos si un grupo con ese id existe
                SolicitudProveedor solPro = (from sp in ctx.SolicitudProveedors
                                             where sp.SolicitudProveedorId == id
                                             select sp).FirstOrDefault<SolicitudProveedor>();
                // existe?
                if (solPro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay una solicitud con el id proporcionado (Solicitudes proveedores)"));
                }
                ctx.Delete(solPro);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}