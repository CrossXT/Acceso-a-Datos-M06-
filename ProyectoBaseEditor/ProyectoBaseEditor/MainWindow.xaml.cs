using System.Data.Common;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
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

        private string roomId;
        private string roomName;
        private string roomDesc;
        private string roomBackground;

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



            connection.Open();
            MessageBox.Show("Conectado a la DB");


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
            MySqlCommand command;

            string DeleteIfExist = "DROP TABLE IF EXISTS Rooms;";
            command = new MySqlCommand(DeleteIfExist, connection);
            command.ExecuteNonQuery();

            string CreateTable = "CREATE TABLE Rooms (" + " id INT NOT NULL PRIMARY KEY, " + " name VARCHAR(40) NOT NULL, " + " description VARCHAR(40) NOT NULL," + " background VARCHAR(40) NOT NULL);";
            command = new MySqlCommand(CreateTable, connection);
            MessageBox.Show(CreateTable);
            command.ExecuteNonQuery();


  

            MessageBox.Show("Borrada la tabla Rooms,pero se ha vuelto a crear");




            // Volver a crear las tablas de la base de datos, eliminando las
            // tablas anteriores
        }

        private void RoomFindButton_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand command;

            
            string findById = "SELECT * FROM Rooms WHERE id = " + RoomIdText.Text;
            MessageBox.Show(findById);
            command = new MySqlCommand(findById, connection);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string desc = reader.GetString(2);
                string background = reader.GetString(3);



                RoomNameText.Text = name;
                RoomDescriptionText.Text = desc;
                RoomBackgroundText.Text = background;
            }

            reader.Close();



            // Actualizar los campos con los valores de la habitación que
            // corresponda al id que el diseñador tenga puesto
        }

        private void RoomAddButton_Click(object sender, RoutedEventArgs e)
        {

            MySqlCommand command;


            roomId = RoomIdText.Text;
            roomName = RoomNameText.Text;
            roomDesc = RoomDescriptionText.Text;
            roomBackground = RoomBackgroundText.Text;

            string addRoom = "INSERT INTO Rooms VALUES(" + roomId + ", '" + roomName + "', '" + roomDesc + "', '" + roomBackground + "');";

            command = new MySqlCommand(addRoom, connection);
            MessageBox.Show(addRoom);
            command.ExecuteNonQuery();




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

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Rooms WHERE id=" + RoomIdText.Text + ";";

            int affectedRows = command.ExecuteNonQuery();


            if (affectedRows == 1)
            {
                MessageBox.Show("Eliminada habitacion");
            }
            else
            {
                MessageBox.Show("La habitacion no existe");
            }


            // Eliminar la habitación con el id que tenga puesto el diseñador
        }

        private void RoomListUpdateButton_Click(object sender, RoutedEventArgs e)
        {

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Rooms ORDER BY id;";


            //Reader es un iterador o cursor
            MySqlDataReader reader = command.ExecuteReader();

            string data = "";


            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string desc = reader.GetString(2);
                string background = reader.GetString(3);

                MessageBox.Show("id: " + id + " name: " + name + " description: " + desc + " background: " + background);
                data = data + "id: " + id + " name: " + name + " description: " + desc + " background: " + background + "\n";
            }

            RoomListText.Text = data;

            reader.Close();

            // Buscar todas las habitaciones y añadirlas a la lista.
        }
    }
}