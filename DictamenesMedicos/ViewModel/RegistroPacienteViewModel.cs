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
