﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalProModelo;
using System.Configuration;
using System.IO;

namespace PortalProWebApi
{
    public static partial class PortalProWebUtility
    {
        /// <summary>
        /// Comprueba que los ficheros necesarios para rellenar a la solicitud están.
        /// Asocia esos ficheros como documentos a la solicitud de que se trate.
        /// </summary>
        /// <param name="webRoot">Directorio de subida de ficheros</param>
        /// <param name="tk">Tique de autorización (caso especial "solicitud")</param>
        /// <param name="solProveedor">Solicitud de proveedor a procesar</param>
        /// <param name="ctx">Contexto para acceso a la base de datos (OpenAccess)</param>
        /// <returns></returns>
        public static string ComprobarCargarFicherosProveedor(string webRoot, string tk, SolicitudProveedor solProveedor, PortalProContext ctx)
        {
            string mens = ""; // mensaje que devoveremos, si vacío todo OK
            string[] listaFicheros; // lista de los ficheros contenidos en el directorio de carga
            // lo primero de todo que no ha habido un error en el directorio 
            if (!Directory.Exists(webRoot))
            {
                return "No existe el directorio de carga";
            }
            else
            {
                // cargamos la lista de ficheros del directorio de carga
                listaFicheros = Directory.GetFiles(webRoot);
            }
            // comprobamos si existe el parámetro que define donde esta el repositorio
            string repo = ConfigurationManager.AppSettings["PortalProRepositorio"];
            if (repo == null || repo == "")
            {
                return "No existe o está vació el parámetro de ubicación del repositorio en el Web.config";
            }
            // comprobamos que el directorio físicamente existe (si no lo creamos);
            if (!Directory.Exists(repo))
            {
                Directory.CreateDirectory(repo);
            }
            // por cada tipo de fichero asociado al grupo de proveedores al que pertenecerá este
            // comprbamos que existe el fichero correspondiente en el directorio de carga.
            foreach (TipoDocumentoGrupoProveedor tdgp in solProveedor.GrupoProveedor.TipoDocumentoGrupoProveedors)
            {
                TipoDocumento td = tdgp.TipoDocumento;
                string buscador = String.Format("{0}#{1}#", tk, td.TipoDocumentoId);
                string fichero = (from f in listaFicheros
                                  where f.Contains(buscador)
                                  select f).FirstOrDefault<string>();
                if (fichero == null)
                {
                    return "Faltan ficheros asociados a este proveedor";
                }
                // creamos el documento correspondiente asignando un nuevo nombre que empieza por el NIF
                Documento d = new Documento();
                d.TipoDocumento = td;
                d.SolicitudProveedor = solProveedor;
                int posFich = fichero.LastIndexOf("#");
                int posExten = fichero.LastIndexOf(".");
                d.NomFichero = String.Format("{0}_{1}_{2}", solProveedor.Nif, td.TipoDocumentoId, fichero.Substring(posFich + 1));
                d.Extension = fichero.Substring(posExten + 1);
                // copiamos al repositorio
                File.Copy(fichero, Path.Combine(repo, d.NomFichero));
                ctx.Add(d);
            }
            // si llegamos aquí podemos borrar los ficheros del directorio de carga
            var rs = (from f in listaFicheros
                      where f.Contains(String.Format("{0}#", tk))
                      select f);
            foreach (string f in rs)
            {
                File.Delete(f);
            }
            return mens;
        }

