using System;
using System.Configuration;

namespace PortalProWebApi
{
    public class EmailConfig
    {
        public EmailConfig()
        {
            // Leemos los valores del archivo Web.config
            // Sección  <appSettings>
            this.Server = ConfigurationManager.AppSettings["mail_server"];
            this.Port = int.Parse(ConfigurationManager.AppSettings["mail_port"]);
            this.Address = ConfigurationManager.AppSettings["mail_address"];
            this.AddressCc = ConfigurationManager.AppSettings["mail_address_cc"];
            this.Usr = ConfigurationManager.AppSettings["mail_usr"];
            this.Password = ConfigurationManager.AppSettings["mail_pass"];
            this.UseSsl = bool.Parse(ConfigurationManager.AppSettings["mail_ssl"]);
        }
        /// <summary>
        /// Servidor smtp
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Puerto de smtp
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Dirección from del correo
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Copia por defecto a esta dirección
        /// </summary>
        public string AddressCc { get; set; }
        /// <summary>
        /// Autenticación (Usuario)
        /// </summary>
        public string Usr { get; set; }
        /// <summary>
        /// Autenticación (Password)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Usa SSL?
        /// </summary>
        public bool UseSsl { get; set; }
    }
}
