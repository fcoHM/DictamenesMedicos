using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DictamenesMedicos.ViewModel
{
    internal class ViewModelCommand: ICommand
    {
        // Pasar todo un metodo como parametro a una funcion
        private readonly Action<object> _executeAction;

        // Para detectar si la accion se puede ejecutar o nel
        // Es el que checa si cumple con las condiciones
        private readonly Predicate<object> _canExecuteAction;

        // Constructores
        public ViewModelCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }

        public ViewModelCommand(Action<object> executeAction,
            Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        // Eventos
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; } // Agrega valors
            remove { CommandManager.RequerySuggested -= value; } // Quita valors
        }

        // Metodos

        // Evaluar si se paso un predicado
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);

        }

        // Simplemente ejecuta la accion
        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
