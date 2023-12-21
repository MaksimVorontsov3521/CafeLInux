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
    /// Логика взаимодействия для StandardForStorage.xaml
    /// </summary>
    public partial class StandardForStorage : Window
    {
        DishesFromMenu DFM = new DishesFromMenu();
        private string ingrediance;
        private string[] ingmass;
        private int[] count;
        private int selectedID;
        public StandardForStorage()
        {
            InitializeComponent();
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            count = DFM.VanFoodstuf(ingrediance,ingmass,"0","Storage","Id_van");
            Products.ItemsSource = combine();
        }
        // Объединение строк в одну
        private string[] combine()
        {
            string[] combined = new string[ingmass.Length];
            for (int i = 0; i < ingmass.Length; i++)
            {
                combined[i] = ingmass[i] + " " + count[i];
            }
            return combined;
        }
        // изменение колличества
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (Products.SelectedItem == null && Quantity.Text == "")
            {

            }
            else
            {
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
        private void Refresh()
        {
            Products.ItemsSource = combine();
        }

        private void Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedID = (int)Products.SelectedIndex;
        }
        // Выполнение
        private void Aply_Click(object sender, RoutedEventArgs e)
        {
            DFM.Standart(ingmass,count);
            this.Close();
        }
    }
}
