﻿using System;
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

namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        DishesFromMenu DFM = new DishesFromMenu();
        private string ingrediance;
        private string[] ingmass;
        private int[] count;
        private int selectedID;
        public Admin(string role)
        {
            InitializeComponent();
            You.Content = role;
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            count = new int[ingmass.Length];
            Products.ItemsSource = combine();
            List<string> category = new List<string>() {"Coffee", "dessert", "sauce", "snack"};
            Category.ItemsSource = category;
            Disheslist.ItemsSource = DFM.Alldishes();
        }
        private string[] combine()
        {
            string[] combined = new string[ingmass.Length];
            for (int i = 0; i < ingmass.Length; i++)
            {
                combined[i] = ingmass[i] +" "+ count[i];
            }
            return combined;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            count[selectedID] = Convert.ToInt32(Quantity.Text);
            Refresh();
        }

        private void Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedID = (int)Products.SelectedIndex;
        }
        private void Refresh()
        {
            Products.ItemsSource = combine();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            DFM.AddIteminMenu(Category.SelectedValue.ToString(),Name.Text,Convert.ToInt32(Price.Text),ingmass,count);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            string item =Disheslist.SelectedItem.ToString();
            DFM.Deleteitem(item);
        }
    }
}