        /// <summary>
        /// Comprueba que los ficheros necesarios para asociar a un proveedor están peresentes
        /// Si esa comprobación es superada dá de alta los ficheros asociados.
        /// </summary>
        /// <param name="webRoot">Directorio en el que la carga ha dejado los ficheros</param>
        /// <param name="tk">El código de tique usado en este momento</param>
        /// <param name="solProveedor">Proveedor al que asociarán los ficheros</param>
        /// <param name="ctx">Contexto de OpenAccess usado por el proceso.</param>
        /// <returns>Una cadena vacía en el caso de que todo haya sido correcto y si no el mensaje explicativo</returns>
        public static string ComprobarCargarFicherosProveedor(string webRoot, string tk, Proveedor proveedor, PortalProContext ctx)
        {
            string mens = ""; // mensaje que devoveremos, si vacío todo OK
            string[] listaFicheros; // lista de los ficheros contenidos en el directorio de carga
            // lo primero de todo que no ha habido un error en el directorio 
            if (!Directory.Exists(webRoot))
            {
                return "No existe el directorio de carga";
            }
            else
            {
                // cargamos la lista de ficheros del directorio de carga
                listaFicheros = Directory.GetFiles(webRoot);
            }
            // comprobamos si existe el parámetro que define donde esta el repositorio
            string repo = ConfigurationManager.AppSettings["PortalProRepositorio"];
            if (repo == null || repo == "")
            {
                return "No existe o está vació el parámetro de ubicación del repositorio en el Web.config";
            }
            // comprobamos que el directorio físicamente existe (si no lo creamos);
            if (!Directory.Exists(repo))
            {
                Directory.CreateDirectory(repo);
            }
            // por cada tipo de fichero asociado al grupo de proveedores al que pertenecerá este
            // comprbamos que existe el fichero correspondiente en el directorio de carga.
            foreach (TipoDocumentoGrupoProveedor tdgp in proveedor.GrupoProveedor.TipoDocumentoGrupoProveedors)
            {
                TipoDocumento td = tdgp.TipoDocumento;
                string buscador = String.Format("{0}#{1}#", tk, td.TipoDocumentoId);
                string fichero = (from f in listaFicheros
                                  where f.Contains(buscador)
                                  select f).FirstOrDefault<string>();
                if (fichero == null)
                {
                    return "Faltan ficheros asociados a este proveedor";
                }
                // creamos el documento correspondiente asignando un nuevo nombre que empieza por el NIF
                Documento d = new Documento();
                d.TipoDocumento = td;
                d.Proveedor = proveedor;
                int posFich = fichero.LastIndexOf("#");
                int posExten = fichero.LastIndexOf(".");
                d.NomFichero = String.Format("{0}_{1}_{2}", proveedor.Nif, td.TipoDocumentoId, fichero.Substring(posFich + 1));
                d.Extension = fichero.Substring(posExten + 1);
                // copiamos al repositorio
                File.Copy(fichero, Path.Combine(repo, d.NomFichero));
                ctx.Add(d);
            }
            // si llegamos aquí podemos borrar los ficheros del directorio de carga
            var rs = (from f in listaFicheros
                      where f.Contains(String.Format("{0}#", tk))
                      select f);
            foreach (string f in rs)
            {
                File.Delete(f);
            }
            return mens;
        }

