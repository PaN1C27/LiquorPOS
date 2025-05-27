using System.Windows;

namespace LiquorPOS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // We set the DataContext in XAML now, but you could
            // set it here instead: this.DataContext = new ViewModels.MainViewModel();
        }

        // The BarcodeTextBox_KeyDown method is now GONE!
    }
}