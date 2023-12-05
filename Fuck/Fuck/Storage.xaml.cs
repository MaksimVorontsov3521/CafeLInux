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
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Fuck
{
    /// <summary>
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Window
    {
        DishesFromMenu DFM = new DishesFromMenu();
        List<string> listOrder = new List<string>();
        private string IDVAN = "";
        private string ingrediance;
        private string[] ingmass;
        public Storage(string role)
        {
            InitializeComponent();
            workerIDlabel.Content = role;
            Vans.ItemsSource = DFM.FillVans();          
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            Foodstuff.ItemsSource = ingmass;
            
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Vans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Van_id.Content = Vans.SelectedItem.ToString();
            IDVAN = Vans.SelectedItem.ToString();
            count = DFM.VanFoodstuf(ingrediance, ingmass, IDVAN);
            Refresh();
            
        }
        private void Refresh()
        {
            Foodstuff.ItemsSource = combine();
        }
        private int[] count;
        private string[] combine()
        {
            string[] combined = new string[ingmass.Length];
            
            for (int i = 0; i < count.Length; i++)
            {
                combined[i] = ingmass[i] + count[i];
            }
            return combined;
        }
        private int selectedID;
        private void Foodstuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedID = (int)Foodstuff.SelectedIndex;
        }
        
        private void Сhange_Click(object sender, RoutedEventArgs e)
        {
            count[selectedID] = Convert.ToInt32(Quantity.Text);
            Refresh();
        }
    }
}
