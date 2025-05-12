using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictamenesMedicos.View
{
    /// <summary>
    /// Lógica de interacción para VerDictamen.xaml
    /// </summary>
    public partial class VerDictamen : Window
    {
        public VerDictamen()
        {
            InitializeComponent();
        }


        // Método para minimizar la ventana
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Método para cerrar la ventana
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Morimos la aplicación
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
