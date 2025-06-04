using System.Windows;
using System.Windows.Controls; // Required for DataGridRow
using System.Windows.Input;   // Required for MouseButtonEventArgs

namespace LiquorPOS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // DataContext is set in XAML or could be set here:
            // this.DataContext = new ViewModels.MainViewModel();
        }

        // --- NEW Event Handler ---
        private void DataGridRow_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure the sender is a DataGridRow
            if (sender is DataGridRow row)
            {
                // If the row is not already selected, select it.
                // This ensures that the DataGrid.SelectedItem (and thus MainViewModel.SelectedItem)
                // is updated to the item being right-clicked *before* the ContextMenu opens.
                if (!row.IsSelected)
                {
                    row.IsSelected = true;
                }

                // The DataContext of the 'row' is the ScannedItemViewModel instance.
                // By setting row.IsSelected = true, the DataGrid's SelectedItem property
                // (which is bound to MainViewModel.SelectedItem) will update.
                // This, in turn, triggers the CanExecute re-evaluation for your commands.
            }
        }

        // Your BarcodeTextBox_KeyDown method has been removed as it's handled by MainViewModel
    }
}