using CommunityToolkit.Mvvm.ComponentModel; // For ObservableObject and [ObservableProperty]
using LiquorPOS.Models; // Assuming your Product model is in this namespace

namespace LiquorPOS.ViewModels // Or LiquorPOS.Models, depending on your project structure
{
    /// <summary>
    /// Represents a single item that has been scanned into the list.
    /// It holds the product details and the quantity for this specific line item.
    /// Inherits from ObservableObject to support property change notifications if needed later
    /// (e.g., if you wanted to allow inline editing of quantity directly in the DataGrid).
    /// </summary>
    public partial class ScannedItemViewModel : ObservableObject
    {
        // The [ObservableProperty] attribute from CommunityToolkit.Mvvm
        // will auto-generate the public property and the INotifyPropertyChanged logic.
        // The private field name (e.g., _productDetails) is a convention.

        [ObservableProperty]
        private Product _productDetails;

        [ObservableProperty]
        private int _quantity;

        // ── NEW: columns requested by the grid ───────────────────────
        public int LineNumber { get; init; }

        public string? Brand => ProductDetails?.Brand;                       // :contentReference[oaicite:2]{index=2}
        public string? Description => ProductDetails?.Description;
        public string? Size => ProductDetails?.Size;
        public string? Pack => ProductDetails?.QtyCase?.ToString();         // “Pack” ≈ QtyCase
        public decimal? Price => ProductDetails?.Price;

        // optional values—leave null for blank cells
        [ObservableProperty] private decimal? _tax;
        [ObservableProperty] private decimal? _deposit;
        [ObservableProperty] private decimal? _discount;
        [ObservableProperty] private decimal? _sale;

        public decimal? Extended => (Price ?? 0m - (Discount ?? 0m) + (Deposit ?? 0m) + (Tax ?? 0m)) * Quantity;

        public ScannedItemViewModel(Product product, int qty, int lineNo)
        {
            _productDetails = product;
            _quantity = qty;
            LineNumber = lineNo;
        }

        /// <summary>
        /// Constructor for a new scanned item.
        /// </summary>
        /// <param name="product">The product that was scanned.</param>
        /// <param name="quantity">The quantity for this line item (typically 1 per scan in your current setup).</param>
        public ScannedItemViewModel(Product product, int quantity)
        {
            // Directly set the backing fields in the constructor if you don't want to
            // trigger PropertyChanged notifications during initial object creation.
            // The source generator for [ObservableProperty] creates these backing fields.
            // If you type `_productDetails =` and it's not recognized immediately,
            // try building the project once so the source generators can run.
            // Alternatively, use the generated public properties:
            // ProductDetails = product;
            // Quantity = quantity;
            // For simplicity and clarity in constructor, direct field assignment is often okay
            // if no side-effects are tied to the setters yet.
            // However, the MVVM toolkit often expects you to use the generated property
            // to ensure all its mechanisms are engaged.
            // Let's use the generated properties to be safe with the toolkit's intended use.

            ProductDetails = product;
            Quantity = quantity;
        }

        // Example of a calculated property (read-only for now) if you need it for display
        // public decimal LineItemTotal => (ProductDetails?.Price ?? 0m) * Quantity;
        // If LineItemTotal were bound in the UI and needed to update when Quantity or Price changes,
        // you would add [NotifyPropertyChangedFor(nameof(LineItemTotal))] to the Quantity and Price properties
        // (Price is in ProductDetails, so that's more complex and would require ProductDetails to be an ObservableObject too,
        // or for ScannedItemViewModel to raise the change for LineItemTotal when ProductDetails.Price changes).
        // For now, the main total is handled in MainViewModel.
    }
}