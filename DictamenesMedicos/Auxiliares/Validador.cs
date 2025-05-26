using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictamenesMedicos.Auxiliares
{
    public class Validador
    {
        static public bool EsNombreValido(string nombre)
        {
            string patron = @"^([A-Za-zÁÉÍÓÚÑáéíóúñ]{3,})(\s[A-Za-zÁÉÍÓÚÑáéíóúñ]{3,})*$";
            return Regex.IsMatch(nombre, patron);
        }
        static public bool EsApellidoValido(string apellido)
        {
            string patron = @"^([A-Za-zÁÉÍÓÚÑáéíóúñ]{3,})(\s[A-Za-zÁÉÍÓÚÑáéíóúñ]{2,})*$";
            return Regex.IsMatch(apellido, patron);
        }

        static public bool EsNumeroTelefonoValido(string numero)
        {
            string patron = @"^\d{10}$";
            return Regex.IsMatch(numero, patron);
        }
        static public bool FechaNacimientoValida(DateTime fechaNac) // Si es mayor a 15 años fechaNac
        {
            return (new DateTime(2009, 12, 31)) > fechaNac;
        }

        static public bool EsCorreoValido(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(correo, patron);
        }

        static public bool EsNSSValido(string nss)
        {
            string patron = @"^\d{11}$";
            return Regex.IsMatch(nss, patron);
        }

        static public bool EsPasswordValida(SecureString strPass)
        {
            string texto = SecureStringHasher.SecureStringToString(strPass);
            return string.IsNullOrWhiteSpace(texto) == false && texto.Length >= 8;
        }

        public static bool EsCodigoPostalValido(string codigoPostal)
        {
            return Regex.IsMatch(codigoPostal, @"^\d{5}$");
        }

        // Texto no vacio
        public static bool EstaVacia(string texto)
        {
            return string.IsNullOrWhiteSpace(texto);
        }
    }
}
