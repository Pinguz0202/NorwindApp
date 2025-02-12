using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NorwindApp.Views
{
    public partial class DeleteProductPage : Page
    {
        public DeleteProductPage()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                DataTable productos = DatabaseHelper.MostrarProductos();
                if (productos != null)
                {
                    ProductosDataGrid.ItemsSource = productos.DefaultView;
                }
                else
                {
                    MostrarMensaje("No se pudieron cargar los productos.", "Advertencia");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar productos: " + ex.Message, "Error");
            }
        }

        private void EliminarProducto(object sender, RoutedEventArgs e)
        {
            string nombreProducto = ProductNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto))
            {
                MostrarMensaje("Por favor, ingrese un nombre de producto válido.", "Advertencia");
                return;
            }

            int productId = DatabaseHelper.ObtenerIDProductoPorNombre(nombreProducto);

            if (productId == -1)
            {
                MostrarMensaje("El producto no existe en la base de datos.", "Error");
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                $"¿Está seguro de eliminar el producto '{nombreProducto}'?",
                "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion == MessageBoxResult.Yes)
            {
                bool resultado = DatabaseHelper.EliminarProducto(productId);

                if (resultado)
                {
                    MostrarMensaje("Producto eliminado exitosamente.", "Éxito");
                    CargarProductos(); // Recargar la lista después de eliminar
                    ProductNameTextBox.Clear(); // Limpiar el TextBox
                }
                else
                {
                    MostrarMensaje("No se pudo eliminar el producto. Verifique dependencias.", "Error");
                }
            }
        }

        private void MostrarMensaje(string mensaje, string titulo)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButton.OK, MessageBoxImage.Information);
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
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}