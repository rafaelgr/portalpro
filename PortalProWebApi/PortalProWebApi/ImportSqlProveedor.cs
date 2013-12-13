using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;

namespace PortalProWebApi
{
    public class ImportSqlProveedor
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchProveedor(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sql = "SELECT COUNT(*) FROM [PortalProTest].[dbo].[Cau_PortalPro_VProveedores]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [ACCOUNTNUM]
                        ,[NAME]
                        ,[ADDRESS]
                        ,[CITY]
                        ,[ZIPCODE]
                        ,[COUNTRYREGIONID]
                        ,[PHONE]
                        ,[TELEFAX]
                        ,[CELLULARPHONE]
                        ,[EMAIL]
                        ,[VATNUM]
                        ,[CONTACTO]
                        ,[LINEOFBUSINESSID]
                        ,[CAUPORTALPROEMAIL]
                        ,[BANKIBAN]
                        ,[CAUPORTALPROALLOWINVOICE]
                    FROM [PortalProTest].[dbo].[Cau_PortalPro_VProveedores]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string accountnum = dr.GetString(0);
                Proveedor pr2 = (from p2 in ctx.Proveedors
                                 where p2.CodAx == accountnum
                                 select p2).FirstOrDefault<Proveedor>();
                if (pr2 == null)
                {
                    pr2 = new Proveedor();
                    ctx.Add(pr2);
                }
                pr2.CodAx = accountnum;
                pr2.NombreComercial = dr.GetString(1);
                pr2.RazonSocial = dr.GetString(1);
                pr2.Direccion = dr.GetString(2);
                pr2.Localidad = dr.GetString(3);
                pr2.CodPostal = dr.GetString(4);
                pr2.Pais = dr.GetString(5);
                pr2.Telefono = dr.GetString(6);
                pr2.Fax = dr.GetString(7);
                pr2.Movil = dr.GetString(8);
                pr2.Email = dr.GetString(9);
                pr2.Nif = dr.GetString(10);
                if (!dr.IsDBNull(11)) pr2.PersonaContacto = dr.GetString(11);
                pr2.EmailFacturas = dr.GetString(13);
                if (!dr.IsDBNull(14)) pr2.IBAN = dr.GetString(14);
                ctx.SaveChanges();
                // Actualizar los registros de proceso
                Progresos progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 2
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
    }

    public delegate string AsyncLaunchSqlProveedor(out int threadId);
}