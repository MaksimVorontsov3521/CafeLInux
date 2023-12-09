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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        private OleDbConnection sqlConnection = null;
        public ObservableCollection<MyDataItem> DataItems { get; set; }
        public Manager(string role)
        {
            InitializeComponent();
            workerIDlabel.Content = role;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);
            WorkersGridUPdate();
            ResultsGridUPdate();
            StorageGridUPdate();
        }
        private void WorkersGridUPdate()
        {
            sqlConnection.Open();
            string query = "Select * From Accounts Where Not Role_user = 'admin'";
            OleDbCommand command = new OleDbCommand(query, sqlConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            DataItems = new ObservableCollection<MyDataItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                MyDataItem item = new MyDataItem
                {
                    Login = row["Login_user"].ToString(),
                    Name = row["Name_user"].ToString(),
                    Surname = row["Surnam_user"].ToString(),
                    Password = row["Password_user"].ToString(),
                    Role = row["Role_user"].ToString(),
                    // Добавьте свойства по мере необходимости
                };
                DataItems.Add(item);
            }
            sqlConnection.Close();
            WorkersGrid.ItemsSource = DataItems;
        }
        private void ResultsGridUPdate()
        {
            sqlConnection.Open();

            // Выполнение SQL-запроса для получения данных
            string query = "SELECT * FROM Report";
            OleDbCommand command = new OleDbCommand(query, sqlConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Использование CollectionViewSource для виртуализации данных
            var collectionView = new CollectionViewSource { Source = dataTable.DefaultView };
            ResultGrid.ItemsSource = collectionView.View;

            // Закрытие соединения
            sqlConnection.Close();
        }
        private void StorageGridUPdate()
        {
            // Выполнение SQL-запроса для получения данных
            string query = "SELECT * FROM Storage";
            OleDbCommand command = new OleDbCommand(query, sqlConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Использование CollectionViewSource для виртуализации данных
            var collectionView = new CollectionViewSource { Source = dataTable.DefaultView };
            StorageGrid.ItemsSource = collectionView.View;

            // Закрытие соединения
            sqlConnection.Close();
        }

        private void WorkersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyDataItem selectedDataItem = (MyDataItem)WorkersGrid.SelectedItem;

            if (selectedDataItem != null)
            {
    
            }
        }
    }
    public class MyDataItem
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname{ get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
