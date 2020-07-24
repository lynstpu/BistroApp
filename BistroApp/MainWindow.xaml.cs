using BistroApp.Views;
using System.Windows;
using System.Windows.Media;

namespace BistroApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new MenuPage();
           
            MenuButton.Background = SelectedButtonColor;
            OrdersButton.Background = DefaultButtonColor;
            GenerateBillButton.Background = DefaultButtonColor;
            TablesButton.Background = DefaultButtonColor;
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OrderPage();

            MenuButton.Background = DefaultButtonColor;
            GenerateBillButton.Background = DefaultButtonColor;
            OrdersButton.Background = SelectedButtonColor;
            TablesButton.Background = DefaultButtonColor;
        }

        private void Tables_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TablesPage();

            MenuButton.Background = DefaultButtonColor;
            GenerateBillButton.Background = DefaultButtonColor;
            OrdersButton.Background = DefaultButtonColor;
            TablesButton.Background = SelectedButtonColor;
        }

        private void GenerateBillButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new GenerateBillPage();

            MenuButton.Background = DefaultButtonColor;
            OrdersButton.Background = DefaultButtonColor;
            GenerateBillButton.Background = SelectedButtonColor;
            TablesButton.Background = DefaultButtonColor;
        }

        public SolidColorBrush DefaultButtonColor => Brushes.DarkSeaGreen;

        public SolidColorBrush SelectedButtonColor => Brushes.DarkOrange;
    }
}
