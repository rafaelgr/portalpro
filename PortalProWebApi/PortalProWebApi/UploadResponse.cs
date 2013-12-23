using System;

namespace PortalProWebApi
{
    public class UploadResponse
    {
        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        private string item;

        public string Item
        {
            get { return item; }
            set { item = value; }
        }
        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
