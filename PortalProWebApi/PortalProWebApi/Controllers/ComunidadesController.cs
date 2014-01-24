using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class ComunidadesController : ApiController
    {
        public virtual IEnumerable<Comunidad> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    IEnumerable<Comunidad> comunidades = (from c in ctx.Comunidads
                                                          orderby c.Nombre
                                                          select c).ToList<Comunidad>();
                    comunidades = ctx.CreateDetachedCopy<IEnumerable<Comunidad>>(comunidades);
                    return comunidades;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (comunidades)"));
                }
            }
        }

        public virtual IEnumerable<Comunidad> Get(string pais, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    int paisId = int.Parse(pais);
                    IEnumerable<Comunidad> comunidades = (from c in ctx.Comunidads
                                                          where c.Pais.PaisId == paisId
                                                          orderby c.Nombre
                                                          select c).ToList<Comunidad>();
                    comunidades = ctx.CreateDetachedCopy<IEnumerable<Comunidad>>(comunidades);
                    return comunidades;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (comunidades)"));
                }
            }
        }
    }
}