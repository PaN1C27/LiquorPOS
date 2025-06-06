using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiquorPOS.Models; // Ensure this namespace is correct for your Product, Barcode models
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LiquorPOS;

namespace LiquorPOS.ViewModels // Ensure this namespace is correct
{
    public partial class MainViewModel : BaseViewModel // Assuming BaseViewModel is set up
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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteItemCommand))] // Only notify for DeleteItemCommand now
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
            if (string.IsNullOrWhiteSpace(BarcodeText))
            {
                return;
            }

            string scannedBarcode = BarcodeText.Trim();
            FoundItemName = "Searching...";
            FoundItemPrice = "---";

            try
            {
                await using (var context = new LiquorDbContext()) // Ensure LiquorDbContext is correctly named and configured
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
            if (ScannedItemsList.Any())
            {
                ScannedItemsList.Clear();
                FoundItemName = "---";
                FoundItemPrice = "---";
            }
            BarcodeText = string.Empty;
        }

        // This method determines if DeleteItemCommand can execute
        private bool CanModifyOrDeleteItem()
        {
            return SelectedItem != null;
        }

        // Command to Delete an Item (used by DEL key)
        [RelayCommand(CanExecute = nameof(CanModifyOrDeleteItem))]
        private void DeleteItem(ScannedItemViewModel? itemToDelete)
        {
            // For DEL key, itemToDelete will be the SelectedItem passed as CommandParameter
            var itemToRemove = itemToDelete ?? SelectedItem;

            if (itemToRemove != null && ScannedItemsList.Contains(itemToRemove))
            {
                ScannedItemsList.Remove(itemToRemove);
                if (SelectedItem == itemToRemove)
                {
                    SelectedItem = null;
                }
            }
        }

        [RelayCommand]
        private void ShowProducts()
        {
            var window = new ProductListWindow();
            window.Show();
        }

        // EditQuantityCommand has been removed
    }
}