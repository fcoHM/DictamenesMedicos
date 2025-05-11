using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictamenesMedicos.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string NSS { get; set; }
        public string Password { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoMovil {  get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoPostal {  get; set; }
        public string Estado {  get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Calle {  get; set; }
        public string NumeroExterior {  get; set; }
        public string NumeroInterior {  get; set; }
        public string DescripcionUbicacion {  get; set; }
        public int TipoSangre {  get; set; }
        public string EnfermedadesCronicas {  get; set; }
        public string Alergias {  get; set; }


    }
}
