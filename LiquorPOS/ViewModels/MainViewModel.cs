using System; // For Exception
using System.Collections.Generic;
using System.Linq; // Required for .Sum()
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiquorPOS.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized; // Required for NotifyCollectionChangedEventArgs

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
        private ObservableCollection<Product> _scannedItemsList;

        // --- NEW: Property to hold the total price ---
        [ObservableProperty]
        private decimal _totalPrice;

        public MainViewModel()
        {
            ScannedItemsList = new ObservableCollection<Product>();
            // --- NEW: Subscribe to the CollectionChanged event ---
            ScannedItemsList.CollectionChanged += ScannedItemsList_CollectionChanged;
            TotalPrice = 0m; // Initialize
        }

        // --- NEW: Event handler for when the ScannedItemsList changes ---
        private void ScannedItemsList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateTotalPrice();
        }

        // --- NEW: Method to calculate the total price ---
        private void CalculateTotalPrice()
        {
            // Product.Price is decimal? (nullable decimal)
            // So, product.Price ?? 0m means "if product.Price is null, use 0m, otherwise use product.Price.Value"
            TotalPrice = ScannedItemsList.Sum(product => product.Price ?? 0m);
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

                        // Add to the list. The CollectionChanged event will trigger CalculateTotalPrice.
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ScannedItemsList.Add(product);
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
    }
}