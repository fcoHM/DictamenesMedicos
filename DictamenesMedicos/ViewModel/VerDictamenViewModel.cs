using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DictamenesMedicos.Model;
using DictamenesMedicos.Repositories;
using DictamenesMedicos.View;

namespace DictamenesMedicos.ViewModel
{
    public class VerDictamenViewModel : ViewModelBase
    {
        public ICommand regresarCommand { get; set; }

        public VerDictamenViewModel()
        {
            regresarCommand = new ViewModelCommand(ExecuteRegresarCommand, null);

            miPacienteRepository = new UserRepository();
            // Por ejemplo con esta funcion populamos todos los datos de miPaciente
            LoadCurrentUserData();

        }

        private UserModel miPaciente;

        // Este es el DAO para jalar datos de un usuario de la bdd
        private UserRepository miPacienteRepository;

        // Campos de la vista paciente
        public string NombrePaciente { get; set; }

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
    }
}
