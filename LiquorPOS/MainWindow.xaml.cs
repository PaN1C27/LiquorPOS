using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiquorPOS.ViewModels;

namespace LiquorPOS
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;                     // DI-provided VM
        }

        private void DataGridRow_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row) row.IsSelected = true;
        }
    }
}
