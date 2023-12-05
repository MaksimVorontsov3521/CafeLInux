using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>
    public partial class Cashier : Window
    {
        List<string> listOrder = new List<string>();
        private OleDbConnection sqlConnection = null;
        DishesFromMenu DFM = new DishesFromMenu();
        public string ingrediance;
        public string[] ingmass;
        public Cashier(string role)
        {
            InitializeComponent();
            workerIDlabel.Content = role;
            Orders.Show();
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            BoxSauce.ItemsSource = combine(DFM.FillBox("Dish", "sauce",6), DFM.FillBox("Price", "sauce",0));
            BoxCoffee.ItemsSource = combine(DFM.FillBox("Dish", "Coffee", 7), DFM.FillBox("Price", "Coffee", 0));
            BoxDesert.ItemsSource = combine(DFM.FillBox("Dish", "dessert",8), DFM.FillBox("Price", "dessert",0));
            BoxSnack.ItemsSource = combine(DFM.FillBox("Dish", "snack", 6), DFM.FillBox("Price", "snack", 0));
        }
        private List<string> combine(List<string> one, List<string> two)
        {
            List<string> combined = new List<string>();
            for (int i = 0; i < one.Count; i++)
            {
                combined.Add(one[i] + " Цена - " + two[i]);
            }
            return combined;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);
            sqlConnection.Open();
        }

        public void addtosum(string item)
        {
            listOrder.Add(item.Remove(6));
            char[] arr;
            arr = item.ToCharArray();
            Array.Reverse(arr);
            Array.Resize(ref arr, 3);
            Array.Reverse(arr);
            item = null;
            for (int i = 0; i < arr.Length; i++)
            {
                item = item + arr[i].ToString();
            }
            int x = Convert.ToInt32(item);

            if (x < 0)
            {
                x = x * -1;
            }
            allsum = allsum + x;
            OrderSum.Content = allsum;
        }
        public int allsum = 0;

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            int[] order = new int[ingmass.Length];
            string query;
            string dynamicCondition;
            string between = "";
            for (int i = 0; i < listOrder.Count; i++)
            {
                dynamicCondition = $"Dish Like'%{listOrder[i]}%'";
                query = $"SELECT {ingrediance} FROM Menu WHERE {dynamicCondition}";

                using (OleDbCommand com = new OleDbCommand(query, sqlConnection))
                {
                    using (OleDbDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int j = 0; j < ingmass.Length; j++)
                            {
                                between = reader[$"{ingmass[j]}"] != DBNull.Value ? reader[$"{ingmass[j]}"].ToString() : null;
                                order[j] = order[j] + Convert.ToInt32(between);
                            }
                        }
                    }
                }
            }
            RemoveFromVan(order);
        }
        private void RemoveFromVan(int[] order)
        {
            int count = 0;
            int[] van = new int[ingmass.Length];
            string between = "";
            string query = $"SELECT {ingrediance},Ordering FROM Van WHERE Account_van='{workerIDlabel.Content.ToString()}' ";
            using (OleDbCommand com = new OleDbCommand(query, sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int j = 0; j < ingmass.Length; j++)
                        {
                            between = reader[$"{ingmass[j]}"] != DBNull.Value ? reader[$"{ingmass[j]}"].ToString() : null;
                            van[j] = van[j] + Convert.ToInt32(between);
                        }
                        between = reader[$"Ordering"] != DBNull.Value ? reader[$"Ordering"].ToString() : null;
                        count = count + Convert.ToInt32(between);
                    }
                }
            }
            UpDate(order, van, count);
        }
        Orders Orders = new Orders();
        private void UpDate(int[] order, int[] van, int count)
        {
            count++;
            string ing = "";
            for (int i = 0; i < ingmass.Length; i++)
            {
                ing = ing + $"{ingmass[i]}={van[i] - order[i]}" + ",";
            }
            string Id = workerIDlabel.Content.ToString();
            string query = $"UPDATE Van SET {ing} Ordering={count} WHERE Account_van = '{Id}'";
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
            Orders.NewItem(listOrder, OrderSum.Content.ToString(), count);
            listOrder.Clear();
            Order.Items.Clear();
            OrderSum.Content = "0";
            allsum = 0;
        }

        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BoxSelect(ComboBox box)
        {
            object item = box.SelectedItem;
            if (item == null)
            {

            }
            else
            {
                Order.Items.Insert(0, item.ToString());
                addtosum(item.ToString());
            }
        }
        private void BoxCoffee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                BoxSelect(BoxCoffee);
        }
        private void BoxDesert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                BoxSelect(BoxDesert);
        }
        private void BoxSauce_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               BoxSelect(BoxSauce);
        }
        private void BoxSnack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               BoxSelect(BoxSnack);
        }
        private void AddCoffee_Click(object sender, RoutedEventArgs e)
        {
                BoxSelect(BoxCoffee);
        }
        private void AddDesert_Click(object sender, RoutedEventArgs e)
        {
                BoxSelect(BoxDesert);
        }
        private void AddSauce_Click(object sender, RoutedEventArgs e)
        {
                BoxSelect(BoxSauce);
        }
        private void AddSnack_Click(object sender, RoutedEventArgs e)
        {
                BoxSelect(BoxSnack);
        }
    }
}
