using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictamenesMedicos.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        String conexionFrancisco = "Server = Francisco_HM\\VSGESTION; " + "Database=DictamenesMedicos; " + "Integrated Security = true";
        String ConexionFernando = "Server = LAPTOP-8DIT8R4T\\VSGESTION; " + "Database=DictamenesMedicos; " + "Integrated Security = true";
        String ConexionDaniel = "Server = LAPTOP-2NELMMPR\\VSGESTION; " + "Database=DictamenesMedicos; " + "Integrated Security = true";
        String ConexionIan = "Server = MARCOHDZ10-PC\\VSGESTION" + "Database=DictamenesMedicos; " + "Integrated Security = true";
        public RepositoryBase()
        {
            // Connection. Solo cambiar el nombre de Server
            _connectionString = conexionFrancisco;
            

            // Crear su conexion de string aqui y comentar y descomentar segun la necesiten
            //_connectionString =
            //"Server = AQUI_VA_SU_NOMBRE_DEL_SERVER; " +
            //"Database=DictamenesMedicos; " +
            //"Integrated Security = true";
           
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
