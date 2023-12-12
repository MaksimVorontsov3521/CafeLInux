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
    /// Логика взаимодействия для AddWorkrer.xaml
    /// </summary>
    public partial class AddWorkrer : Window
    {
        private OleDbConnection sqlConnection = null;
        public AddWorkrer()
        {  
            InitializeComponent();
            string[] Roles = new string[] { "cashier", "storage", "manager" };
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);
            sqlConnection.Open();
            Role.ItemsSource = Roles;

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<TextBox> textBoxes = FindVisualChildren<TextBox>(this);

            if (AreTextBoxesFilled())
            {
                if (UniqeLogin() != 0)
                {
                    MessageBox.Show("Login - занят");
                }
                else if (Role.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, заполните все Поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string query = $"Insert Into Accounts (Login_user,Password_user,Name_user,Surnam_user,Role_user)" +
                        $" Values('{Login.Text}','{Password.Text}','{Name.Text}','{Suname.Text}','{Role.SelectedValue}')";
                        OleDbCommand com = new OleDbCommand(query, sqlConnection);
                        com.ExecuteNonQuery();
                    if (Role.SelectedValue == "cashier")
                    {
                        AddVan();
                    }
                    this.Close();              
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все Поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private int UniqeLogin()
        {
            string query = $"Select Login_user From Accounts Where Login_user='{Login.Text}'";
            using (OleDbCommand com = new OleDbCommand(query, sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader["Login_user"] != DBNull.Value ? reader["Login_user"].ToString() : null;
                        values.Add(value);
                    }
                    return values.Count();
                }
            }

        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private bool AreTextBoxesFilled()
        {
            // Получаем все TextBox внутри контейнера (например, Grid, StackPanel, или Window)
            IEnumerable<TextBox> textBoxes = FindVisualChildren<TextBox>(this);

            // Проверяем, что все TextBox имеют значения
            foreach (TextBox textBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return false; // Хотя бы один TextBox не заполнен
                }
            }

            return true; // Все TextBox заполнены
        }

        public List<string> Allaccunts(string somethig, string where,string column, string equals)
        {
            using (OleDbCommand com = new OleDbCommand($"Select {somethig} From {where} Where {column}={equals}", sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader[somethig] != DBNull.Value ? reader[somethig].ToString() : null;
                        values.Add(value);
                    }
                    return values;
                }
            }
        }

        private void AddVan()
        {
            string query = $"Insert Into Van (Account_van) Values('{Login.Text}')";
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
            query = $"Insert Into Report (N_Van) Values('{Login.Text}')";
            com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
            query = $"Insert Into Storage (Id_van) Values('{Login.Text}')";
            com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
        }

    }
}
