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

namespace NorwindApp
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Cierra la aplicación
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimiza la ventana
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            // Obtenemos el nombre de usuario y la contraseña introducidos
            string usuario = UserNameTextBox.Text;
            string contrasena = PasswordTextBox.Text;

            // Llamamos al método Login de la clase DatabaseHelper
            bool loginExitoso = DatabaseHelper.Login(usuario, contrasena);

            if (loginExitoso)
            {
                // Abrimos la ventana principal
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Cerramos la ventana de login
                this.Close();
            }
            else
            {
                // Si las credenciales no son correctas, mostramos un mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos. Intenta de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
