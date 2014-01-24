using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class ActividadesPrincipalesController : ApiController
    {
        public virtual IEnumerable<ActividadPrincipal> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    IEnumerable<ActividadPrincipal> actividadesPrincipales = (from ai in ctx.ActividadPrincipals
                                                                              orderby ai.Nombre
                                                                              select ai).ToList<ActividadPrincipal>();
                    actividadesPrincipales = ctx.CreateDetachedCopy<IEnumerable<ActividadPrincipal>>(actividadesPrincipales);
                    return actividadesPrincipales;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Actividades principales)"));
                }
            }
        }
    }
}