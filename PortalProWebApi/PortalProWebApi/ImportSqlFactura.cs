using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;
using System.Data.SqlClient;

namespace PortalProWebApi
{
    public class ImportSqlFactura
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchFactura(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sql = "SELECT COUNT(*) FROM [PortalProTest].[dbo].[Cau_PortalPro_VCabFactura]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [ACCOUNTNUM]
                        ,[IDEMPRESA]
                        ,[INVOICEID]
                        ,[INVOICEDATE]
                        ,[INVOICEAMOUNT]
                        ,[FECHAPAGO]
                        ,[ESTADI]
                        ,[FECHAVENCIMIENTO]
                    FROM [PortalProTest].[dbo].[Cau_PortalPro_VCabFactura]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string codAxProveedor = dr.GetString(0);
                string numFactura = dr.GetString(2);
                DateTime fechaFactura = dr.GetDateTime(3);
                Proveedor proveedor = (from pr in ctx.Proveedors
                                       where pr.CodAx == codAxProveedor
                                       select pr).FirstOrDefault<Proveedor>();
                // Buscamos si la fcatura ya existe
                CabFactura fac = (from f in ctx.CabFacturas
                                  where f.NumFactura == numFactura &&
                                        f.FechaEmision == fechaFactura
                                  select f).FirstOrDefault<CabFactura>();
                if (fac == null)
                {
                    fac = new CabFactura();
                    ctx.Add(fac);
                }
                fac.Proveedor = proveedor;
                fac.NumFactura = numFactura;
                fac.FechaEmision = fechaFactura;
                fac.Empresa = (from e in ctx.Empresas
                               where e.CodAx == dr.GetString(1)
                               select e).FirstOrDefault<Empresa>();
                string estado = dr.GetString(6);
                switch (estado)
                {
                    case "Pagado":
                        fac.Estado = "PAGADA";
                        break;
                    case "Recibido":
                        fac.Estado = "PROCESADA";
                        break;
                }
                fac.TotalFactura = dr.GetDecimal(4);
                if (!dr.IsDBNull(5))
                    fac.FechaCobro = dr.GetDateTime(5);
                if (!dr.IsDBNull(7))
                    fac.FechaPrevistaCobro = dr.GetDateTime(7);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                }
                // cargar las lineas
                try
                {
                    LoadAssociateLines(numFactura, fechaFactura);
                }
                catch (Exception ex)
                {
                }
                // Actualizar los registros de proceso
                Progresos progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 4
                                      select p).FirstOrDefault<Progresos>();
                if (progreso != null)
                {
                    progreso.NumReg = numreg;
                    progreso.TotReg = totreg;
                    ctx.SaveChanges();
                }
            }
            dr.Close();
            ctx.Dispose();
            con.Close();
            con.Dispose();
            return "";
        }

        private void LoadAssociateLines(string numFactura, DateTime fechaFactura)
        {
            PortalProContext ctx = new PortalProContext();
            // buscamos la cabecera de pedido relacionada
            CabFactura factura = (from f in ctx.CabFacturas
                                  where f.NumFactura == numFactura &&
                                        f.FechaEmision == fechaFactura
                                  select f).FirstOrDefault<CabFactura>();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sqlb = @"SELECT [INVOICEID]
                              ,[INVOICEDATE]
                              ,[LINENUM]
                              ,[PURCHID]
                              ,[LINEAMOUNT]
                          FROM [PortalProTest].[dbo].[Cau_portalpro_VLinFactura] WHERE [INVOICEID] = '{0}' AND [INVOICEDATE] = '{1:yyyMMdd}';";
            string sql = String.Format(sqlb, numFactura, fechaFactura);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int numLinea = (int)(dr.GetDecimal(2));
                // --
                LinFactura lf = (from l in ctx.LinFacturas
                                 where l.NumFactura == numFactura &&
                                       l.FechaEmision == fechaFactura &&
                                       l.NumLineaFactura == numLinea
                                 select l).FirstOrDefault<LinFactura>();
                if (lf == null)
                {
                    lf = new LinFactura();
                    ctx.Add(lf);
                }
                lf.CabFactura = factura;
                lf.NumFactura = numFactura;
                lf.FechaEmision = fechaFactura;
                lf.NumLineaFactura = numLinea;
                lf.Descripcion = "";
                lf.Importe = dr.GetDecimal(4);
                ctx.SaveChanges();
            }
            dr.Close();
            ctx.Dispose();
            con.Close();
            con.Dispose();
        }
    }

    public delegate string AsyncLaunchSqlFactura(out int threadId);
}