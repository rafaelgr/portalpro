using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class ProvinciasController : ApiController
    {
        public virtual IEnumerable<Provincia> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    IEnumerable<Provincia> provincias = (from c in ctx.Provincias
                                                          orderby c.Nombre
                                                          select c).ToList<Provincia>();
                    provincias = ctx.CreateDetachedCopy<IEnumerable<Provincia>>(provincias);
                    return provincias;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (provincias)"));
                }
            }
        }

        public virtual IEnumerable<Provincia> Get(string comunidad, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    int comunidadId = int.Parse(comunidad);
                    IEnumerable<Provincia> provincias = (from c in ctx.Provincias
                                                          where c.Comunidad.ComunidadId == comunidadId
                                                          orderby c.Nombre
                                                          select c).ToList<Provincia>();
                    provincias = ctx.CreateDetachedCopy<IEnumerable<Provincia>>(provincias);
                    return provincias;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (provincias)"));
                }
            }
        }
    }
}