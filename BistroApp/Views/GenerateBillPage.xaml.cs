using BistroApp.Extensions;
using BistroApp.Models;
using BistroApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BistroApp.Views
{
    /// <summary>
    /// Interaction logic for GenerateBillPage.xaml
    /// </summary>
    public partial class GenerateBillPage : Page
    {
        private GenerateBillPageViewModel vm;

        public GenerateBillPage()
        {
            InitializeComponent();
            DataContext = vm = new GenerateBillPageViewModel();
        }

        private void GenerateBillPage_Loaded(object sender, RoutedEventArgs e)
        {
            vm.LoadData();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tables table = ((ComboBox)sender).SelectedItem as Tables;
            vm.TableOrderItems = BistroDatabase.Instance.Get<TableOrderItems>(DBQuery.GET_TABLE_ORDER_ITEMS).Where(x => x.TableId == table.Id).ToObservableCollection();

            foreach(TableOrderItems item in vm.TableOrderItems)
            {
                vm.Total += item.TotalPrice; 
            }

            if (vm.TableOrderItems.Count() == 0)
            {
                vm.Total = 0;
            }
        }
    }
}
