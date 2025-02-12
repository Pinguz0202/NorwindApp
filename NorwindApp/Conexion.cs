using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace NorwindApp
{
    public static class DatabaseHelper
    {
        private static string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=root;database=northwindprueba";
        private static MySqlConnection conexion = new MySqlConnection(connectionString);

        // Método para abrir conexión (si no está abierta)
        public static void AbrirConexion()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir conexión: {ex.Message}");
            }
        }

        // Método para cerrar conexión
        public static void CerrarConexion()
        {
            try
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar conexión: {ex.Message}");
            }
        }

        // Método para cerrar conexión al salir de la aplicación
        public static void CerrarConexionYSalir()
        {
            CerrarConexion();
            Application.Current.Shutdown();
        }

        // Método para realizar login
        public static bool Login(string usuario, string contrasena)
        {
            bool existe = false;
            string consultaUser = "SELECT * FROM usuario WHERE usuario = @usuario AND contraseña = @contrasena";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consultaUser, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);

                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        existe = lector.Read();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en login: {ex.Message}");
            }

            return existe;
        }

        // Método para mostrar productos
        public static DataTable MostrarProductos()
        {
            DataTable productos = new DataTable();
            string consulta = "SELECT p.ProductID, p.ProductName, c.CategoryName FROM Products p JOIN Categories c ON p.CategoryID = c.CategoryID";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(productos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
            }

            return productos;
        }

        // Método para agregar un producto
        public static bool AgregarProducto(string nombre, int categoriaID)
        {
            bool resultado = false;
            string consulta = "INSERT INTO Products (ProductName, CategoryID) VALUES (@nombre, @categoriaID)";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@categoriaID", categoriaID);
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar producto: {ex.Message}");
            }

            return resultado;
        }

        // Método para obtener el ID del producto por su nombre
        public static int ObtenerIDProductoPorNombre(string nombreProducto)
        {
            int productId = -1; // Valor por defecto si no se encuentra

            string consulta = "SELECT ProductID FROM Products WHERE ProductName = @nombreProducto LIMIT 1";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombreProducto", nombreProducto);
                    object result = cmd.ExecuteScalar(); // Obtener el ID del producto

                    if (result != null)
                    {
                        productId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ID del producto: {ex.Message}");
            }

            return productId;
        }

        public static DataTable ObtenerCategorias()
        {
            DataTable categorias = new DataTable();
            string consulta = "SELECT CategoryID, CategoryName FROM Categories";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(categorias);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
            }

            return categorias;
        }

        // Método para modificar un producto
        public static bool ModificarProducto(int productId, string nuevoNombre, int nuevaCategoriaID)
        {
            bool resultado = false;
            string consulta = "UPDATE Products SET ProductName = @nuevoNombre, CategoryID = @nuevaCategoriaID WHERE ProductID = @productId";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                    cmd.Parameters.AddWithValue("@nuevaCategoriaID", nuevaCategoriaID);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar producto: {ex.Message}");
            }

            return resultado;
        }


        // Método para eliminar un producto
        public static bool EliminarProducto(int productId)
        {
            bool resultado = false;
            string consulta = "DELETE FROM Products WHERE ProductID = @productId";

            try
            {
                AbrirConexion();
                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (MySqlException sqlEx) when (sqlEx.Number == 1451)
            {
                MessageBox.Show("No se puede eliminar este producto porque está relacionado con otros datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
            }

            return resultado;
        }
    }
}