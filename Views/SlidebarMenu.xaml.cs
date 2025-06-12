using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EConstruction.ViewModels; // Для доступу до MenuItemViewModel

namespace EConstruction.Views
{
    /// <summary>
    /// Логіка взаємодії для SlidebarMenu.xaml
    /// </summary>
    public partial class SlidebarMenu : UserControl
    {
        public SlidebarMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробник події кліку на пункті меню (Border).
        /// Викликає команду навігації або перемикання розгортання підменю.
        /// </summary>
        /// <param name="sender">Джерело події.</param>
        /// <param name="e">Аргументи події.</param>
        private void MenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Перевіряємо, чи є відправник Border і чи його DataContext є MenuItemViewModel.
            if (sender is Border border && border.DataContext is MenuItemViewModel menuItemVM)
            {
                // Викликаємо команду навігації/розгортання, яка визначена у ViewModel.
                // ViewModel сам вирішить, чи навігувати, чи розгорнути підменю.
                if (menuItemVM.NavigateCommand.CanExecute(null))
                {
                    menuItemVM.NavigateCommand.Execute(null);
                }
                // Позначаємо подію як оброблену, щоб вона не поширювалася далі по дереву елементів.
                e.Handled = true;
            }
        }
    }
}
