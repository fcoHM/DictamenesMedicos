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
                            Descripcion = reader["DescripcionUbicacion"] != DBNull.Value ? reader["DescripcionUbicacion"].ToString() : null,
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
