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
            Order.ItemsSource = normlist(listOrder);
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
            if (Convert.ToInt32(Change.Content) == 0)
            {
                check = true;
                transaction(check);
            }
            else if(Convert.ToInt32(Change.Content)>0)
            {
                MessageBox.Show("Не хватет средств", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
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
                Orders.NewItem(normlist(listOrder), OrderSum, count);
                Change.Content = "0";
                OrderSumLabel.Content = "0";
                CashSum.Text = null;
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
            CashSum.Text = "0";
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

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            Change.Content = Convert.ToInt32(OrderSum) - Convert.ToInt32(CashSum.Text);
        }

        private void _1_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 1).ToString();
        }

        private void _2_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 2).ToString();
        }

        private void _5_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 5).ToString();
        }

        private void _10_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 10).ToString();
        }

        private void _50_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 50).ToString();
        }

        private void _100_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 100).ToString();
        }

        private void _200_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 200).ToString();
        }

        private void _500_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 500).ToString();
        }

        private void _1000_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 1000).ToString();
        }

        private void _2000_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 2000).ToString();
        }

        private void _5000_Click(object sender, RoutedEventArgs e)
        {
            CashSum.Text = (Convert.ToInt32(CashSum.Text) + 5000).ToString();
        }

        private void NotOK_Click(object sender, RoutedEventArgs e)
        {
            Change.Content = "0";
            OrderSumLabel.Content = "0";
            CashSum.Text = null;
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
