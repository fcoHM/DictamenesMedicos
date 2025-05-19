using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace DictamenesMedicos.Auxiliares
{
    public class VentanasError
    {
        static public void ShowErrorVentana(string errorMessage)
        {
            // Mensaje
            var messageText = new TextBlock
            {
                Text = errorMessage,
                Margin = new Thickness(20),
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.DarkRed,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            };

            // Boton cerrar
            var closeButton = new Button
            {
                Content = "Cerrar",
                Width = 80,
                Height = 30,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.IndianRed,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };

            // Contenedor
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(20)
            };

            stackPanel.Children.Add(messageText);
            stackPanel.Children.Add(closeButton);

            // Crear la ventana de error
            var errorWindow = new Window
            {
                Title = "❌ Error",
                Content = stackPanel,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowStyle = WindowStyle.ToolWindow,
                Background = Brushes.White,
                Topmost = true
            };

            // Funcionalidad Cerrar
            closeButton.Click += (s, e) => errorWindow.Close();

            errorWindow.ShowDialog();
        }
    }
}
