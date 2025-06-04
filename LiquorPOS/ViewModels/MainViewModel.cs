using System; // For Exception
using System.Collections.Generic;
using System.Linq; // Required for .Sum() and .Any()
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiquorPOS.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LiquorPOS.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _barcodeText;

        [ObservableProperty]
        private string _foundItemName = "---";

        [ObservableProperty]
        private string _foundItemPrice = "---";

        [ObservableProperty]
        private ObservableCollection<ScannedItemViewModel> _scannedItemsList;

        [ObservableProperty]
        private decimal _totalPrice;

        // --- NEW: Property to hold the currently selected item in the DataGrid ---
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteItemCommand))] // Update command status when SelectedItem changes
        [NotifyCanExecuteChangedFor(nameof(EditQuantityCommand))]// Update command status when SelectedItem changes
        private ScannedItemViewModel? _selectedItem;

        public MainViewModel()
        {
            ScannedItemsList = new ObservableCollection<ScannedItemViewModel>();
            ScannedItemsList.CollectionChanged += ScannedItemsList_CollectionChanged;
            TotalPrice = 0m;
        }

        private void ScannedItemsList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = ScannedItemsList.Sum(item => (item.ProductDetails?.Price ?? 0m) * item.Quantity);
        }

        [RelayCommand]
        private async Task ScanBarcodeAsync()
        {
            // ... (ScanBarcodeAsync logic remains the same as before)
            if (string.IsNullOrWhiteSpace(BarcodeText)) return;
            string scannedBarcode = BarcodeText.Trim();
            FoundItemName = "Searching...";
            FoundItemPrice = "---";
            try
            {
                await using (var context = new LiquorDbContext())
                {
                    var barcodeEntry = await context.Barcodes
                                                 .Include(b => b.Product)
                                                 .FirstOrDefaultAsync(b => b.BarcodeValue == scannedBarcode);
                    if (barcodeEntry != null && barcodeEntry.Product != null)
                    {
                        var product = barcodeEntry.Product;
                        FoundItemName = $"{product.Brand} {product.Description}".Trim();
                        FoundItemPrice = $"{product.Price:C}";
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var newItem = new ScannedItemViewModel(product, 1);
                            ScannedItemsList.Add(newItem);
                        });
                    }
                    else
                    {
                        FoundItemName = "Item Not Found";
                        FoundItemPrice = "---";
                    }
                }
            }
            catch (Exception ex)
            {
                FoundItemName = "DATABASE ERROR!";
                FoundItemPrice = "Check connection or logs.";
                MessageBox.Show(ex.ToString(), "Database Exception Details");
            }
            finally
            {
                BarcodeText = string.Empty;
            }
        }

        [RelayCommand]
        private void VoidSale()
        {
            // ... (VoidSale logic remains the same as before)
            if (ScannedItemsList.Any())
            {
                ScannedItemsList.Clear();
                FoundItemName = "---";
                FoundItemPrice = "---";
            }
            BarcodeText = string.Empty;
        }

        // --- NEW: Method to determine if Delete/Edit commands can execute ---
        private bool CanModifyOrDeleteItem()
        {
            return SelectedItem != null;
        }

        // --- NEW: Command to Delete an Item ---
        // The CanExecute parameter links this command's enabled state to CanModifyOrDeleteItem
        [RelayCommand(CanExecute = nameof(CanModifyOrDeleteItem))]
        private void DeleteItem(ScannedItemViewModel? itemToDelete)
        {
            // Use the parameter if provided (from ContextMenu), otherwise use SelectedItem (from DEL key)
            var itemToRemove = itemToDelete ?? SelectedItem;

            if (itemToRemove != null && ScannedItemsList.Contains(itemToRemove))
            {
                ScannedItemsList.Remove(itemToRemove);
                // TotalPrice updates via CollectionChanged automatically
                // If the removed item was the selected one, clear the selection
                if (SelectedItem == itemToRemove)
                {
                    SelectedItem = null;
                }
            }
        }

        // --- NEW: Command to Edit Quantity (Placeholder for now) ---
        [RelayCommand(CanExecute = nameof(CanModifyOrDeleteItem))]
        private void EditQuantity(ScannedItemViewModel? itemToEdit)
        {
            var itemToModify = itemToEdit ?? SelectedItem;
            if (itemToModify != null)
            {
                // For now, just show a message. Later, you'd open a dialog or enable inline editing.
                MessageBox.Show($"Placeholder: Edit quantity for '{itemToModify.ProductDetails?.Description}'. Current Qty: {itemToModify.Quantity}", "Edit Quantity");

                // Example of how you might update quantity if you had an input:
                // string newQtyStr = Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Edit Quantity", itemToModify.Quantity.ToString());
                // if (int.TryParse(newQtyStr, out int newQty) && newQty > 0)
                // {
                //     itemToModify.Quantity = newQty; // Assuming ScannedItemViewModel.Quantity is an [ObservableProperty]
                //     CalculateTotalPrice(); // Manually trigger if Quantity in ScannedItemViewModel doesn't trigger collection changed for sum
                // }
            }
        }
    }
}