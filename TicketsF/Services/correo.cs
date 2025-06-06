using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TicketsF.Models;
using TicketsF.Controllers;

namespace TicketsF.Services
{
    public class correo
    {
        private IConfiguration _configuration;

        public correo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void enviar(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("TicketsDbConnection").Value;

                string sqlQuery = "EXEC msdb.dbo.sp_send_dbmail " +
                                  "@profile_name = 'SQLMail', " +
                                  "@recipients = @par_destinatarios, " +
                                  "@subject = @par_asunto, " +
                                  "@body = @par_mensaje";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@par_destinatarios", destinatario);
                        command.Parameters.AddWithValue("@par_asunto", asunto);
                        command.Parameters.AddWithValue("@par_mensaje", cuerpo);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}