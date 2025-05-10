using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAplication
{
    using System;
    using System.Data.SqlClient;

    public class Conexion
    {
        private string connectionString;

        public Conexion()
        {
            connectionString = "Server=DESKTOP-H08CTKA;Database=loginapp;Integrated Security=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexión abierta exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al abrir la conexión: " + ex.Message);
                }
            }
        }

        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Conexión cerrada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la conexión: " + ex.Message);
            }
        }
    }

}
