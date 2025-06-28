// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Views\OrderPage.xaml.cs
using System;
using CommunityToolkit.Mvvm.Input;
using EConstruction.Modules.BusinessOperations.Models;
using EConstruction.Modules.BusinessOperations.Services;
using EConstruction.Modules.BusinessOperations.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace EConstruction.Modules.BusinessOperations.Views
{
    public sealed partial class OrderPage : Page
    {
        public OrderViewModel ViewModel { get; }

        public OrderPage()
        {
            this.InitializeComponent();
            ViewModel = App.AppHost.Services.GetRequiredService<OrderViewModel>();
            this.DataContext = ViewModel;

            Loaded += OrderPage_Loaded;
        }

        private async void OrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadVendorsAsync();
            if (ViewModel.Vendors.Any())
            {
                ViewModel.CurrentOrder.Vendor = ViewModel.Vendors.First();
            }
        }
    }
}