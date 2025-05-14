using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DictamenesMedicos.Auxiliares;
using DictamenesMedicos.Model;
using DictamenesMedicos.Repositories;
using DictamenesMedicos.View;


namespace DictamenesMedicos.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {


        // Commandos
        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        // Campos
        private string _nss;
        private SecureString _password;
        private string _errorMessage;
        private UserRepository userRepository;

        public string NSS
        {
            get { return _nss; }
            set { 
                _nss = value;
                OnPropertyChanged(nameof(NSS));
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
          {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        // Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            SignUpCommand = new ViewModelCommand(ExecuteSignUpCommand, null);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(NSS)
                || NSS.Length < 3
                || Password == null
                || Password.Length < 3)
                validData = false;
            else
                validData = true;


            return validData;
        }


        private void ExecuteLoginCommand(object obj)
        {
            Password = 
                SecureStringHasher.ConvertToSecureString(
                    SecureStringHasher.HashPasswordFromSecureString(Password)
                    );

            var isValidUser = userRepository.AuthenticateUser(
                new NetworkCredential(NSS, Password));

            if (isValidUser)
            {
                //Console.WriteLine("Usuario Valido");

                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(NSS), null);


                var homePaciente = new View.HomePaciente();
                homePaciente.Show();
                // Creamos y Abrimos la nueva ventana
                Application.Current.Dispatcher.Invoke(() =>
                {
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
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }

        private void ExecuteSignUpCommand(object obj)
        {

            //Creamos y Abrimos la nueva ventana
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
