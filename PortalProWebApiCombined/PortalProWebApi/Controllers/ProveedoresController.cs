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
    public class ProveedoresController : ApiController
    {
        /// <summary>
        /// Obtiene todos los proveedores de la base de datos
        /// </summary>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual IEnumerable<Proveedor> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    IEnumerable<Proveedor> proveedores = (from pr in ctx.Proveedors
                                                     select pr).ToList<Proveedor>();

                    // fetch estrategy, necesaria para poder devolver el grupo junto con cada usuariuo
                    FetchStrategy fs = new FetchStrategy();
                    fs.LoadWith<Proveedor>(x => x.GrupoProveedor);
                    proveedores = ctx.CreateDetachedCopy<IEnumerable<Proveedor>>(proveedores, fs);
                    return proveedores;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
            }
        }

        /// <summary>
        /// Obtiene el proveedor cuyo ID corresponde con el pasado
        /// </summary>
        /// <param name="id">Identificador único del proveedor</param>
        /// <param name="tk">Código del tique de autorización (Véase "Login")</param>
        /// <returns></returns>
        public virtual Proveedor Get(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Proveedor proveedor = (from p in ctx.Proveedors
                                       where p.ProveedorId == id
                                       select p).FirstOrDefault<Proveedor>();
                    if (proveedor != null)
                    {
                        proveedor = ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
                        return proveedor;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
            }
        }

        public virtual IEnumerable<Documento> GetDocumentos(int idPro, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    Proveedor proveedor = (from p in ctx.Proveedors
                                           where p.ProveedorId == idPro
                                           select p).FirstOrDefault<Proveedor>();
                    if (proveedor != null)
                    {
                        IEnumerable<Documento> docs = (from d in ctx.Documentos
                                                       where d.Proveedor.ProveedorId == idPro
                                                       select d).ToList<Documento>();
                        // La aplicación ahora depende del comienzo del usuario
                        string application = "PortalPro";
                        switch (userId.Substring(0, 1))
                        {
                            case "U":
                                application = "PortalPro2";
                                break;
                            case "G":
                                application = "PortalPro";
                                break;
                        }
                        foreach (Documento d in docs)
                        {
                            d.DescargaUrl = PortalProWebUtility.CargarUrlDocumento(application, d, tk);
                        }
                        FetchStrategy fs = new FetchStrategy();
                        fs.LoadWith<Documento>(x => x.TipoDocumento);
                        IEnumerable<Documento> documentos = ctx.CreateDetachedCopy<IEnumerable<Documento>>(docs, fs);
                        return documentos;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
            }
        }




        /// <summary>
        /// Crear un nuevo proveedor
        /// </summary>
        /// <param name="Proveedor">Objeto a crear, el atributo ProveedorId lo genera la aplicación y es devuelto en el objeto incluido en la respuesta.</param>
        /// <param name="tk"> Tique de autorzación (se debe obtener con la accion Login)</param>
        /// <returns></returns>
        public virtual Proveedor Post(Proveedor proveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // comprobar las precondiciones
                if (proveedor == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (proveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = proveedor.GrupoProveedor.GrupoProveedorId;
                    proveedor.GrupoProveedor = null;
                }
                // dar de alta el objeto en la base de datos y devolverlo en el mensaje
                ctx.Add(proveedor);
                if (grupoProveedorId != 0)
                {
                    proveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                            where gp.GrupoProveedorId == grupoProveedorId
                                            select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
            }
        }

        /// <summary>
        /// Modificar un proveedor. En el cuerpo del mensaje se envía en el formato adecuado el objeto con los datos modificados
        /// </summary>
        /// <param name="id"> Identificador único del grupo </param>
        /// <param name="proveedor">Grupo de proveedor con los valores que se desean en sus atributos</param>
        /// <param name="tk"> Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual Proveedor Put(int id, Proveedor proveedor, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // comprobar los formatos
                if (proveedor == null || id != proveedor.ProveedorId)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un proveedor con ese id existe
                Proveedor pro = (from p in ctx.Proveedors
                               where p.ProveedorId == id
                               select p).FirstOrDefault<Proveedor>();
                // existe?
                if (pro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                }
                int grupoProveedorId = 0;
                // Controlamos las propiedades que son en realidad objetos.
                if (proveedor.GrupoProveedor != null)
                {
                    grupoProveedorId = proveedor.GrupoProveedor.GrupoProveedorId;
                    proveedor.GrupoProveedor = null;
                }
                // modificar el objeto
                ctx.AttachCopy<Proveedor>(proveedor);
                // volvemos a leer el objecto para que lo maneje este contexto.
                proveedor = (from u in ctx.Proveedors
                           where u.ProveedorId == id
                           select u).FirstOrDefault<Proveedor>();
                if (grupoProveedorId != 0)
                {
                    proveedor.GrupoProveedor = (from gp in ctx.GrupoProveedors
                                            where gp.GrupoProveedorId == grupoProveedorId
                                            select gp).FirstOrDefault<GrupoProveedor>();
                }
                ctx.SaveChanges();
                return ctx.CreateDetachedCopy<Proveedor>(proveedor, x => x.GrupoProveedor);
            }
        }

        public virtual bool Put(int idPro, IEnumerable<Documento> documentos, string userId, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (LinFactura)"));
                }
                // comprobamos que los documentos no son nulos
                if (documentos == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                // primero buscamos si un proveedor con ese id existe
                Proveedor pro = (from p in ctx.Proveedors
                                 where p.ProveedorId == idPro
                                 select p).FirstOrDefault<Proveedor>();
                if (pro == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un proveedor con el id proporcionado (Proveedores)"));
                }
                // primero eliminamos los posibles documentos anteriores
                foreach (Documento d in pro.Documentos)
                {
                    PortalProWebUtility.EliminarDocumento(d, ctx);
                }
                // La aplicación ahora depende del comienzo del usuario
                string application = "PortalPro";
                switch (userId.Substring(0, 1))
                {
                    case "U":
                        application = "PortalPro2";
                        break;
                    case "G":
                        application = "PortalPro";
                        break;
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
                            string fieldId = String.Format("PDFT{0}", doc.TipoDocumento.TipoDocumentoId);
                            string fpdf = PortalProWebUtility.BuscarArchivoCargado(application, userId, "Proveedor", fieldId);
                            if (fpdf != "")
                            {
                                Documento vDoc = PortalProWebUtility.CrearDocumentoDesdeArchivoCargado(application, fpdf, ctx);
                                vDoc.TipoDocumento = tp;
                                vDoc.Proveedor = pro;
                                ctx.Add(vDoc);
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Elimina el proveedor que coincide con el id pasado
        /// </summary>
        /// <param name="id">Identificador del proveedor a eliminar</param>
        /// <param name="tk">Tique de autorización (Ver 'Login')</param>
        /// <returns></returns>
        public virtual bool Delete(int id, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                // comprobar el tique
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Proveedores)"));
                }
                // primero buscamos si un grupo con ese id existe
                Proveedor pro = (from u in ctx.Proveedors
                               where u.ProveedorId == id
                               select u).FirstOrDefault<Proveedor>();
                // existe?
                if (pro == null)
                {
                     throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un grupo con el id proporcionado (Proveedores)"));
                }
                // hay que eliminar los ficheros asociados.
                foreach (Documento d in pro.Documentos)
                {
                    PortalProWebUtility.EliminarDocumento(d, ctx);
                }
                ctx.Delete(pro);
                ctx.SaveChanges();
                return true;
            }
        }
        
    }
}