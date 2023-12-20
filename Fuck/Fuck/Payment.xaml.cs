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
using System.Configuration;
using System.Data.OleDb;


namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        Orders Orders = new Orders();
        DishesFromMenu DFM = new DishesFromMenu();
        private OleDbConnection sqlConnection = null;
        string query="";
        string OrderSum = "";
        int count = 0;
        string Id;
        string ingrediance;
        string[] ingmass;
        string[] listOrder;
        public Payment()
        {
            InitializeComponent();
            Orders.Show();
        }

        public void UPDATE(string query,List<string> listOrder,string OrderSum,int count)
        {
            this.query = query;
            string[] boof=new string[listOrder.Count];
            for (int i = 0; i < listOrder.Count; i++)
            {
                boof[i] = listOrder[i];
            }
            this.listOrder = boof;
            this.OrderSum = OrderSum;
            OrderSumLabel.Content = OrderSum;
            this.count = count;          
        }
        public void OneTime(string ingrediance,string [] ingmass, string Id)
        {
            this.ingmass = ingmass;
            this.ingrediance = ingrediance;
            this.Id = Id;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            transaction(check);
        }
        private void transaction(bool check)
        {
            if (check == true)
            {
                List<string> listOrder = new List<string>();
                for (int i=0;i<this.listOrder.Length;i++)
                {
                    listOrder.Add(this.listOrder[i]);
                }
                OleDbCommand com = new OleDbCommand(query, sqlConnection);
                com.ExecuteNonQuery();
                DFM.CashirReport(listOrder, Id, ingrediance, ingmass);
                Orders.NewItem(listOrder, OrderSum, count);
                MessageBox.Show("Оплата прошла успешно");
            }
            else
            { 
            MessageBox.Show("Не хватет средств", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);
            sqlConnection.Open();
        }
        public List<string> normlist(List<string> listOrder)
        {
            List<string> normlistOrder = new List<string>();
            var counts = listOrder
                .GroupBy(n => n)
                .Select(g => new { Dish = g.Key, Count = g.Count() })
                .ToList();
            foreach (var count in counts)
            {
                normlistOrder.Add($"{count.Dish} - {count.Count}");
            }
            return normlistOrder;
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            Bank bank = new Bank();
            transaction(bank.Account(Convert.ToInt32(OrderSum)));
        }

        private void _1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class Bank
    {
        public Bank()
        {

        }
        public bool Account(int Price)
        {
            bool a = transaction(Price);
            return a;
        }
        private bool transaction(int Price)
        {
            return true;
        }
    }
}
