using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DictamenesMedicos.CustomControls
{
    /// <summary>
    /// Interaction logic for EmergenteCita.xaml
    /// </summary>
    public partial class EmergenteCita : UserControl
    {
        public EmergenteCita()
        {
            InitializeComponent();
        }

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            // Cierra el Popup y notifica al ViewModel
            if (this.Parent is Popup popup)
            {
                popup.IsOpen = false; // Esto actualizará automáticamente IsPopupOpen (gracias al Binding TwoWay)
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
