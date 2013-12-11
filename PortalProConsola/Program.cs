using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PortalProModelo;

namespace PortalProConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            // abrir conexiones 
            PortalProContext ctx = new PortalProContext();
            string strConnect = ConfigurationManager.ConnectionStrings["PortalProTestConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnect);
            con.Open();
            
            Console.WriteLine("Cargar empresas --------------");
            CargarEmpresas(ctx, con);
            Console.ReadLine();

            Console.WriteLine("Cargar poveedores --------------");
            CargarProveedores(ctx, con);
            Console.ReadLine();
        }

        static void CargarEmpresas(PortalProContext ctx, SqlConnection con)
        {
            string sql = "SELECT *  FROM [PortalProTest].[dbo].[Cau_PortalPro_VEmpresas]";
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string idEmpresa = reader["IDEMPRESA"].ToString();
                string nombre = reader["NOMBRE"].ToString();
                Console.WriteLine("ID:{0} N:{1}", idEmpresa, nombre);
            }
            reader.Close();
        }

        static void CargarProveedores(PortalProContext ctx, SqlConnection con)
        {
            // first retrive number of rows
            string sql = "SELECT COUNT(*) FROM [PortalProTest].[dbo].[Cau_PortalPro_VProveedores]";
            SqlCommand cmd = new SqlCommand(sql, con);
            int totreg = (int)cmd.ExecuteScalar();
            int numreg = 0;
            sql = @"SELECT  
                        [ACCOUNTNUM]
                        ,[NAME]
                        ,[ADDRESS]
                        ,[CITY]
                        ,[ZIPCODE]
                        ,[COUNTRYREGIONID]
                        ,[PHONE]
                        ,[TELEFAX]
                        ,[CELLULARPHONE]
                        ,[EMAIL]
                        ,[VATNUM]
                        ,[CONTACTO]
                        ,[LINEOFBUSINESSID]
                        ,[CAUPORTALPROEMAIL]
                        ,[BANKIBAN]
                        ,[CAUPORTALPROALLOWINVOICE]
                    FROM [PortalProTest].[dbo].[Cau_PortalPro_VProveedores]";
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                numreg++;
                string accountnum = dr.GetString(0);
                string name = dr.GetString(1);
                Console.WriteLine("ID:{0} N:{1} {2} de {3}", accountnum, name, numreg, totreg);
            }
            dr.Close();
        }
    }
}