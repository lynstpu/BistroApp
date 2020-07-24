using BistroApp.Models;
using BistroApp.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BistroApp.Views
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        private OrdersPageViewModel vm;

        public OrderPage()
        {
            InitializeComponent();
            DataContext = vm = new OrdersPageViewModel();
        }

        private void OrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            vm.LoadData();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TableOrderItems tableOrderItems = ((ListViewItem)sender).Content as TableOrderItems;
            vm.Quantity = tableOrderItems.Quantity;
            vm.SelectedOrderItem = vm.OrderItems.FirstOrDefault(x => x.Id == tableOrderItems.OrderItemId);
            vm.SelectedTable = vm.Tables.FirstOrDefault(x => x.Id == tableOrderItems.TableId);
            vm.IsEditMode = true;
            vm.TableDropdownEnabled = false;
            vm.OrderItemsDropdownEnabled = false;
            vm.UpdateButtonEnabled = vm.CanUpdate();
            vm.DeleteButtonEnabled = vm.CanDelete();
        }
    }
}
