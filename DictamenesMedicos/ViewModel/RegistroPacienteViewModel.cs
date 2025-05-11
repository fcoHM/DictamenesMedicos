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
    internal class RegistroPacienteViewModel
    {
        public ICommand regresarCommand { get; set; }

        public RegistroPacienteViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);
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
    }
}
