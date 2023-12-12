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
    {// Создание необходимых переменных с областью видимости в пределах класса
        DishesFromMenu DFM = new DishesFromMenu();
        List<string> listOrder = new List<string>();
        private string IDVAN="" ;
        private string ingrediance;
        private string[] ingmass; 
        private int[] count;
        private int[] invan;
        private int selectedID;
        public Storage(string role)
        {
            InitializeComponent();
            // Login пользователя
            workerIDlabel.Content = role;
            // Список фургонов
            Vans.ItemsSource = DFM.FillVans(); 
            // Список ингридиентов
            ingrediance = DFM.Ingrediance(DFM.COLUMN_NAME("Menu"));
            ingmass = ingrediance.Split(',');
            Foodstuff.ItemsSource = ingmass;           
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DFM.StorageUpDate(count,IDVAN,ingmass,invan);
        }
        // Выбор фургона
        private void Vans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Van_id.Content = Vans.SelectedItem.ToString();
            IDVAN = Vans.SelectedItem.ToString();
            invan = DFM.VanFoodstuf(ingrediance, ingmass, IDVAN,"Van","Account_van");
            count = DFM.HaveToBeAdd(ingmass,ingrediance,invan);               
            Refresh();
            
        }
        // Обновление списка
        private void Refresh()
        {
            Foodstuff.ItemsSource = combine();
        }
        
        // Метод для сложения строк
        private string[] combine()
        {
            string[] combined = new string[ingmass.Length];
            
            for (int i = 0; i < count.Length; i++)
            {
                combined[i] = ingmass[i] + count[i];
            }
            return combined;
        }
        // Изменение фургона
        private void Foodstuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedID = (int)Foodstuff.SelectedIndex;
        }
        // Изменение количества продуктов 
        private void Сhange_Click(object sender, RoutedEventArgs e)
        {
            count[selectedID] = Convert.ToInt32(Quantity.Text);
            Refresh();
        }
    }
}
