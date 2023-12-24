using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.ObjectModel;



namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>
    public partial class Cashier : Window
    {// Создание необходимых переменных с областью видимости в пределах класса
        List<string> listOrder = new List<string>();
        private OleDbConnection sqlConnection = null;
        DishesFromMenu DFM = new DishesFromMenu();
        private string ingrediance;
        private string[] ingmass;       
        Payment payment = new Payment();
        private char[] charprice = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'Ц', 'е', 'н', 'а',' ','-' };

        public Cashier(string role)
        {
            InitializeComponent();
            // Login пользователя
            workerIDlabel.Content = role;
            // Открытие окна заказов
            
            payment.Show();
            // Получение списка продуктов из БД в string и string[]
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            payment.OneTime(ingrediance, ingmass,role);
            // Заполнение ComboBox с категориями товаров
            BoxSauce.ItemsSource = combine(DFM.FillBox("Dish", "sauce",6), DFM.FillBox("Price", "sauce",0));
            BoxCoffee.ItemsSource = combine(DFM.FillBox("Dish", "Coffee", 7), DFM.FillBox("Price", "Coffee", 0));
            BoxDesert.ItemsSource = combine(DFM.FillBox("Dish", "dessert",8), DFM.FillBox("Price", "dessert",0));
            BoxSnack.ItemsSource = combine(DFM.FillBox("Dish", "snack", 6), DFM.FillBox("Price", "snack", 0));
        }
        // Метод для сложения строк
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
            string relativePath = "Data\\NormBase.accdb";
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fullPath};";
            sqlConnection = new OleDbConnection(connectionString);
            sqlConnection.Open();
        }
        // Метод для подсчёта суммы заказа 
        public void addtosum(string item)
        {
            listOrder.Add(item.TrimEnd(charprice));
            //Groupe(item);
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
        // Метод для подсчёта суммы заказа при удалении позиции
        public void delfromsum(string item)
        {
            listOrder.Remove(item.Remove(5));
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
            allsum = allsum - x;
            Order.Items.Remove(Order.SelectedItem);
            OrderSum.Content = allsum;                      
            Order.SelectedItem = null;
        }
        // Метод находит количество продуктов необходимых для заказа
        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {   if (allsum==0)
            {
                return;
            }
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
        // Метод находит количество  продуктов в фургоне пользователя и номер заказа
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

        //Метод обновляет продукты в фургоне, отправляет заказ в ожидание и очищает поля для нового заказа
        private void UpDate(int[] order, int[] van, int count)
        {
            int[] update = new int[ingmass.Length];
            for (int i = 0; i < ingmass.Length; i++)
            {
                update[i] = van[i] - order[i];
                if (update[i] < 0)
                {
                    MessageBox.Show($"Нехватает {ingmass[i]}");
                    return;
                }
            }
            count++;
            string ing = "";
            for (int i = 0; i < ingmass.Length; i++)
            {
                ing = ing + $"{ingmass[i]}={update[i]}" + ",";
            }
            string Id = workerIDlabel.Content.ToString();
            string query = $"UPDATE Van SET {ing} Ordering={count} WHERE Account_van = '{Id}'";
            payment.UPDATE(query,listOrder,allsum.ToString(),count);
            //
            //
            Order.SelectedItem = null;
            clearall();           
        }
        // Метод очистки  
        private void clearall()
        {
            Order.SelectedItem = null;
            listOrder.Clear();
            Order.Items.Clear();
            OrderSum.Content = "0";
            allsum = 0;
        }
        // Метод проверяет выбранный товар и добавляет его в заказ
        private void BoxSelect(ComboBox box)
        {
            Order.SelectedItem = null;
            object item = box.SelectedItem;
            if (item == null)
            {

            }
            else
            {             
                addtosum(item.ToString());
                Order.Items.Insert(0, item.ToString());
            }
        }

        // методы для добавление товаров в заказ
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
        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = Order.SelectedItem;
            if (item == null)
            { }
            else
            {
                delfromsum(item.ToString());
            }           
        }
        // Удалить все позиции
        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            clearall();
        }
        //
    }
}
