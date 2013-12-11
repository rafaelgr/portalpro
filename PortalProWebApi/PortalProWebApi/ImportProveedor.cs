using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PortalProModelo;
using PortalProAxapta;

namespace PortalProWebApi
{
    public class ImportProveedor
    {
        // este método se ejecutará de manera asíncrona.
        public string LaunchProveedor(out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            int numreg = 0;
            int totreg = 0; 
            try
            {
                PortalProContext ctx = new PortalProContext();
                EntitiesModel con = new EntitiesModel();
                var rs = (from p in con.Cau_PortalPro_VProveedores
                          select p);
                totreg = rs.Count();
                foreach (Cau_PortalPro_VProveedore p1 in rs)
                {
                    numreg++;
                    // Buscamos si esa empresa existe
                    Proveedor pr2 = (from p2 in ctx.Proveedors
                                     where p2.CodAx == p1.ACCOUNTNUM
                                     select p2).FirstOrDefault<Proveedor>();
                    if (pr2 == null)
                    {
                        pr2 = new Proveedor();
                        pr2.CodAx = p1.ACCOUNTNUM;
                        pr2.NombreComercial = p1.NAME;
                        pr2.RazonSocial = p1.NAME;
                        pr2.Direccion = p1.ADDRESS;
                        pr2.Localidad = p1.CITY;
                        pr2.CodPostal = p1.ZIPCODE;
                        pr2.Pais = p1.COUNTRYREGIONID;
                        pr2.Nif = p1.VATNUM;
                        pr2.PersonaContacto = p1.CONTACTO;
                        pr2.Telefono = p1.PHONE;
                        pr2.Fax = p1.TELEFAX;
                        pr2.Movil = p1.CELLULARPHONE;
                        pr2.Email = p1.EMAIL;
                        pr2.EmailFacturas = p1.CAUPORTALPROEMAIL;
                        pr2.IBAN = p1.BANKIBAN;
                        ctx.Add(pr2);
                    }
                    else
                    {
                        pr2.NombreComercial = p1.NAME;
                        pr2.RazonSocial = p1.NAME;
                        pr2.Direccion = p1.ADDRESS;
                        pr2.Localidad = p1.CITY;
                        pr2.CodPostal = p1.ZIPCODE;
                        pr2.Pais = p1.COUNTRYREGIONID;
                        pr2.Nif = p1.VATNUM;
                        pr2.PersonaContacto = p1.CONTACTO;
                        pr2.Telefono = p1.PHONE;
                        pr2.Fax = p1.TELEFAX;
                        pr2.Movil = p1.CELLULARPHONE;
                        pr2.Email = p1.EMAIL;
                        pr2.EmailFacturas = p1.CAUPORTALPROEMAIL;
                        pr2.IBAN = p1.BANKIBAN;
                    }
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
                ctx.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }

    public delegate string AsyncLaunchProveedor(out int threadId);
}