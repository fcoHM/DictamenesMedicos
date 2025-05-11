using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DictamenesMedicos.Model;
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


    }
}
