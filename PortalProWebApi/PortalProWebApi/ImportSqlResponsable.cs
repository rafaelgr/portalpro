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
    public class ImportSqlResponsable
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchResponsable(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            // Actualizar los registros de proceso
            Progresos progreso = (from p in ctx.Progresos
                                  where p.ProgresoId == 5
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
            string sql = "SELECT COUNT(*) FROM [dbo].[Cau_PortalPro_VResponsable]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [ID]
                        ,[NAME]
                        ,[EMAIL]
                    FROM [dbo].[Cau_PortalPro_VResponsable]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string codax = dr.GetString(0);
                // Buscamos si esa empresa existe
                Responsable res2 = (from e2 in ctx.Responsables
                                where e2.CodAx == codax
                                select e2).FirstOrDefault<Responsable>();
                if (res2 == null)
                {
                    res2 = new Responsable();
                    ctx.Add(res2);
                }
                res2.CodAx = codax;
                res2.Nombre = dr.GetString(1);
                res2.Email = dr.GetString(2);
                ctx.SaveChanges();
                // Actualizar los registros de proceso
                progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 5
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
    public delegate string AsyncLaunchSqlResponsable(out int threadId);
}