        /// <summary>
        /// Comprueba que los ficheros necesarios para asociar a una solicitud de proveedor están peresentes
        /// Si esa comprobación es superada dá de alta los ficheros asociados.
        /// </summary>
        /// <param name="webRoot">Directorio en el que la carga ha dejado los ficheros</param>
        /// <param name="tk">El código de tique usado en este momento</param>
        /// <param name="solProveedor">Proveedor al que asociarán los ficheros</param>
        /// <param name="ctx">Contexto de OpenAccess usado por el proceso.</param>
        /// <returns>Una cadena vacía en el caso de que todo haya sido correcto y si no el mensaje explicativo</returns>
        public static string ComprobarCargarFicherosSolicitudProveedor(string webRoot, SolicitudProveedor solProveedor, PortalProContext ctx)
        {
            string mens = ""; // mensaje que devoveremos, si vacío todo OK
            string tk = "solicitud"; 
            string[] listaFicheros; // lista de los ficheros contenidos en el directorio de carga
            // lo primero de todo que no ha habido un error en el directorio 
            if (!Directory.Exists(webRoot))
            {
                return "No existe el directorio de carga";
            }
            else
            {
                // cargamos la lista de ficheros del directorio de carga
                listaFicheros = Directory.GetFiles(webRoot);
            }
            // comprobamos si existe el parámetro que define donde esta el repositorio
            string repo = ConfigurationManager.AppSettings["PortalProRepositorio"];
            if (repo == null || repo == "")
            {
                return "No existe o está vació el parámetro de ubicación del repositorio en el Web.config";
            }
            // comprobamos que el directorio físicamente existe (si no lo creamos);
            if (!Directory.Exists(repo))
            {
                Directory.CreateDirectory(repo);
            }
            // por cada tipo de fichero asociado al grupo de proveedores al que pertenecerá este
            // comprbamos que existe el fichero correspondiente en el directorio de carga.
            foreach (TipoDocumentoGrupoProveedor tdgp in solProveedor.GrupoProveedor.TipoDocumentoGrupoProveedors)
            {
                TipoDocumento td = tdgp.TipoDocumento;
                string buscador = String.Format("{0}#{1}#", tk, td.TipoDocumentoId);
                string fichero = (from f in listaFicheros
                                  where f.Contains(buscador)
                                  select f).FirstOrDefault<string>();
                if (fichero == null)
                {
                    return "Faltan ficheros asociados a este proveedor";
                }
                // creamos el documento correspondiente asignando un nuevo nombre que empieza por el NIF
                Documento d = new Documento();
                d.TipoDocumento = td;
                d.SolicitudProveedor = solProveedor;
                int posFich = fichero.LastIndexOf("#");
                int posExten = fichero.LastIndexOf(".");
                d.NomFichero = String.Format("{0}_{1}_{2}", solProveedor.Nif, td.TipoDocumentoId, fichero.Substring(posFich + 1));
                d.Extension = fichero.Substring(posExten + 1);
                // copiamos al repositorio
                File.Copy(fichero, Path.Combine(repo, d.NomFichero));
                ctx.Add(d);
            }
            // si llegamos aquí podemos borrar los ficheros del directorio de carga
            var rs = (from f in listaFicheros
                      where f.Contains(String.Format("{0}#", tk))
                      select f);
            foreach (string f in rs)
            {
                File.Delete(f);
            }
            return mens;
        }

        public static string ObtenerUrlDeDocumento(string webRoot, string tk, Documento doc, PortalProContext ctx)
        {
            string mens = ""; // mensaje que devoveremos, si vacío todo OK
            string[] listaFicheros; // lista de los ficheros contenidos en el directorio de carga
            // lo primero de todo que no ha habido un error en el directorio 
            if (!Directory.Exists(webRoot))
            {
                return "No existe el directorio de carga";
            }
            // Hay que copiar los ficheros al directorio de carga para obtener la url
            // leer de parámetros la ubicación del repositorio.
            string repo = ConfigurationManager.AppSettings["PortalProRepositorio"];
            if (repo == null || repo == "")
            {
                return "No existe o está vació el parámetro de ubicación del repositorio en el Web.config";
            }
            // a partir de la información del documento copiar al directorio de descarga el 
            // fichero en cuestión, identificado con el ticket y el tipo.
            // En el propio objeto documento se monta la url
            string fichero = Path.Combine(repo, doc.NomFichero);
            string destino = Path.Combine(webRoot, String.Format("{0}_{1}", tk, doc.NomFichero));
            File.Copy(fichero, destino, true);
            doc.DescargaUrl = "/downloads/" + String.Format("{0}_{1}", tk, doc.NomFichero);
            return mens;
        }

