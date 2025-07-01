using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using LiquorPOS.Models;                 // Product, LiquorDbContext

namespace LiquorPOS
{
    public partial class ProductListWindow : Window
    {
        private readonly IDbContextFactory<LiquorDbContext> _dbFactory;
        private ObservableCollection<Product>? _allProducts;
        private ICollectionView? _collectionView;

        // DI supplies the factory
        public ProductListWindow(IDbContextFactory<LiquorDbContext> dbFactory)
        {
            InitializeComponent();
            _dbFactory = dbFactory;

            Loaded += ProductListWindow_Loaded;
        }

        private async void ProductListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await using var context = await _dbFactory.CreateDbContextAsync();
                var products = await context.Products.AsNoTracking().ToListAsync();

                _allProducts = new ObservableCollection<Product>(products);
                _collectionView = CollectionViewSource.GetDefaultView(_allProducts);
                _collectionView.Filter = FilterProducts;
                ProductsDataGrid.ItemsSource = _collectionView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error Loading Products");
            }
        }

        private bool FilterProducts(object obj)
        {
            if (obj is not Product p) return false;

            string q = SearchTextBox.Text?.Trim() ?? string.Empty;
            if (q.Length == 0) return true;

            return (p.CodeNum?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                || (p.Brand?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                || (p.Description?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void SearchTextBox_TextChanged(object sender,
                                               System.Windows.Controls.TextChangedEventArgs e)
        {
            _collectionView?.Refresh();
        }
    }
}
