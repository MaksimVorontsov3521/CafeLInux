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
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OleDbConnection sqlConnection = null;
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            // метод для входа по login и password
            string x = Enter();
            // получение роли сотрудника
            string role = Role();
            if (x == "cashier")
            {
                Cashier cashier = new Cashier(role);
                cashier.Show();
                this.Hide();
                
            }
            else if (x == "storage")
            {
                Storage storage = new Storage(role);
                storage.Show();
                this.Hide();
                
            }
            // если неверные login или password
            else
            {
                MessageBox.Show("not yet");
                sqlConnection.Close();
            }

            
        }
        // получение роли из БД
        private string Role()
        {
            using (OleDbCommand com = new OleDbCommand($"Select Login_user From Accounts Where Login_user=@l And Password_user=@p ", sqlConnection))
            {
                com.Parameters.AddWithValue("l", Login.Text);
                com.Parameters.AddWithValue("p", Password.Text);
                object result = com.ExecuteScalar();
                string val = (result != null) ? result.ToString() : "Значение отсутствует";
                return val;
            }

        }
        // Поиск login и password в БД
        private string Enter()
        {
            using (OleDbCommand com = new OleDbCommand($"Select Role_user From Accounts Where Login_user=@l And Password_user=@p ", sqlConnection))
            {
                com.Parameters.AddWithValue("l", Login.Text);
                com.Parameters.AddWithValue("p", Password.Text);
                object result = com.ExecuteScalar();
                string val = (result != null) ? result.ToString() : "Значение отсутствует";
                return val;
            }         
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);
            
        }
    }
}