        public static string BuscarArchivoCargado(string application, string userId, string formId, string fieldId)
        {
            string archivo = "";
            string pathFileName = ""; // aqui se montará el camino de búsqueda
            string searchFileName = ""; // nombre con wildcards para búsqueda
            // leemos el directorio en el que se encuentran las cargas
            string directorio = ConfigurationManager.AppSettings["UploadRepository"];
            switch (application)
            {
                case "PortalPro":
                    directorio = ConfigurationManager.AppSettings["UploadRepository"];
                    break;
                case "PortalPro2":
                    directorio = ConfigurationManager.AppSettings["UploadRepository2"];
                    break;
            }

            pathFileName = directorio;
            searchFileName = String.Format("{0}-{1}-{2}-{3}-*", application, userId, formId, fieldId);
            string[] archivos = Directory.GetFiles(pathFileName, searchFileName);
            // si encontramos algún coincidente devolvemos el primero.
            if (archivos.Length > 0)
                archivo = archivos[0];
            // si contiene un path lo eliminamos (solo el nombre final de fcihero)
            int pos = archivo.LastIndexOf("\\");
            if (pos > -1)
                archivo = archivo.Substring(pos + 1);
            return archivo;
        }

        public static Documento CrearDocumentoDesdeArchivoCargado(string application, string fichero, PortalProContext ctx)
        {
            Documento d = null;
            // siempre creamos un nuevo documento
            string portalProRepo = ConfigurationManager.AppSettings["PortalProRepository"];
            string uploadRepo = ConfigurationManager.AppSettings["UploadRepository"];
            switch (application)
            {
                case "PortalPro":
                    uploadRepo = ConfigurationManager.AppSettings["UploadRepository"];
                    break;
                case "PortalPro2":
                    uploadRepo = ConfigurationManager.AppSettings["UploadRepository2"];
                    break;
            }
            d = new Documento();
            int pos = fichero.LastIndexOf(".");
            if (pos > -1)
                d.Extension = fichero.Substring(pos + 1);
            pos = fichero.LastIndexOf("-");
            if (pos > 0)
                d.NomFichero = fichero.Substring(pos + 1);
            ctx.Add(d);
            ctx.SaveChanges();
            string nFichero = String.Format("{0:000000}-{1}", d.DocumentoId, d.NomFichero);
            string origen = Path.Combine(uploadRepo, fichero);
            string destino = Path.Combine(portalProRepo, nFichero);
            File.Move(origen, destino);
            ctx.SaveChanges();
            return d;
        }

        public static void EliminarDocumento(Documento d, PortalProContext ctx)
        {
            if (d == null)
                return;
            // eliminamos el fichero físico asociado
            string portalProRepo = ConfigurationManager.AppSettings["PortalProRepository"];
            string fichero = String.Format("{0:000000}-{1}", d.DocumentoId, d.NomFichero);
            string ficheroCompleto = Path.Combine(portalProRepo, fichero);
            ctx.Delete(d);
            File.Delete(ficheroCompleto);
            ctx.SaveChanges();
        }

        public static string CargarUrlDocumento(string application, Documento d, string tk)
        {
            if (d == null)
                return "";
            string url = "";
            // copiamos el fichero del repositiorio al directorio de descargas
            string portalProRepo = ConfigurationManager.AppSettings["PortalProRepository"];
            string downloadRepository = ConfigurationManager.AppSettings["DownloadRepository"];
            switch (application)
            {
                case "PortalPro":
                    downloadRepository = ConfigurationManager.AppSettings["DownloadRepository"];
                    break;
                case "PortalPro2":
                    downloadRepository = ConfigurationManager.AppSettings["DownloadRepository2"];
                    break;
            }
            string fichero = String.Format("{0:000000}-{1}", d.DocumentoId, d.NomFichero);
            string origen = Path.Combine(portalProRepo, fichero);
            string destino = Path.Combine(downloadRepository, String.Format("{0}-{1}", tk, fichero));
            url = String.Format("{0}-{1}", tk, fichero);
            File.Copy(origen, destino, true);
            return url;
        }

        public static void BorrarDocumentos(string tk) 
        {
            string downloadRepository = ConfigurationManager.AppSettings["DownloadRepository"];
            foreach (FileInfo f in new DirectoryInfo(downloadRepository).GetFiles(String.Format("{0}*", tk)))
            {
                f.Delete();
            }
        }

