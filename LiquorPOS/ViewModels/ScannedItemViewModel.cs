using CommunityToolkit.Mvvm.ComponentModel;
using LiquorPOS.Models;
using LiquorPOS.Services;

namespace LiquorPOS.ViewModels;

public partial class ScannedItemViewModel : ObservableObject
{
    // ── ctor & basic fields ────────────────────────────────────────
    public ScannedItemViewModel(Product p, int qty, int lineNo)
    {
        ProductDetails = p;
        Quantity = qty;
        LineNumber = lineNo;
    }

    [ObservableProperty] private Product _productDetails = null!;
    [ObservableProperty] private int _quantity;

    [ObservableProperty] private int _lineNumber;
    public string? Brand => ProductDetails.Brand;
    public string? Description => ProductDetails.Description;
    public string? Size => ProductDetails.Size;
    public string? Pack => ProductDetails.QtyCase?.ToString();
    public decimal? Price => ProductDetails.Price;

    // ── per-line tax breakdown ─────────────────────────────────────
    public List<TaxAmount> LineTax { get; } = [];
    [ObservableProperty] private decimal _tax;

    public decimal? Extended =>
        ((Price ?? 0m) - (Discount ?? 0m) + (Deposit ?? 0m) + Tax) * Quantity;

    [ObservableProperty] private decimal? _deposit;
    [ObservableProperty] private decimal? _discount;
    [ObservableProperty] private decimal? _sale;   // null = blank cell


    public void RefreshTax(ITaxService svc, DateTime saleDate)
    {
        LineTax.Clear();
        LineTax.AddRange(svc.GetTaxBreakdown(ProductDetails, Quantity, saleDate));
        Tax = LineTax.Sum(t => t.Amount);
        OnPropertyChanged(nameof(Extended));
    }
}
