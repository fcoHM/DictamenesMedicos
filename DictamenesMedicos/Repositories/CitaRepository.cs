using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictamenesMedicos.Model;



namespace DictamenesMedicos.Repositories
{
    public class CitaRepository:RepositoryBase
    {
        public CitaModel getCitaById(String Id)
        {
            CitaModel cita = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Cita WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cita = new CitaModel()
                        {
                            Id = reader["Id"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            correoElectronico = reader["CorreoElectronico"].ToString(),
                            fechaCita = reader["FechaCita"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaCita"]),
                            IdTipoExamen = reader["IdTipoExamen"].ToString(),
                            IdPaciente = reader["IdPaciente"].ToString(),
                            resultado = reader["result"].ToString(),
                        };
                    }
                }

                connection.Close();
            }
            return cita;
        }



        public bool GuardarCita(CitaModel cita)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    if (connection.State != ConnectionState.Open)
                    {
                        Debug.WriteLine("Error: No se pudo establecer conexión con la base de datos");
                        return false;
                    }

                    if (string.IsNullOrEmpty(cita.IdPaciente) ||
                        string.IsNullOrEmpty(cita.IdTipoExamen) ||
                        cita.fechaCita == DateTime.MinValue)
                    {
                        Debug.WriteLine("Error: Datos obligatorios faltantes");
                        return false;
                    }

                    cita.Id = string.IsNullOrEmpty(cita.Id) ? Guid.NewGuid().ToString() : cita.Id;
                    cita.resultado = string.IsNullOrEmpty(cita.resultado) ? "Pendiente" : cita.resultado;
                    cita.Telefono = cita.Telefono ?? string.Empty;
                    cita.correoElectronico = cita.correoElectronico ?? string.Empty;

                    string query = @"
            IF EXISTS (SELECT 1 FROM Cita WHERE Id = @Id)
            BEGIN
                UPDATE Cita SET 
                    Telefono = @Telefono,
                    CorreoElectronico = @CorreoElectronico,
                    FechaCita = @FechaCita,
                    IdTipoExamen = @IdTipoExamen,
                    IdPaciente = @IdPaciente,
                    Resultado = @Resultado
                WHERE Id = @Id
            END
            ELSE
            BEGIN
                INSERT INTO Cita 
                    (Id, Telefono, CorreoElectronico, FechaCita, IdTipoExamen, IdPaciente, Resultado)
                VALUES 
                    (@Id, @Telefono, @CorreoElectronico, @FechaCita, @IdTipoExamen, @IdPaciente, @Resultado)
            END";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", cita.Id);
                        command.Parameters.AddWithValue("@Telefono", cita.Telefono);
                        command.Parameters.AddWithValue("@CorreoElectronico", cita.correoElectronico);
                        command.Parameters.AddWithValue("@FechaCita", cita.fechaCita);
                        command.Parameters.AddWithValue("@IdTipoExamen", cita.IdTipoExamen);
                        command.Parameters.AddWithValue("@IdPaciente", cita.IdPaciente);
                        command.Parameters.AddWithValue("@Resultado", cita.resultado);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            Debug.WriteLine("Advertencia: No se modificó ninguna fila");
                            return false;
                        }

                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine($"Error SQL #{ex.Number}: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error general: {ex.GetType().Name}: {ex.Message}");
                    return false;
                }
            }
        }




    }
}
