using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;

namespace PortalProWebApi
{
    public class ImportResponsable
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchResponsable(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
                        int numreg = 0;
            int totreg = 0;
            PortalProContext ctx = new PortalProContext();
            EntitiesModel con = new EntitiesModel();
            var rs = (from e in con.Cau_PortalPro_VResponsables
                      select e);
            totreg = rs.Count();
            foreach (Cau_PortalPro_VResponsable re1 in rs)
            {
                numreg++;
                // Buscamos si esa empresa existe
                Responsable re2 = (from e2 in ctx.Responsables
                                where e2.CodAx == re1.ID
                                select e2).FirstOrDefault<Responsable>();
                if (re2 == null)
                {
                    re2 = new Responsable();
                    re2.Nombre = re1.NAME;
                    re2.CodAx = re1.ID;
                    re2.Email = re1.EMAIL;
                    ctx.Add(re2);
                }
                else
                {
                    re2.Nombre = re1.NAME;
                    re2.CodAx = re1.ID;
                    re2.Email = re1.EMAIL;
                }
                // Actualizar los registros de proceso
                Progresos progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 5
                                      select p).FirstOrDefault<Progresos>();
                if (progreso != null)
                {
                    progreso.NumReg = numreg;
                    progreso.TotReg = totreg;
                    ctx.SaveChanges();
                }
            }
            ctx.Dispose();
            con.Dispose();
            return "";
        }
    }
    public delegate string AsyncLaunchResponsable(out int threadId);
}