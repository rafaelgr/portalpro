﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class UploadsController : ApiController
    {
       
        /// <summary>
        /// Se utiliza par subir ficheros al servidor
        /// el nombre completo del fichero a guardar se monta encadenando 
        /// tk#tipo#nombre_del_fichero
        /// Esta operación se realiza sobre el directorio de carga, habitualemnte 
        /// \App_Data\Uploads
        /// </summary>
        /// <param name="tk">Tique de autorizacion o el valor "solicitud" (ver Login). Admite el caso "solicitud"</param>
        /// <param name="tipo">Tipo del fichero subido según la acción TiposDocumentos</param>
        /// <returns></returns>
        public bool PostFile(string tk, string tipo)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk!= "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Carga de ficheros)"));
                }
            }

            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(o =>
            {
                FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);
                string fichero = provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");
                fichero = String.Format("{0}#{1}#{2}", tk, tipo, fichero);
                string destino = Path.Combine(root, fichero);
                //File.Copy(finfo.FullName, destino, true);
                //File.Delete(finfo.FullName);
                File.Move(finfo.FullName, destino);
                return new HttpResponseMessage()
                {
                    Content = new StringContent("")
                };
            });
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            return true;
        }

        /// <summary>
        /// Elimina el fichero con ell nombre pasado en una variable en el body
        /// el nombre completo del fichero a borrar se monta encadenando 
        /// tk#tipo#nombre_del_fichero
        /// Esta operación se realiza sobre el directorio de carga, habitualemnte 
        /// \App_Data\Uploads
        /// </summary>
        /// <param name="tk">Tique de autorización, admite "solicitud"</param>
        /// <param name="tipo">Tipo del fichero a borra</param>
        /// <returns></returns>
        public bool DeleteFile(string tk, string tipo)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Carga de ficheros)"));
                }
            }

            HttpRequestMessage request = this.Request;
            string str = HttpUtility.UrlDecode(request.Content.ReadAsStringAsync().Result);
            int pos = str.IndexOf("=");
            string fichero = str.Substring(pos + 1);
            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            fichero = String.Format("{0}#{1}#{2}", tk, tipo, fichero);
            string destino = Path.Combine(root, fichero);
            File.Delete(destino);
            // This access control necessary for Autoupload (kendo UI)
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            return true;
        }
        /// <summary>
        /// Borra todos los ficheros del directorio de descargas que se han 
        /// cargado con el tique pasado
        /// </summary>
        /// <param name="tk">Tique usado</param>
        /// <returns></returns>
        public bool DeleteFiles(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx) && tk != "solicitud")
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Carga de ficheros)"));
                }
            }
            // This access control necessary for Autoupload (kendo UI)
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            PortalProWebUtility.BorrarDocumentos(tk);
            return true;
        }
        public void PostFile()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(o =>
            {
                FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);
                string fichero = provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");
                fichero = String.Format("PRUEBAS#{0}",fichero);
                string destino = Path.Combine(root, fichero);
                //File.Copy(finfo.FullName, destino, true);
                //File.Delete(finfo.FullName);
                File.Move(finfo.FullName, destino);
                return new HttpResponseMessage()
                {
                    Content = new StringContent("")
                };
            });
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
        }
    }
}
