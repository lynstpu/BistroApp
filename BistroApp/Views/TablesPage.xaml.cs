using BistroApp.Models;
using BistroApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BistroApp.Views
{
    /// <summary>
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    public partial class TablesPage : Page
    {
        private TablesPageViewModel vm;

        public TablesPage()
        {
            InitializeComponent();
            DataContext = vm = new TablesPageViewModel();
        }

        private void TablesPage_Loaded(object sender, RoutedEventArgs e)
        {
            vm.LoadData();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Tables table = ((ListViewItem)sender).Content as Tables;
            vm.IsEditMode = true;
            vm.SelectedTable = table;
            vm.TableName = table.Name;
        }
    }
}