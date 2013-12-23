using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;

namespace PortalProWebApi
{
    public class ImportPedido
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchPedido(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            int numreg = 0;
            int totreg = 0;
            PortalProContext ctx = new PortalProContext();
            EntitiesModel con = new EntitiesModel();
            var rs = (from cp in con.Cau_PortalPro_VCabPedidos
                      select cp);
            totreg = rs.Count();
            foreach (Cau_PortalPro_VCabPedido cp1 in rs)
            {
                numreg++;
                // Buscamos si ese pedido ya existe
                Pedido p2 = (from p in ctx.Pedidos
                             where p.NumPedido == cp1.PURCHID
                             select p).FirstOrDefault<Pedido>();
                if (p2 == null)
                {
                    p2 = new Pedido();
                    p2.NumPedido = cp1.PURCHID;
                    p2.FechaAlta = cp1.CREATEDDATE;
                    // buscamos al proveedor
                    p2.Proveedor = (from pr in ctx.Proveedors
                                    where pr.CodAx == cp1.INVOICEACCOUNT
                                    select pr).FirstOrDefault<Proveedor>();
                    p2.TotalPedido = 0;
                    p2.TotalFacturado = 0;
                    p2.Estado = cp1.ESTADO;
                    p2.DocumentoPdf = null;
                    p2.DocumentoXml = null;
                    p2.TipoPedido = cp1.TIPO;
                    // buscar la empresa
                    p2.Empresa = (from e in ctx.Empresas
                                  where e.CodAx == cp1.DATAAREAID
                                  select e).FirstOrDefault<Empresa>();
                    p2.FechaLimite = cp1.FECHALIMITE;
                    p2.FechaRecepcion = cp1.FECHARECEPCION;
                }
                else
                {

                }
                // Actualizar los registros de proceso
                Progresos progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 3
                                      select p).FirstOrDefault<Progresos>();
                if (progreso != null)
                {
                    progreso.NumReg = numreg;
                    progreso.TotReg = totreg;
                    ctx.SaveChanges();
                }
            }
            return "";
        }
    }

    public delegate string AsyncLaunchPedido(out int threadId);
}