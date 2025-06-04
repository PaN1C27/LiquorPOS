using System.Windows;
// using System.Windows.Controls; // May not be needed if DataGridRow is not explicitly typed
// using System.Windows.Input; // May not be needed if KeyEventArgs/MouseButtonEventArgs are not used here

namespace LiquorPOS // Ensure this namespace is correct
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set initial focus to the barcode textbox when the window loads
            Loaded += (sender, e) =>
            {
                // Assuming your TextBox in XAML is named BarcodeEntryTextBox
                if (FindName("BarcodeEntryTextBox") is System.Windows.Controls.TextBox barcodeTextBox)
                {
                    barcodeTextBox.Focus();
                }
            };
        }

        // The DataGridRow_PreviewMouseRightButtonDown event handler has been REMOVED.
    }
}