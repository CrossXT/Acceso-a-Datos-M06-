using System.Windows;
using MySql.Data.MySqlClient;

namespace MansionMapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;
        private string host;
        private string username;
        private string password;
        private string database;
        private string port;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            //Fijacion de valores

            host = ServerText.Text;
            username = UserText.Text;
            password = PassText.Text;
            database = DBText.Text;
            port = PortText.Text;

            // Crea la conexión con la base de datos utilizando los valores
            // de los campos como server o user para construir el connection string

            string connectionString;

            connectionString = $"SERVER={host};DATABASE={database};UID={username};PASSWORD={password};PORT={port}";

            //Iniciando conexion
            connection = new MySqlConnection(connectionString);

            MessageBox.Show(connectionString);


            //try
            //{
                connection.Open();
                MessageBox.Show("Conectado a la DB");
            //}
            //catch (MySqlException ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);

            //    //switch (ex.Number)
            //    //{
            //    //    case 0:
            //    //        Console.WriteLine("No se pudo conectar a la DB");
            //    //        break;

            //    //    case 1045:
            //    //        Console.WriteLine("Usuario o Contraseña incorrectos,intentelo otra vez");
            //    //        break;
            //    //}
            //}

        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Cierra la conexión con la base de datos
            try
            {
                connection.Close();
                MessageBox.Show("Desconectado de la DB,Hasta la proxima!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InitializeButton_Click(object sender, RoutedEventArgs e)
        {
            string DeleteIfExist = "DROP TABLE IF EXISTS Rooms";
            MySqlCommand Initialize = new MySqlCommand(DeleteIfExist, connection);




           
            // Volver a crear las tablas de la base de datos, eliminando las
            // tablas anteriores
        }

        private void RoomFindButton_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar los campos con los valores de la habitación que
            // corresponda al id que el diseñador tenga puesto
        }

        private void RoomAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Añadir una nueva habitación con los valores que el diseñador tenga
            // puestos en los campos
        }

        private void RoomModifyButton_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar la habitación con el id que tenga puesto el diseñador
            // con que tenga puesto en el resto de campos

        }

        private void RoomDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar la habitación con el id que tenga puesto el diseñador
        }

        private void RoomListUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            RoomListText.Text = "";

            // Buscar todas las habitaciones y añadirlas a la lista.
        }
    }
}