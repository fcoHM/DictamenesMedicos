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
    public class HomePacienteViewModel
    {

        // Commands
        public ICommand DictamenCommand { get; set; }
        public ICommand MisCitasCommand { get; set; }
        public ICommand SolicitarCitaCommand { get; set; }
        public ICommand SalirCommand { get; set; }

        // Aqui es el modelo de mi usuario, despues le rellenaremos los datos desde la bdd
        private UserModel miPaciente;

        // Este es el DAO para jalar datos de un usuario de la bdd
        private UserRepository miPacienteRepository;

        // Campos de la vista
        public string NombrePaciente {  get; set; }

        // Constructor 
        public HomePacienteViewModel()
        {
            DictamenCommand = new ViewModelCommand(ExecuteDictamenCommand, null);
            MisCitasCommand = new ViewModelCommand(ExecuteMisCitasCommand, null);
            SolicitarCitaCommand = new ViewModelCommand(ExecuteSolicitarCitaCommand, null);
            SalirCommand = new ViewModelCommand(ExecuteSalirCommand, null);

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
