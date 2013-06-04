using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PortalProModelo;

namespace PortalProWebApi.Controllers
{
    public class UploadsController : ApiController
    {
        public Task<HttpResponseMessage> PostFile()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(o =>
            {
                FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

                string guid = Guid.NewGuid().ToString();

                File.Copy(finfo.FullName, Path.Combine(root, provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "")), true);
                File.Delete(finfo.FullName);

                return new HttpResponseMessage()
                {
                    Content = new StringContent("")
                };
            });
            return task;
        }
        /// <summary>
        /// Se utiliza par subir ficheros al servidor
        /// </summary>
        /// <param name="tk">Tique de autorizacion (ver Login)</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostFile(string tk)
        {
            using (PortalProContext ctx = new PortalProContext())
            {
                if (!CntWebApiSeguridad.CheckTicket(tk, ctx))
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Se necesita tique de autorización (Carga de ficheros)"));
                }
            }

            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(o =>
            {
                FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

                File.Move(finfo.FullName, Path.Combine(root, provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "")));
                return new HttpResponseMessage()
                {
                    Content = new StringContent("")
                };
            });
            return task;
        }

    }
}
