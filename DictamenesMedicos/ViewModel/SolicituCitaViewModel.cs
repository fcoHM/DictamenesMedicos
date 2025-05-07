using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DictamenesMedicos.View;

namespace DictamenesMedicos.ViewModel
{
    internal class SolicituCitaViewModel
    {

        public ICommand regresarCommand { get; set; }
        public ICommand mostrarCommand { get; set;}

        //constructor

        public SolicituCitaViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);
            mostrarCommand = new ViewModelCommand(OpenPopUp, null);
        }



        //metodo para 
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




        private bool isOpenPopUp = false;

        public void OpenPopUp(object obj) 
        {
            if (obj is Popup popup)
            {
                popup.IsOpen = true;
            }
        }

    }
}