        public static CabFactura GenerarFacturaDesdePedido(Pedido p, PortalProContext ctx)
        {
            CabFactura f = new CabFactura()
            {
                CabFacturaId = 0,
                Proveedor = p.Proveedor,
                FechaAlta = DateTime.Now,
                Empresa = p.Empresa,
                Responsable = p.Responsable,
                Estado = "INCIDENCIA"
            };
            ctx.Add(f);
            ctx.SaveChanges();
            decimal totalF = 0;
            if (p.TotalFacturado == 0)
            {
                // caso (1) es el más sencillo, una factura con tantas líneas como tenga el pedido
                foreach (LinPedido lp in p.LinPedidos)
                {
                    ctx.Add(new LinFactura()
                    {
                        LinFacturaId = 0,
                        NumeroPedido = p.NumPedido,
                        Importe = lp.Importe,
                        Descripcion = lp.Descripcion,
                        PorcentajeIva = 0,
                        CabFactura = f
                    });
                    f.TotalFactura += lp.Importe;
                    p.TotalFacturado += lp.Importe;
                    ctx.SaveChanges();
                }
            }
            else
            {
                // caso (2) generaremos una única linea con el resto del pedido
                ctx.Add(new LinFactura()
                {
                    LinFacturaId = 0,
                    NumeroPedido = p.NumPedido,
                    Importe = p.TotalPedido - p.TotalFacturado,
                    Descripcion = "Resto de pedido",
                    PorcentajeIva = 0,
                    CabFactura = f
                });
                f.TotalFactura = p.TotalPedido - p.TotalFacturado;
                p.TotalFacturado = p.TotalPedido;
                ctx.SaveChanges();
            }

            return f;
        }

        public static IList<LinFactura> GenerarLineasFacturaDesdePedido(Pedido p, PortalProContext ctx)
        {
            IList<LinFactura> llfac = new List<LinFactura>();
            // hay dos casos
            // (1) el pedido no ha sido facturado en absoluto p.TotalFacturado = 0
            // (2) el pedido ya ha sido facturado en parte p.TotalFacturado > 0
            if (p.TotalFacturado == 0)
            {
                // caso (1) es el más sencillo, una factura con tantas líneas como tenga el pedido
                foreach (LinPedido lp in p.LinPedidos)
                {
                    llfac.Add(new LinFactura()
                    {
                        LinFacturaId = 0,
                        NumeroPedido = p.NumPedido,
                        Importe = lp.Importe,
                        Descripcion = lp.Descripcion,
                        PorcentajeIva = lp.PorcentajeIva
                    });
                }
            }
            else
            {
                // caso (2) generaremos una única linea con el resto del pedido
                llfac.Add(new LinFactura()
                {
                    LinFacturaId = 0,
                    NumeroPedido = p.NumPedido,
                    Importe = p.TotalPedido - p.TotalFacturado,
                    Descripcion = "Resto de pedido",
                    PorcentajeIva = p.LinPedidos[0].PorcentajeIva
                });
            }
            return llfac;
        }

