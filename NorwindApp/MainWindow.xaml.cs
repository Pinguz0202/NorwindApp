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

        // Cerrar completamente la aplicación (Botón X)
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

        // Cerrar la conexión y volver al login (Botón Exit)
        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseHelper.CerrarConexion(); // Cerrar conexión antes de volver al login
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Verificar si LoginWindow existe
            Login login = new Login();
            login.Show();

            // Cerrar la ventana actual
            this.Close();
        }

        // Manejo del evento de cierre de ventana (Closing)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DatabaseHelper.CerrarConexion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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