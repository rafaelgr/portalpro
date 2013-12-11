using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;

namespace PortalProWebApi
{
    public class ImportEmpresa
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchEmpresa(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
                        int numreg = 0;
            int totreg = 0;
            PortalProContext ctx = new PortalProContext();
            EntitiesModel con = new EntitiesModel();
            var rs = (from e in con.Cau_PortalPro_VEmpresas
                      select e);
            totreg = rs.Count();
            foreach (Cau_PortalPro_VEmpresa emp1 in rs)
            {
                numreg++;
                // Buscamos si esa empresa existe
                Empresa emp2 = (from e2 in ctx.Empresas
                                where e2.CodAx == emp1.IDEMPRESA
                                select e2).FirstOrDefault<Empresa>();
                if (emp2 == null)
                {
                    emp2 = new Empresa();
                    emp2.Nombre = emp1.NOMBRE;
                    emp2.CodAx = emp1.IDEMPRESA;
                    ctx.Add(emp2);
                }
                else
                {
                    emp2.Nombre = emp1.NOMBRE;
                }
                // Actualizar los registros de proceso
                Progresos progreso = (from p in ctx.Progresos
                                      where p.ProgresoId == 1
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
    public delegate string AsyncLaunchEmpresa(out int threadId);
}