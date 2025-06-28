// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls; // <-- �������: ��� NavigationView, Frame
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
using EConstruction.Modules.BusinessOperations.Views; // <-- �������: ��� OrderPage

namespace EConstruction
{
    public sealed partial class MainWindow : Window // <-- �������������, �� �� partial class
    {
        public MainWindow()
        {
            this.InitializeComponent(); // ��� ����� ���������� XAML-����������� � �� ���� ���������,
                                        // ���� XAML-���� ���������.

            // �������: �������������, �� XAML �������� � x:Name ������� � �������� � MainWindow.xaml
            // AppNavigationView �� ContentFrame ����������� ����������� �� ���� ����� partial �����.
            // ���� ���� ��� �� �� "��������", �� �������� ����/��������� XAML.

            // �������� �� ������� "����������" ��� �������
            ContentFrame.Navigate(typeof(OrderPage)); // ��������� �� OrderPage ����� ����� ������� using EConstruction.Modules.BusinessOperations.Views;
        }

        // �������� ������ �������� ���� NavigationView
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
                        Console.WriteLine("������� �� ������� ������������� (�� ����������).");
                        break;
                    case "ReportsPage":
                        Console.WriteLine("������� �� ������� ���� (�� ����������).");
                        break;
                }
            }
        }
    }
}
