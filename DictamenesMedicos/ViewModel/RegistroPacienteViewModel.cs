using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DictamenesMedicos.Model;
using DictamenesMedicos.Repositories;
using DictamenesMedicos.View;
using DictamenesMedicos.Auxiliares;
using System.Security;

namespace DictamenesMedicos.ViewModel
{
    public class RegistroPacienteViewModel: ViewModelBase
    {
        public ICommand regresarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand DescartarCommand { get; set; }


        // Datos Paciente
        private UserModel _paciente;

        public UserModel Paciente
        {
            get => _paciente;
            set
            {
                _paciente = value;
                OnPropertyChanged(nameof(Paciente));
            }
        }

        private int _selectedSexo;
        public int SelectedSexo
        {
            get { return _selectedSexo; }
            set
            {
                _selectedSexo = value;
                OnPropertyChanged(nameof(SelectedSexo));
            }
        }

        private int _tipoSangre;
        public int TipoSangre
        {
            get => _tipoSangre;
            set
            {
                _tipoSangre = value;
                OnPropertyChanged(nameof(TipoSangre));
            }
        }

        private DateTime? _fechaNacimiento;
        public DateTime? FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                _fechaNacimiento = value;
                OnPropertyChanged(nameof(FechaNacimiento));
            }
        }

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private UserRepository _userRepository;

        public RegistroPacienteViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);
            GuardarCommand = new ViewModelCommand(ExecuteGuardarCommand, null);
            DescartarCommand = new ViewModelCommand(ExecuteDescartarCommand, null);
            _userRepository = new UserRepository();
            SelectedSexo = 0;
            TipoSangre = 0;
            _paciente = new UserModel
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = null,
                NSS = null,
                Password = null,
                ApellidoPaterno = null,
                ApellidoMaterno = null,
                Sexo = 1,
                TelefonoFijo = null,
                TelefonoMovil = null,
                CorreoElectronico = null,
                CodigoPostal = null,
                Estado = null,
                Municipio = null,
                Localidad = null,
                Calle = null,
                NumeroExterior = null,
                NumeroInterior = null,
                DescripcionUbicacion = null,
                TipoSangre = 0,
                EnfermedadesCronicas = null,
                Alergias = null
            };
        }

        // Metodo para registrar un paciente
        private void ExecuteGuardarCommand(object obj)
        {
            Paciente.Sexo = SelectedSexo;
            Paciente.FechaNacimiento = FechaNacimiento ?? DateTime.Now;
            Paciente.TipoSangre = TipoSangre;
            

            if (CheckAllData())// esta todo en orden por lo tanto podemos guardar
            {
                Paciente.Password = SecureStringHasher.HashPasswordFromSecureString(Password);
                // Lo guardamos
                _userRepository.AddPaciente(Paciente);

                // Nos vamos al login de new
                var loginView = new View.LoginView();
                loginView.Show();
                // Creamos y Abrimos la nueva ventana
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is CrudPaciente)
                        {
                            window.Close();
                            break;
                        }
                    }
                });
            }
        }

        private bool CheckAllData() {
            if (string.IsNullOrEmpty(Paciente.Nombre) || Validador.EsNombreValido(Paciente.Nombre) == false)
            {
                VentanasError.ShowErrorVentana("El Nombre no es válido.\nDebe ser mayor a 3 letras ('Juan' o 'Juan José').");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.ApellidoPaterno) || Validador.EsApellidoValido(Paciente.ApellidoPaterno) == false)
            {
                VentanasError.ShowErrorVentana("El Apellido Paterno no es válido.\nDebe ser mayor a 3 letras ('Nuñez' o ' Fernández de Córdoba').");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.ApellidoMaterno) || Validador.EsApellidoValido(Paciente.ApellidoMaterno) == false)
            {
                VentanasError.ShowErrorVentana("El Apellido Materno no es válido.\nDebe ser mayor a 3 letras ('Nuñez' o ' Fernández de Córdoba').");
                return false;
            }
            else if (SelectedSexo == 0)
            {
                VentanasError.ShowErrorVentana("El Sexo no es válido.\nElige una opción.");
                return false;
            }
            else if (Validador.FechaNacimientoValida(Paciente.FechaNacimiento) == false) // TIENE QUE SER MAYOR A 15 AÑOS
            {
                VentanasError.ShowErrorVentana("La Fecha de Nacimiento no es válida.\nEl paciente debe ser mayor a 15 años.");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.TelefonoFijo) == false && Validador.EsNumeroTelefonoValido(Paciente.TelefonoFijo) == false)
            {
                VentanasError.ShowErrorVentana("El número de Teléfono Fijo no es válido.\nDebe tener solo 10 dígitos (e.g. 4922448987).");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.TelefonoMovil) == true || Validador.EsNumeroTelefonoValido(Paciente.TelefonoMovil) == false)
            {
                VentanasError.ShowErrorVentana("El número de Teléfono Móvil no es válido.\nDebe tener solo 10 dígitos (e.g. 4922448987).");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.CorreoElectronico) == true || Validador.EsCorreoValido(Paciente.CorreoElectronico) == false)
            {
                VentanasError.ShowErrorVentana("El Correo no es válido.\nE.g. example.ex_ex@example.com");
                return false;
            }
            else if (string.IsNullOrEmpty(Paciente.NSS) == true || Validador.EsNSSValido(Paciente.NSS) == false)
            {
                VentanasError.ShowErrorVentana("El Número de Seguridad Social no es válido.\nDebe contener 11 digitos.");
                return false;
            }
            else if (Validador.EsPasswordValida(Password) == false)
            {
                VentanasError.ShowErrorVentana("La Contraseña es inválida.\nDebe ser mayor o igual a 8 caracteres.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.CodigoPostal) == true || Validador.EsCodigoPostalValido(Paciente.CodigoPostal) == false)
            {
                VentanasError.ShowErrorVentana("El Código Postal es inválido.\nDebe ser de 5 digitos.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.Estado) == true)
            {
                VentanasError.ShowErrorVentana("El Estado es inválido.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.Municipio) == true)
            {
                VentanasError.ShowErrorVentana("El Municipio es inválido.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.Localidad) == true)
            {
                VentanasError.ShowErrorVentana("La Localidad es inválida.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.Calle) == true)
            {
                VentanasError.ShowErrorVentana("La Calle es inválida.");
                return false;
            }
            else if (Validador.EstaVacia(Paciente.NumeroExterior) == true)
            {
                VentanasError.ShowErrorVentana("El Número Exterior es inválido.");
                return false;
            }

            else { return true; }
        }

        //metodo para 
        private void ExecuteRegresarCommand(object obj)
        {
            // Buscar la ventana HomePaciente oculta
            var loginPaciente = Application.Current.Windows
                .OfType<LoginView>()
                .FirstOrDefault();

            // Obtener la ventana actual (Registro Paciente)
            var ventanaActual = obj as Window ??
                              Application.Current.Windows
                                  .OfType<CrudPaciente>()
                                  .FirstOrDefault();

            if (loginPaciente != null)
            {
                // Mostrar la ventana Home existente
                loginPaciente.Show();

                // Cerrar la ventana actual
                ventanaActual?.Close();
            }
            else
            {
                // Crear nueva instancia si no se encontró
                var nuevoLogin = new LoginView();
                nuevoLogin.Show();
                ventanaActual?.Close();
            }
        }

        private void ExecuteDescartarCommand(object obj)
        {
            // Reiniciamos el paciente
            Paciente = new UserModel
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = null,
                NSS = null,
                Password = null,
                ApellidoPaterno = null,
                ApellidoMaterno = null,
                Sexo = 1,
                TelefonoFijo = null,
                TelefonoMovil = null,
                CorreoElectronico = null,
                CodigoPostal = null,
                Estado = null,
                Municipio = null,
                Localidad = null,
                Calle = null,
                NumeroExterior = null,
                NumeroInterior = null,
                DescripcionUbicacion = null,
                TipoSangre = 0,
                EnfermedadesCronicas = null,
                Alergias = null
            };

            // Reiniciamos las demás propiedades
            SelectedSexo = 0;
            TipoSangre = 0;
            FechaNacimiento = null;
            Password = null;
        }
    }
}
