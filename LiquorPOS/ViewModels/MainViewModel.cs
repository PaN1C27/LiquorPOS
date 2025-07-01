using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiquorPOS.Models;
using LiquorPOS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;                  // ← add this
using LiquorPOS;                    // ← only if ProductListWindow isn’t found
/* existing using lines stay */


namespace LiquorPOS.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ITaxService _taxService;
    private readonly IDbContextFactory<LiquorDbContext> _dbFactory;
    private readonly IServiceProvider _services;

    public MainViewModel(ITaxService taxService,
                         IDbContextFactory<LiquorDbContext> dbFactory, IServiceProvider services)
    {
        _taxService = taxService;
        _dbFactory = dbFactory;
        _services = services;

        ScannedItemsList.CollectionChanged += (_, __) => RecalculateTotals();
    }

    // 🔵 1) currently-selected grid row
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditQuantityCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteItemCommand))]
    private ScannedItemViewModel? _selectedItem;

    // 🔵 2) Change-Qty command  (F2, button, context-menu)
    [RelayCommand(CanExecute = nameof(CanModifyLine))]
    private void EditQuantity()
    {
        if (SelectedItem is null) return;

        // open the dialog
        var dlg = new QuantityInputWindow(SelectedItem.Quantity)
        { Owner = Application.Current.MainWindow };

        if (dlg.ShowDialog() == true)
        {
            SelectedItem.Quantity = dlg.EnteredQuantity;
            SelectedItem.RefreshTax(_taxService, DateTime.Today);
            RecalculateTotals();
        }
    }

    // 🔵 3) Delete-line command   (Delete key)
    [RelayCommand(CanExecute = nameof(CanModifyLine))]
    private void DeleteItem()
    {
        if (SelectedItem is null) return;
        ScannedItemsList.Remove(SelectedItem);
        RenumberLines();
        RecalculateTotals();
    }

    // 🔵 4) Void-sale command    (button)
    [RelayCommand]
    private void VoidSale()
    {
        ScannedItemsList.Clear();
        RecalculateTotals();
    }

    // helper so CanExecute updates with selection
    private bool CanModifyLine() => SelectedItem != null;

    private void RenumberLines()
    {
        for (int i = 0; i < ScannedItemsList.Count; i++)
            ScannedItemsList[i].LineNumber = i + 1;
    }

    // ── collections & simple props ─────────────────────────────────
    public ObservableCollection<ScannedItemViewModel> ScannedItemsList { get; } = [];

    [ObservableProperty] private string _barcodeText = string.Empty;
    [ObservableProperty] private string _foundItemName = string.Empty;
    [ObservableProperty] private string _foundItemPrice = string.Empty;

    // Ticket-level totals
    [ObservableProperty] private decimal _subTotal;
    [ObservableProperty] private decimal _generalTaxTotal;
    [ObservableProperty] private decimal _alcoholTaxTotal;
    [ObservableProperty] private decimal _grandTotal;

    // ── barcode scan command (async) ───────────────────────────────
    [RelayCommand]
    private async Task ScanBarcodeAsync()
    {
        var code = BarcodeText.Trim();
        if (code.Length == 0) return;

        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var hit = await db.Barcodes
                              .Include(b => b.Product)
                              .FirstOrDefaultAsync(b => b.BarcodeValue == code);

            if (hit?.Product == null)
            {
                FoundItemName = "Item Not Found";
                FoundItemPrice = "---";
                return;
            }

            var line = new ScannedItemViewModel(hit.Product, 1,
                                 ScannedItemsList.Count + 1);

            line.RefreshTax(_taxService, DateTime.Today);
            ScannedItemsList.Add(line);

            FoundItemName = $"{line.Brand} {line.Description}".Trim();
            FoundItemPrice = $"{line.Price:C}";
            BarcodeText = string.Empty;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Database error");
        }
    }

    [RelayCommand]
    private void ShowProducts()
    {
        var win = _services.GetRequiredService<ProductListWindow>();
        win.Owner = Application.Current.MainWindow;
        win.ShowDialog();
    }

    // ── helper ────────────────────────────────────────────────────
    private void RecalculateTotals()
    {
        SubTotal = ScannedItemsList.Sum(l => (l.Price ?? 0m) * l.Quantity);
        GeneralTaxTotal = ScannedItemsList.SelectMany(l => l.LineTax)
                                          .Where(t => t.ComponentName == "General")
                                          .Sum(t => t.Amount);
        AlcoholTaxTotal = ScannedItemsList.SelectMany(l => l.LineTax)
                                          .Where(t => t.ComponentName == "Alcohol")
                                          .Sum(t => t.Amount);
        GrandTotal = SubTotal + GeneralTaxTotal + AlcoholTaxTotal;
    }
}
