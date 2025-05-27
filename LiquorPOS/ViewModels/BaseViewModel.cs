using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LiquorPOS.ViewModels
{
    /// <summary>
    /// A base class for ViewModels that inherits from ObservableObject,
    /// providing INotifyPropertyChanged implementation through the MVVM Toolkit.
    /// Other ViewModels will inherit from this BaseViewModel.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        // By inheriting from ObservableObject, you automatically get:
        // 1. Implementation of INotifyPropertyChanged.
        // 2. Methods like OnPropertyChanged(), SetProperty(), etc.
        // 3. The ability to use [ObservableProperty] and [RelayCommand]
        //    attributes in your derived classes (like MainViewModel).

        // You can add any properties or methods here that you want
        // *all* of your ViewModels to share. For now, it can be empty
        // because ObservableObject provides the core functionality.

        // Example of a common property you might add later:
        // [ObservableProperty]
        // private bool _isBusy;
    }
}