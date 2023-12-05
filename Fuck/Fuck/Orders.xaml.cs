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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            InitializeComponent();
        }
        
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Remove(ListBox.SelectedItem);
        }

        public void NewItem(List<string>OrderList,string Total,int number)
        {
            string resalt = "";
            for (int i = 0; i < OrderList.Count; i++)
            {
                resalt = resalt +" "+  OrderList[i]+",";              
            }
            
            resalt = resalt + " Цена -" + Total +" Номер заказа-"+ number.ToString();
            ListBox.Items.Insert(0, resalt);
        }
    }
}
