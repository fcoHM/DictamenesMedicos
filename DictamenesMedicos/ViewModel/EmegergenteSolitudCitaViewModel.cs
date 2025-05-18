using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using DictamenesMedicos.Model;
using DictamenesMedicos.Repositories;

namespace DictamenesMedicos.ViewModel
{
    internal class EmergenteSolicitudCitaViewModel : INotifyPropertyChanged
    {
        private readonly CitaRepository _citaRepository = new CitaRepository();
        private readonly UserRepository _userRepository = new UserRepository();

        // Modelo de cita
        public CitaModel Cita { get; set; }

        // Propiedades para la vista
        private string _nombrePaciente;
        public string NombrePaciente
        {
            get => _nombrePaciente;
            set { _nombrePaciente = value; OnPropertyChanged(); }
        }

        private string _tipoCita;
        public string TipoCita
        {
            get => _tipoCita;
            set { _tipoCita = value; OnPropertyChanged(); }
        }

        private string _fechaCitaFormateada;
        public string FechaCitaFormateada
        {
            get => _fechaCitaFormateada;
            set { _fechaCitaFormateada = value; OnPropertyChanged(); }
        }

        // Comandos
        public ICommand GuardarCitaCommand { get; }
        public ICommand CancelarCommand { get; }

        public EmergenteSolicitudCitaViewModel(CitaModel cita)
        {
            Cita = cita ?? throw new ArgumentNullException(nameof(cita));

            // Inicializar comandos
            GuardarCitaCommand = new RelayCommand(ExecuteGuardarCita, CanExecuteGuardarCita);
            CancelarCommand = new RelayCommand(ExecuteCancelar);

            // Cargar datos
            CargarDatosPaciente();
            FormatearDatos();
        }

        private void CargarDatosPaciente()
        {
            try
            {
                string nss = Thread.CurrentPrincipal.Identity.Name;
                var paciente = _userRepository.GetByNSS(nss);
                NombrePaciente = paciente?.Nombre ?? "Nombre no disponible";

                // Asignar ID de paciente si no está establecido
                if (string.IsNullOrEmpty(Cita.IdPaciente))
                {
                    Cita.IdPaciente = paciente?.Id;
                }
            }
            catch (Exception ex)
            {
                NombrePaciente = "Error cargando datos";
                Debug.WriteLine($"Error cargando paciente: {ex.Message}");
            }
        }

        private void FormatearDatos()
        {
            TipoCita = Cita?.IdTipoExamen ?? "Tipo no especificado";
            FechaCitaFormateada = Cita?.fechaCita.ToString("dd/MM/yyyy") ?? "Fecha no especificada";
        }

        #region Command Handlers

        private bool CanExecuteGuardarCita(object parameter)
        {
            return Cita != null &&
                   !string.IsNullOrWhiteSpace(Cita.IdTipoExamen) &&
                   !string.IsNullOrWhiteSpace(Cita.IdPaciente) &&
                   Cita.fechaCita > DateTime.Now;
        }

        private void ExecuteGuardarCita(object parameter)
        {
            try
            {
                // Validar datos antes de guardar
                if (!CanExecuteGuardarCita(parameter))
                {
                    MessageBox.Show("Datos de cita incompletos o inválidos", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Asignar doctor según tipo de examen
                // Normalizar entrada
                var tipo = Cita.IdTipoExamen?.Trim().ToLowerInvariant().Normalize(System.Text.NormalizationForm.FormD);


                switch (tipo)
                {
                    case "vista":
                        Cita.IdTipoExamen = "12D5F708-B621-486C-8383-4B43DC438148";
                        break;
                    case "densitometria osea":
                        Cita.IdTipoExamen = "5906B945-D205-48A6-942A-6A7B071364DB";
                        break;
                    case "espirometria":
                        Cita.IdTipoExamen = "490CB103-3EB3-4D0B-8FE6-C1692733F498";
                        break;
                    default:
                        MessageBox.Show("Tipo de examen no válido", "Error",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }

                // Asignar resultado por defecto si es necesario
                if (string.IsNullOrEmpty(Cita.resultado))
                {
                    Cita.resultado = "Pendiente";
                }



                if (_citaRepository.GuardarCita(Cita))
                {
                    MessageBox.Show("Cita guardada exitosamente", "Éxito",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    if (parameter is Window window)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo guardar la cita. Verifique los datos y conexión.", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error completo al guardar: {ex}");
                MessageBox.Show($"Error técnico al guardar: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelar(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }



    //permite enlazar (bindear) acciones de la interfaz gráfica con la lógica del ViewModel
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    }



}