using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel; // For ObservableObject
using LiquorPOS.Models; // Assuming your Product model is here

namespace LiquorPOS.ViewModels
{
    public partial class ScannedItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product _productDetails; // The actual product data

        [ObservableProperty]
        private int _quantity; // Quantity for this specific line item

        // Calculated property for display or binding, if needed
        public decimal LineItemPrice => (ProductDetails?.Price ?? 0m) * Quantity;

        public ScannedItemViewModel(Product product, int quantity)
        {
            _productDetails = product; // Use underscore for direct assignment to backing field
            _quantity = quantity;      // to avoid triggering PropertyChanged unnecessarily in constructor
        }
    }
}