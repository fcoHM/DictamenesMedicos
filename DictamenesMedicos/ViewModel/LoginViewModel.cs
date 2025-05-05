using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DictamenesMedicos.Repositories;
using DictamenesMedicos.View;


namespace DictamenesMedicos.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {


        // Commandos
        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }


        // Constructor
        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            SignUpCommand = new ViewModelCommand(ExecuteSignUpCommand, null);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            return true;
        }
        private void ExecuteLoginCommand(object obj) {
            

            // Creamos y Abrimos la nueva ventana
            Application.Current.Dispatcher.Invoke(() =>
            {
                var homePaciente = new View.HomePaciente();
                homePaciente.Show();

                // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is LoginView)
                    {
                        window.Close();
                        break;
                    }
                }
            });

        }

        private void ExecuteSignUpCommand(object obj)
        {


            // Creamos y Abrimos la nueva ventana
            Application.Current.Dispatcher.Invoke(() =>
            {
                var signUp = new CrudPaciente();
                signUp.Show();

                // Cerrar ventana de login, hay veces que solo es ocultar la ventana, no cerrar
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is LoginView)
                    {
                        window.Close();
                        break;
                    }
                }
            });

        }
    }
}
