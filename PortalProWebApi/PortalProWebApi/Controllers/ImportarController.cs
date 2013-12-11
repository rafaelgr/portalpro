﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class ImportarController : ApiController
    {
        public virtual Progresos Get(string proceso, string tk)
        {
            Progresos pgs = null;
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    int procesoId = 0;
                    switch (proceso)
                    {
                        case "Empresa":
                            procesoId = 1;
                            break;
                        case "Proveedor":
                            procesoId = 2;
                            break;
                        case "Pedido":
                            procesoId = 3;
                            break;
                        case "Factura":
                            procesoId = 4;
                            break;
                        case "Responsable":
                            procesoId = 5;
                            break;
                    }
                    pgs = (from p in ctx.Progresos
                           where p.ProgresoId == procesoId
                           select p).FirstOrDefault<Progresos>();
                    pgs = ctx.CreateDetachedCopy<Progresos>(pgs);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Importar)"));
                }
            }
            return pgs;
        }
        public virtual bool Post(string proceso, string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (CntWebApiSeguridad.CheckTicket(tk, ctx) || tk == "solicitud")
                {
                    switch (proceso)
                    {
                        case "Empresa":
                            CntAxapta.ImportarEmpresas();
                            break;
                        case "Proveedor":
                            CntAxapta.ImportarProveedors();
                            break;
                        case "Responsable":
                            CntAxapta.ImportarResponsables();
                            break;
                    }

                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Importar)"));
                }
            }
            return true;
        }
    }
}
