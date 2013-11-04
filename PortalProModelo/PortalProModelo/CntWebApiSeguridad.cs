using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PortalProModelo
{
    public static partial class CntWebApiSeguridad
    {
        #region Métodos criptográficos
        public static string GetHashCode(string password)
        {
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tmpHash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
        #endregion Métodos criprográficos

        #region Manejo de tiques
        public static WebApiTicket Login(string login, string password, int minutes, PortalProContext ctx)
        {
            WebApiTicket tk = null;
            // Primero comprobar si exite un usuario con ese login y contraseña
            Usuario usuario = (from u in ctx.Usuarios
                         where u.Login == login
                         select u).FirstOrDefault<Usuario>();
            if (usuario != null)
            {
                // User exists. Does the password match?
                if (usuario.Password == GetHashCode(password))
                {
                    // Go to get the ticket
                    string code = GenerateTicket();
                    tk = new WebApiTicket()
                    {
                        Codigo = code,
                        Inicio = DateTime.Now,
                        Usuario = usuario
                    };
                    tk.Fin = tk.Inicio.AddMinutes(minutes);
                }
            }
            return tk;
        }

        public static WebApiTicket LoginProveedor(string login, string password, int minutes, PortalProContext ctx)
        {
            WebApiTicket tk = null;
            // Primero comprobar si exite un usuario con ese login y contraseña
            UsuarioProveedor usuario = (from u in ctx.UsuarioProveedors
                               where u.Login == login
                               select u).FirstOrDefault<UsuarioProveedor>();
            if (usuario != null)
            {
                // User exists. Does the password match?
                if (usuario.Password == GetHashCode(password))
                {
                    // Go to get the ticket
                    string code = GenerateTicket();
                    tk = new WebApiTicket()
                    {
                        Codigo = code,
                        Inicio = DateTime.Now,
                        UsuarioProveedor = usuario
                    };
                    tk.Fin = tk.Inicio.AddMinutes(minutes);
                }
            }
            return tk;
        }

        
        
        /// <summary>
        /// Comprueba si hay un tique activo con un código
        /// </summary>
        /// <param name="code">Código del tique a verificar</param>
        /// <param name="ctx">Contexto de acceso a datos</param>
        /// <returns></returns>
        public static bool CheckTicket(string code, PortalProContext ctx)
        {
            // Tiempo actual
            DateTime curtime = DateTime.Now;
            // Buscar un tique con ese código y activo
            WebApiTicket tk = (from t in ctx.WebApiTickets
                               where t.Codigo == code
                               && t.Fin > curtime
                               select t).FirstOrDefault<WebApiTicket>();
            if (tk != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Genera una cadena aleatoria 
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Devuelve un número aleatorio entre el máximo y 
        /// el mínimo dado
        /// </summary>
        /// <param name="min">Límite inferior</param>
        /// <param name="max">Límite superio</param>
        /// <returns>Número aleatorio</returns>
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Genera una cadena con codigo aleatorio
        /// </summary>
        /// <returns></returns>
        public static string GenerateTicket()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, false));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        #endregion
    }
}
