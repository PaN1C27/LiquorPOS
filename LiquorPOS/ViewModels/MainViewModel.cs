using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiquorPOS.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows; // For MessageBox

namespace LiquorPOS.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        // These properties will automatically notify the View when changed
        [ObservableProperty]
        private string? _barcodeText; // Holds the text from the TextBox

        [ObservableProperty]
        private string _foundItemName = "---";

        [ObservableProperty]
        private string _foundItemPrice = "---";

        // This creates an ICommand called 'ScanBarcodeCommand'
        // which will execute the ScanBarcodeAsync method.
        [RelayCommand]
        private async Task ScanBarcodeAsync()
        {
            if (string.IsNullOrWhiteSpace(BarcodeText))
            {
                return; // Do nothing if input is empty
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
                BarcodeText = string.Empty; // Clear the input property
                                            // How to set focus back is a bit trickier in pure MVVM,
                                            // sometimes requires an "interaction" or small code-behind.
            }
        }
    }
}