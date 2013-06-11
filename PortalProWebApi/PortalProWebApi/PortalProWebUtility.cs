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
        /// Comprueba que los ficheros necesarios para rellenar al solicitud están.
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
                d.NomFichero = String.Format("{0}_{1}_{2}",solProveedor.Nif,td.TipoDocumentoId, fichero.Substring(posFich+1));
                d.Extension = fichero.Substring(posExten + 1);
                // copiamos al repositorio
                File.Copy(fichero, Path.Combine(repo,d.NomFichero));
                ctx.Add(d);
            }
            // si llegamos aquí podemos borrar los ficheros del directorio de carga
            var rs = (from f in listaFicheros
                      where f.Contains(String.Format("{0}#",tk))
                      select f);
            foreach( string f in rs){
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
        public static string ComprobarCargarFicherosSolicitudProveedor(string webRoot,  SolicitudProveedor solProveedor, PortalProContext ctx)
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
    }
}