        public static string ComprobarLineaFacturaContraPedido(CabFactura factura, LinFactura l, PortalProContext ctx)
        {
            string m = "";
            // (1) comprobar que el pedido existe
            Pedido p = (from ped in ctx.Pedidos
                        where ped.NumPedido == l.NumeroPedido
                        select ped).FirstOrDefault<Pedido>();
            if (p == null)
            {
                m = String.Format("El pedido {0} no existe.", l.NumeroPedido);
                return m;
            }
            // ahora se compara contra línea, luego hay que buscar la línea correspondiente
            LinPedido lp = (from linped in ctx.LinPedidos
                            where linped.NumPedido == l.NumeroPedido
                            && linped.NumLinea == l.NumLineaPedido
                            select linped).FirstOrDefault<LinPedido>();
            if (lp == null)
            {
                m = String.Format("El linea {0} del pedido {1} no existe.",l.NumLineaPedido, l.NumeroPedido);
                return m;
            }
            // Obtener parámetros y datos de proveedor para verificar márgenes
            Parametro parametro = (from par in ctx.Parametros1
                                   where par.ParametroId == 1
                                   select par).FirstOrDefault<Parametro>();
            Proveedor proveedor = p.Proveedor;
            //
            bool facAbierto = parametro.FacturaAbierto;
            facAbierto = proveedor.FacAbierto;
            int margen = parametro.MargenFactura;
            if (p.Estado == "FACTURADO" || p.Estado == "CANCELADO")
            {
                m = String.Format("El pedido {0} ya ha sido facturado o está cancelado.", l.NumeroPedido);
                return m;
            }
            if (p.Proveedor.ProveedorId != factura.Proveedor.ProveedorId)
            {
                m = String.Format("El pedido {0} no corresponde al proveedor {1}", l.NumeroPedido, factura.Proveedor.NombreComercial);
                return m;
            }
            // (2) comprobar que el importe que se va a facturar es menor o igual que el del pedido
            if (l.Importe > ((lp.Importe - lp.Facturado) + margen))
            {
                m = String.Format("El importe {0:##,###,##0.00} supera el resto por facturar del pedido {1} linea {2}.", l.Importe, l.NumeroPedido, l.NumLineaPedido);
                return m;
            }
            // el estado por defecto de una factura es ACEPTADA, pero si es de un pedido ABIERTO  deberá ser RECIBIDA
            if (lp.Estado == "ABIERTO")
            {
                if (facAbierto)
                {
                    factura.Estado = "RECIBIDA2";
                    factura.Historial += String.Format("{0:dd/MM/yyyy hh:mm:ss} La factura {1} pasa a estado {3} debido a que el pedido {4} linea {5} no está recibido, se espera a su aceptación manual <br/>",
                        DateTime.Now, factura.NumFactura, factura.TotalFactura, factura.Estado, lp.NumPedido, lp.NumLinea);
                }
                else
                {
                    m = String.Format("El pedido {0} linea {1} no está recibido, no puede emitir la factura contra él", lp.NumPedido, lp.NumLinea);
                    return m;
                }
            }
            // si llega aquí es una linea a dar de alta.
            lp.Facturado = l.Importe;
            factura.TotalFactura += l.Importe;
            p.TotalFacturado += l.Importe;
            // actualizar empresa y responsables
            factura.Empresa = p.Empresa;
            factura.Responsable = p.Responsable;
            l.CabFactura = factura;
            ctx.Add(l);
            ctx.SaveChanges();
            return m;
        }

        public static void EliminarLineasFactura(IList<LinFactura> lineas, PortalProContext ctx)
        {
            foreach (LinFactura l in lineas)
            {
                // antes de eliminar la línea hay que actualizar el total facturado del pedido
                Pedido ped = (from p in ctx.Pedidos
                              where p.NumPedido == l.NumeroPedido
                              select p).FirstOrDefault<Pedido>();
                if (ped != null)
                {
                    ped.TotalFacturado = ped.TotalFacturado - l.Importe;
                    if (ped.TotalFacturado < 0)
                        ped.TotalFacturado = 0;
                }
                // lo mismo para las lineas
                LinPedido lped = (from lp in ctx.LinPedidos
                                  where lp.NumPedido == l.NumeroPedido
                                  && lp.NumLinea == l.NumLineaPedido
                                  select lp).FirstOrDefault<LinPedido>();
                if (lped != null)
                {
                    lped.Facturado = lped.Facturado - l.Importe;
                    if (lped.Facturado < 0) lped.Facturado = 0;
                }
                l.CabFactura.TotalFactura = l.CabFactura.TotalFactura - l.Importe;
                ctx.Delete(l);
            }
            ctx.SaveChanges();
        }

