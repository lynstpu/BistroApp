using BistroApp.Models;
using BistroApp.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BistroApp.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private readonly MenuPageViewModel vm;

        public MenuPage()
        {
            InitializeComponent();
            DataContext = vm = new MenuPageViewModel();
        }

        private void MenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            vm.LoadData();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CategoryOrderItem categoryOrderItem = ((ListViewItem)sender).Content as CategoryOrderItem;
            vm.SCOId = categoryOrderItem.SCOId;
            vm.OrderItemId = categoryOrderItem.OrderItemId;
            vm.OrderItemName = categoryOrderItem.OrderItemName;
            vm.Price = categoryOrderItem.Price;
            vm.Description = categoryOrderItem.Description;
            vm.SelectedSubCategory = vm.SubCategories.FirstOrDefault(sc => sc.Id == (int)categoryOrderItem.SubCategory);
            vm.SubCategoryOrderItemId = BistroDatabase.Instance.Get<int>($@"SELECT Id FROM SubCategory_OrderItem WHERE SubCategoryId = {vm.SelectedSubCategory.Id} AND OrderItemId = {vm.OrderItemId};").FirstOrDefault();
            vm.IsEditMode = true;
            vm.UpdateButtonEnabled = vm.CanUpdate();
            vm.DeleteButtonEnabled = vm.CanDelete();
        }
    }
}
