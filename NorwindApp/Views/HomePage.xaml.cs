using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NorwindApp.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            DataTable productos = DatabaseHelper.MostrarProductos();

            if (productos != null && productos.Rows.Count > 0)
            {
                ProductosDataGrid.ItemsSource = productos.DefaultView;
            }
            else
            {
                MessageBox.Show("No hay productos disponibles.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

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

            Window.GetWindow(this)?.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}