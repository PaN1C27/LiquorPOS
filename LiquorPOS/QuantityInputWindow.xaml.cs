using System.Text.RegularExpressions;
using System.Windows;
namespace LiquorPOS
{
    public partial class QuantityInputWindow : Window
    {
        public int EnteredQuantity { get; private set; }

        public QuantityInputWindow(int currentQty)
        {
            InitializeComponent();
            QtyTextBox.Text = currentQty.ToString();
            QtyTextBox.SelectAll();
            QtyTextBox.Focus();
        }

        // Only allow digits
        private void QtyTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) => e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QtyTextBox.Text, out var qty) && qty >= 0)
            {
                EnteredQuantity = qty;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid non-negative integer.");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
