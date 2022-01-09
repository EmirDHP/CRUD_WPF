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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace CRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VR4M00D;Initial Catalog=NewDB;Integrated Security=True");

        public void limpiarDatos()
        {
            nombre_txt.Clear();
            edad_txt.Clear();
            genero_txt.Clear();
            ciudad_txt.Clear();
            buscar_txt.Clear();
        }
        private void limpiar_btn_Click(object sender, RoutedEventArgs e)
        {
            limpiarDatos();
        }

        public void CargarGrid()
        {
            SqlCommand cmd = new SqlCommand("exec SP_UsuariosVista;", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        public bool isValid()
        {
            if(nombre_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (edad_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (genero_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ciudad_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void crear_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Usuarios VALUES (@Nombre, @Edad, @Genero, @Ciudad)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Nombre", nombre_txt.Text);
                    cmd.Parameters.AddWithValue("@Edad", edad_txt.Text);
                    cmd.Parameters.AddWithValue("@Genero", genero_txt.Text);
                    cmd.Parameters.AddWithValue("@Ciudad", ciudad_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    CargarGrid();
                    MessageBox.Show("Registrado exitosamente", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpiarDatos();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Usuarios WHERE ID_Usuario = " +buscar_txt.Text+ " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("El usuario a sido eliminado", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                limpiarDatos();
                CargarGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No eliminado" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void actualizar_btn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Nombre = '"+nombre_txt.Text+"', Edad = '"+edad_txt.Text+"', Genero = '"+genero_txt.Text+"', Ciudad = '"+ciudad_txt.Text+"' WHERE ID_Usuario = '"+buscar_txt.Text+"' ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("El ususario a sido actualizado exitosamente!", "Actualizar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                limpiarDatos();
                CargarGrid();
            }
        }
    }
}
