// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls; // <-- Важливо: для NavigationView, Frame
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.Extensions.DependencyInjection;
using EConstruction.Modules.BusinessOperations.Models;
using EConstruction.Modules.BusinessOperations.Services;
using System.Threading.Tasks;
using EConstruction.Modules.BusinessOperations.ViewModels;
using EConstruction.Modules.BusinessOperations.Views; // <-- Важливо: для OrderPage

namespace EConstruction
{
    public sealed partial class MainWindow : Window // <-- Переконайтеся, що це partial class
    {
        public MainWindow()
        {
            this.InitializeComponent(); // Цей метод генерується XAML-компілятором і він буде доступний,
                                        // якщо XAML-файл коректний.

            // Важливо: переконайтеся, що XAML елементи з x:Name існують і оголошені в MainWindow.xaml
            // AppNavigationView та ContentFrame генеруються автоматично як поля цього partial класу.
            // Якщо вони все ще не "бачаться", це проблема кешу/генерації XAML.

            // Навігація на сторінку "Замовлення" при запуску
            ContentFrame.Navigate(typeof(OrderPage)); // Посилання на OrderPage тепер пряме завдяки using EConstruction.Modules.BusinessOperations.Views;
        }

        // Обробник вибору елементу меню NavigationView
        private void AppNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItemContainer != null)
            {
                var selectedItemTag = args.SelectedItemContainer.Tag?.ToString();
                switch (selectedItemTag)
                {
                    case "OrderPage":
                        ContentFrame.Navigate(typeof(OrderPage));
                        break;
                    case "VendorPage":
                        Console.WriteLine("Перехід до сторінки постачальників (не реалізовано).");
                        break;
                    case "ReportsPage":
                        Console.WriteLine("Перехід до сторінки звітів (не реалізовано).");
                        break;
                }
            }
        }
    }
}
