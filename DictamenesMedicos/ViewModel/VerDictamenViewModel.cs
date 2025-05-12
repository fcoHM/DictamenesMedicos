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
    public class VerDictamenViewModel : ViewModelBase
    {
        public ICommand regresarCommand { get; set; }

        public VerDictamenViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);


        }

        private void ExecuteRegresarCommand(object obj)
        {
            // Buscar la ventana HomePaciente oculta
            var homePaciente = Application.Current.Windows
                .OfType<HomePaciente>()
                .FirstOrDefault();

            // Obtener la ventana actual (Registro Paciente)
            var ventanaActual = obj as Window ??
                              Application.Current.Windows
                                  .OfType<VerDictamen>()
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
