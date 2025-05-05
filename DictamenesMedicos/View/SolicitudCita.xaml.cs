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
    /// Interaction logic for SolicitudCita.xaml
    /// </summary>
    public partial class SolicitudCita : Window
    {
        public SolicitudCita()
        {
            InitializeComponent();
        }

       

        private  void btnRegresar_Click(object sender, RoutedEventArgs e)
        {

        }
 
        private void btnMinimizar_Click(object sender, RoutedEventArgs e) // minimizar app
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) // cerrar app
        {
            this.Close();
        }
    }
}
