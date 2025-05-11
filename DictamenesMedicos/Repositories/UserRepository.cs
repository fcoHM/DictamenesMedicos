using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DictamenesMedicos.Model;

namespace DictamenesMedicos.Repositories
{
    public class UserRepository: RepositoryBase
    {

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Paciente] where NSS= @username and [Passwd] = @password";
                command.Parameters.Add("@username", System.Data.SqlDbType.Char).Value = credential.UserName;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void AddPaciente(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = @"
            INSERT INTO Paciente (
                Id, Nombre, NSS, Passwd, ApellidoPaterno, ApellidoMaterno,
                Sexo, FechaNacimiento, TelefonoFijo, TelefonoMovil,
                CorreoElectronico, CodigoPostal, Estado, Municipio,
                Localidad, Calle, NumeroExterior, NumeroInterior,
                DescripcionUbicacion, TipoSangre, EnfermedadesCronicas, Alergias
            )
            VALUES (
                @Id, @Nombre, @NSS, @Passwd, @ApellidoPaterno, @ApellidoMaterno,
                @Sexo, @FechaNacimiento, @TelefonoFijo, @TelefonoMovil,
                @CorreoElectronico, @CodigoPostal, @Estado, @Municipio,
                @Localidad, @Calle, @NumeroExterior, @NumeroInterior,
                @DescripcionUbicacion, @TipoSangre, @EnfermedadesCronicas, @Alergias
            )";

                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Nombre", userModel.Nombre);
                command.Parameters.AddWithValue("@NSS", userModel.NSS);
                command.Parameters.AddWithValue("@Passwd", userModel.Password);
                command.Parameters.AddWithValue("@ApellidoPaterno", userModel.ApellidoPaterno);
                command.Parameters.AddWithValue("@ApellidoMaterno", (object)userModel.ApellidoMaterno ?? DBNull.Value);
                command.Parameters.AddWithValue("@Sexo", userModel.Sexo);
                command.Parameters.AddWithValue("@FechaNacimiento", userModel.FechaNacimiento);
                command.Parameters.AddWithValue("@TelefonoFijo", (object)userModel.TelefonoFijo ?? DBNull.Value);
                command.Parameters.AddWithValue("@TelefonoMovil", (object)userModel.TelefonoMovil ?? DBNull.Value);
                command.Parameters.AddWithValue("@CorreoElectronico", userModel.CorreoElectronico);
                command.Parameters.AddWithValue("@CodigoPostal", userModel.CodigoPostal);
                command.Parameters.AddWithValue("@Estado", userModel.Estado);
                command.Parameters.AddWithValue("@Municipio", userModel.Municipio);
                command.Parameters.AddWithValue("@Localidad", userModel.Localidad);
                command.Parameters.AddWithValue("@Calle", userModel.Calle);
                command.Parameters.AddWithValue("@NumeroExterior", userModel.NumeroExterior);
                command.Parameters.AddWithValue("@NumeroInterior", (object)userModel.NumeroInterior ?? DBNull.Value);
                command.Parameters.AddWithValue("@DescripcionUbicacion", (object)userModel.DescripcionUbicacion ?? DBNull.Value);
                command.Parameters.AddWithValue("@TipoSangre", userModel.TipoSangre);
                command.Parameters.AddWithValue("@EnfermedadesCronicas", (object)userModel.EnfermedadesCronicas ?? DBNull.Value);
                command.Parameters.AddWithValue("@Alergias", (object)userModel.Alergias ?? DBNull.Value);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public UserModel GetByNSS(string nss)
        {
            UserModel user = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Paciente WHERE NSS = @NSS";
                command.Parameters.AddWithValue("@NSS", nss);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["Id"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            NSS = reader["NSS"].ToString(),
                            Password = reader["Passwd"].ToString(),
                            ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            ApellidoMaterno = reader["ApellidoMaterno"] != DBNull.Value ? reader["ApellidoMaterno"].ToString() : null,
                            Sexo = reader["Sexo"] != DBNull.Value ? Convert.ToInt32(reader["Sexo"]) : 0,
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            TelefonoFijo = reader["TelefonoFijo"] != DBNull.Value ? reader["TelefonoFijo"].ToString() : null,
                            TelefonoMovil = reader["TelefonoMovil"] != DBNull.Value ? reader["TelefonoMovil"].ToString() : null,
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            CodigoPostal = reader["CodigoPostal"].ToString(),
                            Estado = reader["Estado"].ToString(),
                            Municipio = reader["Municipio"].ToString(),
                            Localidad = reader["Localidad"].ToString(),
                            Calle = reader["Calle"].ToString(),
                            NumeroExterior = reader["NumeroExterior"].ToString(),
                            NumeroInterior = reader["NumeroInterior"] != DBNull.Value ? reader["NumeroInterior"].ToString() : null,
                            DescripcionUbicacion = reader["DescripcionUbicacion"] != DBNull.Value ? reader["DescripcionUbicacion"].ToString() : null,
                            TipoSangre = reader["TipoSangre"] != DBNull.Value ? Convert.ToInt32(reader["TipoSangre"]) : 0,
                            EnfermedadesCronicas = reader["EnfermedadesCronicas"] != DBNull.Value ? reader["EnfermedadesCronicas"].ToString() : null,
                            Alergias = reader["Alergias"] != DBNull.Value ? reader["Alergias"].ToString() : null
                        };
                    }
                }

                connection.Close();
            }

            return user;
        }

    }
}
