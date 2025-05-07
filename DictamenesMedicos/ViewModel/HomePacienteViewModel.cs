using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DictamenesMedicos.View;

namespace DictamenesMedicos.ViewModel
{
    internal class HomePacienteViewModel
    {

        // Commands
        public ICommand DictamenCommand { get; set; }
        public ICommand MisCitasCommand { get; set; }
        public ICommand SolicitarCitaCommand { get; set; }
        public ICommand SalirCommand { get; set; }

        // Constructor 
        public HomePacienteViewModel()
        {
            DictamenCommand = new ViewModelCommand(ExecuteDictamenCommand, null);
            MisCitasCommand = new ViewModelCommand(ExecuteMisCitasCommand, null);
            SolicitarCitaCommand = new ViewModelCommand(ExecuteSolicitarCitaCommand, null);
            SalirCommand = new ViewModelCommand(ExecuteSalirCommand, null);
        }

        private void ExecuteSalirCommand(object obj)
        {
            Application.Current.Shutdown(); // Morimos la aplicación
        }

        private void ExecuteDictamenCommand(object obj)
        {


            // Creamos y Abrimos la nueva ventana
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dictamenes = new VerDictamen();
                dictamenes.Show();

                // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is HomePaciente)
                    {
                        window.Hide();
                        break;
                    }
                }
            });
        }

        private void ExecuteMisCitasCommand(object obj)
        {
            // Creamos y Abrimos la nueva ventana
            Application.Current.Dispatcher.Invoke(() =>
            {
                var citas = new SolicitudCita();
                citas.Show();

                // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is HomePaciente)
                    {
                        window.Hide();
                        break;
                    }
                }
            });
        }

        private void ExecuteSolicitarCitaCommand(object obj)
        {
            // Creamos y Abrimos la nueva ventana
            Application.Current.Dispatcher.Invoke(() =>
            {
                var citas = new SolicitudCita();
                citas.Show();

                // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is HomePaciente)
                    {
                        window.Hide();
                        break;
                    }
                }
            });
        }


    }
}
