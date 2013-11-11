using System;
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

        public static Documento CrearDocumentoDesdeArchivoCargado(string fichero, PortalProContext ctx)
        {
            Documento d = null;
            // siempre creamos un nuevo documento
            string portalProRepo = ConfigurationManager.AppSettings["PortalProRepository"];
            string uploadRepo = ConfigurationManager.AppSettings["UploadRepository"];
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

        public static string CargarUrlDocumento(Documento d, string tk)
        {
            if (d == null) return "";
            string url = "";
            // copiamos el fichero del repositiorio al directorio de descargas
            string portalProRepo = ConfigurationManager.AppSettings["PortalProRepository"];
            string downloadRepository = ConfigurationManager.AppSettings["DownloadRepository"];
            string fichero = String.Format("{0:000000}-{1}",d.DocumentoId, d.NomFichero);
            string origen = Path.Combine(portalProRepo, fichero);
            string destino = Path.Combine(downloadRepository,String.Format("{0}-{1}",tk,fichero));
            url = String.Format("{0}-{1}", tk, fichero);
            File.Copy(origen, destino,true);
            return url;
        }
        public static void BorrarDocumentos(string tk) 
        {
            string downloadRepository = ConfigurationManager.AppSettings["DownloadRepository"];
            foreach(FileInfo f in new DirectoryInfo(downloadRepository).GetFiles(String.Format("{0}*", tk)))
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
                FechaEmision = DateTime.Now,
                Estado = "RECIBIDA"
            };
            ctx.Add(f);
            ctx.SaveChanges();
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
                        PorcentajeIva = lp.PorcentajeIva,
                        CabFactura = f
                    });
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
                    PorcentajeIva = p.LinPedidos[0].PorcentajeIva,
                    CabFactura = f
                });
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
                    llfac.Add( new LinFactura()
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
            if (p.Proveedor.ProveedorId != factura.Proveedor.ProveedorId)
            {
                m = String.Format("El pedido {0} no corresponde al proveedor {1}", l.NumeroPedido, factura.Proveedor.NombreComercial);
                return m;
            }
            // (2) comprobar que el importe que se va a facturar es menor o igual que el del pedido
            if (l.Importe > (p.TotalPedido - p.TotalFacturado))
            {
                m = String.Format("El importe {0:##,###,##0.00} supera el resto por facturar del pedido {1}.", l.Importe, l.NumeroPedido);
                return m;
            }
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
                    if (ped.TotalFacturado < 0) ped.TotalFacturado = 0;
                    ctx.Delete(l);
                }
            }
            ctx.SaveChanges();
        }
    }
}