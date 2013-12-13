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
    public class ImportSqlPedido
    {

        // este método se ejecutará de manera asíncrona.
        public string LaunchPedido(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sql = "SELECT COUNT(*) FROM [PortalProTest].[dbo].[Cau_PortalPro_VCabPedido]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [PURCHID]
                        ,[INVOICEACCOUNT]
                        ,[DATAAREAID]
                        ,[ESTADO]
                        ,[TIPO]
                        ,[CODCONTACTO]
                        ,[CONTACTO]
                        ,[CREATEDDATE]
                        ,[FECHARECEPCION]
                        ,[FECHALIMITE]
                    FROM [PortalProTest].[dbo].[Cau_PortalPro_VCabPedido]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string numpedido = dr.GetString(0);
                // Buscamos si esa empresa existe
                Pedido ped2 = (from p2 in ctx.Pedidos
                                where p2.NumPedido == numpedido
                                select p2).FirstOrDefault<Pedido>();
                if (ped2 == null)
                {
                    ped2 = new Pedido();
                    ctx.Add(ped2);
                }
                ped2.NumPedido = numpedido;
                ped2.Proveedor = (from pr in ctx.Proveedors
                                  where pr.CodAx == dr.GetString(1)
                                  select pr).FirstOrDefault<Proveedor>();
                ped2.Empresa = (from e in ctx.Empresas
                                    where e.CodAx == dr.GetString(2)
                                    select e).FirstOrDefault<Empresa>();
                if (!dr.IsDBNull(3)) ped2.Estado = dr.GetString(3);
                if (!dr.IsDBNull(4)) ped2.TipoPedido = dr.GetString(4);
                ped2.Responsable = (from r in ctx.Responsables
                                    where r.CodAx == dr.GetString(5)
                                    select r).FirstOrDefault<Responsable>();
                ped2.FechaAlta = dr.GetDateTime(7);
                if (!dr.IsDBNull(8)) ped2.FechaRecepcion = dr.GetDateTime(8);
                if (!dr.IsDBNull(9)) ped2.FechaRecepcion = dr.GetDateTime(9);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                }
                // cargar las lineas
                LoadAssociateLines(numpedido);
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
            dr.Close();
            ctx.Dispose();
            con.Close();
            con.Dispose();
            return "";
        }

        private void LoadAssociateLines(string numPedido)
        {
            PortalProContext ctx = new PortalProContext();
            // buscamos la cabecera de pedido relacionada
            Pedido pedido = (from p in ctx.Pedidos
                             where p.NumPedido == numPedido
                             select p).FirstOrDefault<Pedido>();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sqlb = @"SELECT [PURCHID]
                              ,[LINENUM]
                              ,[ITEMID]
                              ,[NAME]
                              ,[PURCHQTY]
                              ,[PURCHUNIT]
                              ,[PURCHPRICE]
                              ,[LINEAMOUNT]
                              ,[ESTADO]
                              ,[REMAINPURCHPHYSICAL]
                              ,[REMAINPURCHFINANCIAL]
                              ,[FECHARECEPCION]
                          FROM [PortalProTest].[dbo].[Cau_PortalPro_VLinPedido]  WHERE [PURCHID] = '{0}';";
            string sql = String.Format(sqlb, numPedido);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int numLinea = (int)(dr.GetDecimal(1));
                // --
                LinPedido lped = (from lp in ctx.LinPedidos
                                  where lp.NumPedido == numPedido
                                  && lp.NumLinea == numLinea
                                  select lp).FirstOrDefault<LinPedido>();
                if (lped == null)
                {
                    lped = new LinPedido();
                    ctx.Add(lped);
                }
                lped.Pedido = pedido;
                lped.NumPedido = numPedido;
                lped.NumLinea = numLinea;
                lped.Descripcion = dr.GetString(3);
                lped.Importe = dr.GetDecimal(7);
                string estado = dr.GetString(8);
                switch (estado)
                {
                    case "Facturado":
                        lped.Estado = "FACTURADO";
                        break;
                    case "Recibido":
                        lped.Estado = "RECIBIDO";
                        break;
                    case "Pedido Abierto":
                        lped.Estado = "ABIERTO";
                        break;

                }
                if (!dr.IsDBNull(11)) lped.FechaRecepcion = dr.GetDateTime(11);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }
            dr.Close();
            ctx.Dispose();
            con.Close();
            con.Dispose();
        }
    }
    public delegate string AsyncLaunchSqlPedido(out int threadId);
}