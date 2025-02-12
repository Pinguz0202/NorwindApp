using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace NorwindApp.Views
{
    public partial class AddProductPage : Page
    {
        public AddProductPage()
        {
            InitializeComponent();
            CargarProductos();
            CargarCategorias();
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

        private void CargarCategorias()
        {
            try
            {
                DataTable categorias = DatabaseHelper.ObtenerCategorias();

                if (categorias != null && categorias.Rows.Count > 0)
                {
                    CategoriaComboBox.ItemsSource = categorias.DefaultView;
                    CategoriaComboBox.DisplayMemberPath = "CategoryName";
                    CategoriaComboBox.SelectedValuePath = "CategoryID";
                }
                else
                {
                    MessageBox.Show("No se pudieron cargar las categorías.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarCampos()
        {
            NombreTextBox.Clear();
            CategoriaComboBox.SelectedIndex = -1;
        }

        private void AgregarProducto(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = NombreTextBox.Text;
                if (CategoriaComboBox.SelectedValue is int categoriaID)
                {
                    if (!string.IsNullOrWhiteSpace(nombre))
                    {
                        if (DatabaseHelper.AgregarProducto(nombre, categoriaID))
                        {
                            MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            CargarProductos();
                            LimpiarCampos();
                        }
                        else
                        {
                            MessageBox.Show("Error al agregar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El campo Producto no puede estar vacío.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una categoría válida.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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