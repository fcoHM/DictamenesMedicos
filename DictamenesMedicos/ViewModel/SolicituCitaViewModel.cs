using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DictamenesMedicos.CustomControls;
using DictamenesMedicos.Model;
using DictamenesMedicos.Repositories;
using DictamenesMedicos.View;

namespace DictamenesMedicos.ViewModel
{
    internal class SolicituCitaViewModel:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged; 

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //comando para regresar a una ventana home
        public ICommand regresarCommand { get; set; }
        // Comando para abrir/cerrar el Popup
        public ICommand TogglePopupCommand { get; set; }
        // Comando para enviar datos
        public ICommand EnviarDatosCommand { get; }


        private UserModel miPaciente;
        private CitaModel miCita;

        // Este es el DAO para jalar datos de un usuario de la bdd
        private UserRepository miPacienteRepository;

        // Campos de la vista paciente
        public string NombrePaciente { get; set; }

        //campos cita

        
        
      


        //popup
        // Ejemplo de propiedad con notificación
        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                if (_isPopupOpen != value)
                {
                    _isPopupOpen = value;
                    OnPropertyChanged(); // Notifica el cambio
                }
            }
        }


        //metodo para mostrar/cerrar popup
        private void ExecuteTogglePopup(object obj)
        {
            // Alternar el estado del Popup
            IsPopupOpen = !IsPopupOpen;

            // Si necesitas acceder al Popup directamente (opcional):
            if (obj is Popup popup)
            {
                popup.IsOpen = IsPopupOpen;
            }
        }



    


        //constructor

        public SolicituCitaViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);
            TogglePopupCommand = new ViewModelCommand(ExecuteTogglePopup, null);
            EnviarDatosCommand = new ViewModelCommand(ExecuteEnviarDatosCommand);

            // Inicializamos el DAO de mipaciente
            miPacienteRepository = new UserRepository();
            // Por ejemplo con esta funcion populamos todos los datos de miPaciente
            LoadCurrentUserData();

        }



        private void LoadCurrentUserData()
        {
            // Con esto jalamos el NSS del usuario actual corriendo la app
            string _nss = Thread.CurrentPrincipal.Identity.Name;

            // Con este nss podemos hacer un query para jalar todo el usuario desde la bdd
            // usando un metodo llamado GetByNss() que creamos en el UserRepository
            miPaciente = miPacienteRepository.GetByNSS(_nss); // Nos devuelve un UserModel 

            // Ahora ya que tenemos el binding con la vista
            // Podemos mostrar el nombre del usuario actualmente loggeado en la app 
            // solamente reasingando la propiedad de a la que estamos haciendo binding
            NombrePaciente = miPaciente.Nombre;

            // Per ahora ya con miPaciente le podemos acceder todos sus atributos
            //miPaciente.Calle
            //miPaciente.Id
            // etc
        }



        //metodo para regresar
        private void ExecuteRegresarCommand(object obj)
        {
            // Buscar la ventana HomePaciente oculta
            var homePaciente = Application.Current.Windows
                .OfType<HomePaciente>()
                .FirstOrDefault();

            // Obtener la ventana actual (SolicitudCita)
            var ventanaActual = obj as Window ??
                              Application.Current.Windows
                                  .OfType<SolicitudCita>()
                                  .FirstOrDefault();

            if (homePaciente != null)
            {
                // Mostrar la ventana Home existente
                homePaciente.Show();

                // Cerrar la ventana actual
                ventanaActual?.Close();
            }
            else
            {
                // Crear nueva instancia si no se encontró
                var nuevoHome = new HomePaciente();
                nuevoHome.Show();
                ventanaActual?.Close();
            }
        }



        //mandar campos a ventana emergente 
        // Reemplaza estas propiedades:
        public string CorreoPaciente { get; set; }
        public DateTime FechaCita { get; set; }

        // Por estas (con notificación de cambios):
        private string _telefono;
        public string Telefono
        {
            get => _telefono;
            set { _telefono = value; OnPropertyChanged(); }
        }

        private string _correoElectronico;
        public string CorreoElectronico
        {
            get => _correoElectronico;
            set { _correoElectronico = value; OnPropertyChanged(); }
        }

        private DateTime? _fechaSeleccionada;
        public DateTime? FechaSeleccionada
        {
            get => _fechaSeleccionada;
            set { _fechaSeleccionada = value; OnPropertyChanged(); }
        }

        private string _tipoExamenSeleccionado;
        public string TipoExamenSeleccionado
        {
            get => _tipoExamenSeleccionado;
            set { _tipoExamenSeleccionado = value; OnPropertyChanged(); }
        }
        private void ExecuteEnviarDatosCommand(object obj)
        {
            // 1. Validar datos antes de continuar
            if (!ValidarDatosCita())
            {
                MessageBox.Show("Por favor complete todos los campos requeridos");
                return;
            }

            // 2. Crear el modelo de cita con los datos del formulario
            var cita = new CitaModel
            {
                Id = Guid.NewGuid().ToString(),
                Telefono = this.Telefono,
                correoElectronico = this.CorreoElectronico,
                fechaCita = this.FechaSeleccionada ?? DateTime.Now,
                IdTipoExamen = this.TipoExamenSeleccionado,
                IdPaciente = miPaciente?.Id // Usar el ID del paciente cargado
            };

            // 3. Abrir el popup (reutilizando tu comando existente)
            IsPopupOpen = true;

            // 4. Pasar los datos al popup (implementa una de estas opciones)
            if (obj is Popup popup && popup.Child is EmergenteCita emergente)
            {
                emergente.DataContext = new EmergenteSolicitudCitaViewModel(cita);
            }
        }

        private bool ValidarDatosCita()
        {
            return !string.IsNullOrEmpty(CorreoElectronico) &&
                   FechaSeleccionada.HasValue &&
                   !string.IsNullOrEmpty(TipoExamenSeleccionado);
        }

    }
}