        public static CabFactura YaExisteUnaFacturaComoEsta(CabFactura factura, PortalProContext ctx)
        {
            // obtener el año de la fecha de factura.
            DateTime fechaEmision = (DateTime)factura.FechaEmision;
            int ano = fechaEmision.Year;
            // primer y último día de ese año.
            DateTime primerDia = new DateTime(ano, 1, 1);
            DateTime ultimoDia = new DateTime(ano, 12, 31);
            CabFactura fac = null;
            if (factura.CabFacturaId != 0)
            {
                fac = (from f in ctx.CabFacturas
                       where f.Proveedor.ProveedorId == factura.Proveedor.ProveedorId &&
                             f.NumFactura == factura.NumFactura &&
                             f.FechaEmision >= primerDia &&
                             f.FechaEmision <= ultimoDia &&
                             f.CabFacturaId != factura.CabFacturaId
                       select f).FirstOrDefault<CabFactura>();
            }
            else
            {
                fac = (from f in ctx.CabFacturas
                       where f.Proveedor.ProveedorId == factura.Proveedor.ProveedorId &&
                             f.NumFactura == factura.NumFactura &&
                             f.FechaEmision >= primerDia &&
                             f.FechaEmision <= ultimoDia
                       select f).FirstOrDefault<CabFactura>();
            }
            return fac;
        }

        #region Generación de fcaturas a partir de los pedidos
        public static CabFactura GenerarFacturaDesdePedidos(int[] pedidos, PortalProContext ctx)
        {
            CabFactura factura = null; // provisional
            if (pedidos.Length == 0) return factura; // está vacío el array de ids
            if (!TodosLosPedidosExisten(pedidos, ctx)) return factura; // no existen todos los pedidos de la lista
            // creamos la cabecera de factura
            factura = new CabFactura();
            factura.Generada = true;
            factura.FechaAlta = DateTime.Now;
            // obtenemos el proveedor del primer pedido, en teoría todos son del mismo
            Pedido ped = (from p in ctx.Pedidos
                          where p.PedidoId == pedidos[0]
                          select p).FirstOrDefault<Pedido>();
            factura.Proveedor = ped.Proveedor;
            ctx.Add(factura);
            ctx.SaveChanges();
            CrearLineasFacturaDesdePedidos(factura, pedidos, ctx);
            return factura;
        }
        public static bool TodosLosPedidosExisten(int[] pedidos, PortalProContext ctx)
        {
            bool existenTodos = true; // por defectos existen todos
            // recorremos el array con los id's de pedidos
            for (int i = 0; i < pedidos.Length; i++)
            {
                Pedido ped = (from p in ctx.Pedidos
                              where p.PedidoId == pedidos[i]
                              select p).FirstOrDefault<Pedido>();
                if (ped == null) existenTodos = false;
            }
            return existenTodos;
        }
        public static void CrearLineasFacturaDesdePedidos(CabFactura factura, int[] pedidos, PortalProContext ctx)
        {
            for (int i = 0; i < pedidos.Length; i++)
            {
                Pedido ped = (from p in ctx.Pedidos
                              where p.PedidoId == pedidos[i]
                              select p).FirstOrDefault<Pedido>();
                CrearLineasFacturaDesdeUnPedido(factura, ped, ctx);
            }
        }
        public static void CrearLineasFacturaDesdeUnPedido(CabFactura factura, Pedido pedido, PortalProContext ctx)
        {
            // procesamos secuencialmente las lineas de este pedido.
            foreach (LinPedido lp in pedido.LinPedidos)
            {
                // la primera condición es que quede algo que facturar
                if (lp.Facturado < lp.Importe)
                {
                    decimal importe = lp.Importe - lp.Facturado;
                    LinFactura lf = new LinFactura();
                    lf.CabFactura = factura;
                    lf.Descripcion = lp.Descripcion;
                    lf.Importe = importe;
                    lf.NumeroPedido = lp.NumPedido;
                    lf.NumLineaPedido = lp.NumLinea;
                    factura.TotalFactura += importe;
                    ctx.Add(lf);
                    ctx.SaveChanges();
                }
            }
        }
        #endregion 
    }
}