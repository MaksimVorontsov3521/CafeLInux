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
            Disheslist.ItemsSource = DFM.AllSomething("Dish","Menu");
        }
        // метод для объединения сторк в одну
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
        {// проверка на соответствие условию
            if (Products.SelectedItem == null && Quantity.Text=="")
            {

            }
            else
            {// проверка на соответствие условию
                try
                {
                    count[selectedID] = Convert.ToInt32(Quantity.Text);
                    Refresh();
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, Введите цыфры.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

                }

            }
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
        {// проверка на соответствие условию
            if (Category.SelectedValue == null && Name.Text == "")
            {
                MessageBox.Show("Пожалуйста, заполните все Поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // проверка уникальность
            if (DFM.UniqeDish(Category.SelectedValue.ToString(), Name.Text)!=0)
            {
                MessageBox.Show("Такое блюдо уже существует.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Выполнение метода
            DFM.AddIteminMenu(Category.SelectedValue.ToString(),Name.Text,Convert.ToInt32(Price.Text),ingmass,count);
            DFM.Addcolumn(Category.SelectedValue.ToString(), Name.Text);
        }


        private void Del_Click(object sender, RoutedEventArgs e)
        {// проверка на соответствие условию
            if (Disheslist.SelectedItem==null)
            {
                return;
            }
                string item = Disheslist.SelectedItem.ToString();
                DFM.Deleteitem(item);
        }

        private void AddNewItem_Click(object sender, RoutedEventArgs e)
        {// проверка на соответствие условию
            if (NewItem.Text=="")
            {
                return;
            }
            DFM.AddNewItem(NewItem.Text);
        }
    }
}
