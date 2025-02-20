using NorwindApp.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace NorwindApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        // Navegar a la pantalla de inicio
        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new HomePage());
        }

        // Navegar a la pantalla de agregar productos
        private void NavigateToAdd(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new AddProductPage());
        }

        // Navegar a la pantalla de modificar productos
        private void NavigateToModify(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new ModifyProductPage());
        }

        // Navegar a la pantalla de eliminar productos
        private void NavigateToDelete(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new DeleteProductPage());
        }

        // Minimizar la ventana
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Cerrar la aplicación
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseHelper.CerrarConexion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            Application.Current.Shutdown(); // Cierra la aplicación
        }

        // Cerrar la aplicación y volver al login
        private void CloseApp(object sender, RoutedEventArgs e)
        {
                try
                {
                    // Cerrar la conexión con la base de datos
                    DatabaseHelper.CerrarConexion();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                // Abrir la ventana de login
                Login loginVentana = new Login();
                loginVentana.Show(); // Mostrar la ventana de login

                // Cerrar la ventana principal (MainWindow)
                // this.Close();
        }

        // Manejo para mover la ventana sin bordes
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}