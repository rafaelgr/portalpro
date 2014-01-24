using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class PaisesController : ApiController
    {
        public virtual IEnumerable<Pais> Get(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    IEnumerable<Pais> paises = (from p in ctx.Pais
                                                orderby p.Nombre
                                                select p).ToList<Pais>();
                    paises = ctx.CreateDetachedCopy<IEnumerable<Pais>>(paises);
                    return paises;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Paises)"));
                }
            }
        }
    }
}