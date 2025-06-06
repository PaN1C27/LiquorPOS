using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using LiquorPOS.Models;

namespace LiquorPOS
{
    public partial class ProductListWindow : Window
    {
        private ObservableCollection<Product>? _allProducts;
        private ICollectionView? _collectionView;

        public ProductListWindow()
        {
            InitializeComponent();
            Loaded += ProductListWindow_Loaded;
        }

        private async void ProductListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await using var context = new LiquorDbContext();
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
            if (obj is not Product product)
                return false;

            string query = SearchTextBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(query))
                return true;

            return (product.CodeNum?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
                   || (product.Brand?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
                   || (product.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _collectionView?.Refresh();
        }
    }
}