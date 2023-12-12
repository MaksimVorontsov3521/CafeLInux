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
        public ObservableCollection<DataItemsVan> DataItemsVan { get; set; }
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
            VansUPdate();
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
        private void VansUPdate()
        {
            sqlConnection.Open();
            string query = "Select * From Van ";
            OleDbCommand command = new OleDbCommand(query, sqlConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            DataItemsVan = new ObservableCollection<DataItemsVan>();
            foreach (DataRow row in dataTable.Rows)
            {
                DataItemsVan item = new DataItemsVan
                {
                    Account = row["Account_van"].ToString(),
                    Ordering = row["Ordering"].ToString(),
                    // Добавьте свойства по мере необходимости
                };
                DataItemsVan.Add(item);
            }
            sqlConnection.Close();
            VansGrid.ItemsSource = DataItemsVan;
        }

        private void WorkersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            MyDataItem selectedDataItem = (MyDataItem)WorkersGrid.SelectedItem;
            
            if (selectedDataItem != null)
            {
                sqlConnection.Open();
                string query = $"DELETE FROM Accounts WHERE Login_user = '{selectedDataItem.Login}'";
                OleDbCommand com = new OleDbCommand(query, sqlConnection);
                com.ExecuteNonQuery();
                query = $"DELETE FROM Van WHERE Account_van = '{selectedDataItem.Login}'";
                com = new OleDbCommand(query, sqlConnection);
                com.ExecuteNonQuery();
                query = $"DELETE FROM Report WHERE N_Van = '{selectedDataItem.Login}'";
                com = new OleDbCommand(query, sqlConnection);
                com.ExecuteNonQuery();
                query = $"DELETE FROM Storage WHERE Id_van = '{selectedDataItem.Login}'";
                com = new OleDbCommand(query, sqlConnection);
                com.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        private void Addorkerbutton_Click(object sender, RoutedEventArgs e)
        {
            AddWorkrer addWorkrer = new AddWorkrer();
            addWorkrer.Show();          
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            WorkersGridUPdate();
            ResultsGridUPdate();
            StorageGridUPdate();
            VansUPdate();
        }

        private void Standard_Click(object sender, RoutedEventArgs e)
        {
            StandardForStorage SFS = new StandardForStorage();
            SFS.Show();
        }
    }
    public class DataItemsVan
    {
        public string Account { get; set; }
        public string Ordering { get; set; }
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
