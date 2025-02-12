using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NorwindApp.Views
{
    public partial class ModifyProductPage : Page
    {
        public ModifyProductPage()
        {
            InitializeComponent();
            CargarProductos();
            CargarCategorias();
        }

        private void CargarProductos()
        {
            DataTable productos = DatabaseHelper.MostrarProductos();
            if (productos != null) // ✅ Verificar que no sea null
            {
                ProductosDataGrid.ItemsSource = productos.DefaultView;
            }
        }

        private void CargarCategorias()
        {
            DataTable categorias = DatabaseHelper.ObtenerCategorias();
            if (categorias != null) // ✅ Verificar que no sea null
            {
                CategoryComboBox.ItemsSource = categorias.DefaultView;
                CategoryComboBox.DisplayMemberPath = "CategoryName";
                CategoryComboBox.SelectedValuePath = "CategoryID";
            }
        }

        private void ModificarProducto(object sender, RoutedEventArgs e)
        {
            string oldProduct = OldProductTextBox?.Text.Trim();
            string newProduct = NewProductTextBox?.Text.Trim();

            if (string.IsNullOrWhiteSpace(oldProduct) || string.IsNullOrWhiteSpace(newProduct))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox?.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoría.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int oldProductID = DatabaseHelper.ObtenerIDProductoPorNombre(oldProduct);
            if (oldProductID == -1)
            {
                MessageBox.Show("El producto antiguo no existe en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CategoryComboBox.SelectedItem is DataRowView selectedRow)
            {
                int categoriaID = Convert.ToInt32(selectedRow["CategoryID"]);
                bool resultado = DatabaseHelper.ModificarProducto(oldProductID, newProduct, categoriaID);

                if (resultado)
                {
                    MessageBox.Show("Producto modificado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarProductos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al modificar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Error al obtener la categoría seleccionada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarCampos()
        {
            OldProductTextBox.Text = "";
            NewProductTextBox.Text = "";
            CategoryComboBox.SelectedIndex = -1;
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