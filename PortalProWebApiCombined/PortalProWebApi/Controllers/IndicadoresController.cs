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
    public class IndicadoresController : ApiController
    {
        /// <summary>
        /// Retorna un vector con parejas de nombre y valor
        /// que reflejan una serie de indicadores:
        /// n_solPro = Número de solicitudes de proveedores
        /// n_solProA = Núemro de solicitudes aceptadas.
        /// n_solProR = Número de solicitudes rechazadas
        /// </summary>
        /// <param name="tk">Tique de autorización (ver Login)</param>
        /// <returns></returns>
        public virtual IEnumerable<Indicador> Get(string tk)
        {
            IList<Indicador> lindi = new List<Indicador>();
            Indicador indi = null;
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    // Se van obteniendo uno a uno y aplciando a lista
                    // Número solicitudes de proveedor
                    int n_solPro = (from p in ctx.SolicitudProveedors
                                    select p).Count();
                    indi = new Indicador()
                    {
                        id = "n_solPro",
                        valor = (decimal)n_solPro
                    };
                    lindi.Add(indi);
                    // Número de solicitudes aprobadas
                    int n_solProA = (from p in ctx.SolicitudProveedors
                                     where p.SolicitudStatus.SolicitudStatusId == 2
                                     select p).Count();
                    indi = new Indicador()
                    {
                        id = "n_solProA",
                        valor = (decimal)n_solProA
                    };
                    lindi.Add(indi);
                    // Número de solicitudes rechazadas
                    int n_solProR = (from p in ctx.SolicitudProveedors
                                     where p.SolicitudStatus.SolicitudStatusId == 3
                                     select p).Count();
                    indi = new Indicador()
                    {
                        id = "n_solProR",
                        valor = (decimal)n_solProR
                    };
                    lindi.Add(indi);
                    int n_solProP = (from p in ctx.SolicitudProveedors
                                     where p.SolicitudStatus.SolicitudStatusId == 1
                                     select p).Count();
                    indi = new Indicador()
                    {
                        id = "n_solProP",
                        valor = (decimal)n_solProP
                    };
                    lindi.Add(indi);
                    return lindi;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (SolicitudLogs)"));
                }
            }
        }
    }
}
