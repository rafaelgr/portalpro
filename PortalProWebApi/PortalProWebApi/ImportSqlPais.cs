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
    public class ImportSqlPais
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchEmpresa(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            // Actualizar los registros de proceso para dejar bloqueada la barra
            Progresos progreso = (from p in ctx.Progresos
                                  where p.ProgresoId == 7
                                  select p).FirstOrDefault<Progresos>();
            if (progreso != null)
            {
                progreso.NumReg = 0;
                progreso.TotReg = 1;
                ctx.SaveChanges();
            }

            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            string sql = "SELECT COUNT(*) FROM [dbo].[Cau_PortalProv_AddressCountryRegion]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [COUNTRYREGIONID]
                        ,[NAME]
                    FROM [dbo].[Cau_PortalProv_AddressCountryRegion]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string codax = dr.GetString(0);
                // Buscamos si esa Pais existe
                Pais emp2 = (from e2 in ctx.Pais
                                where e2.CodAx == codax
                                select e2).FirstOrDefault<Pais>();
                if (emp2 == null)
                {
                    emp2 = new Pais();
                    ctx.Add(emp2);
                }
                emp2.CodAx = codax;
                emp2.Nombre = dr.GetString(1);
                ctx.SaveChanges();
                // Actualizar los registros de proceso
                progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 7
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
    public delegate string AsyncLaunchSqlPais(out int threadId);
}