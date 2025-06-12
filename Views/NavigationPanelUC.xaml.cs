using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EConstruction.Views // Correct namespace
{
    /// <summary>
    /// Interaction logic for NavigationPanelUC.xaml
    /// </summary>
    public partial class NavigationPanelUC : UserControl
    {
        // Dependency Property for the navigation command, to be bound from MainViewModel
        public static readonly DependencyProperty NavigateCommandProperty =
            DependencyProperty.Register("NavigateCommand", typeof(ICommand), typeof(NavigationPanelUC), new PropertyMetadata(null));

        public ICommand NavigateCommand
        {
            get { return (ICommand)GetValue(NavigateCommandProperty); }
            set { SetValue(NavigateCommandProperty, value); }
        }

        // Dependency Property for the active module (to highlight the selected item)
        public static readonly DependencyProperty ActiveModuleProperty =
            DependencyProperty.Register("ActiveModule", typeof(string), typeof(NavigationPanelUC), new PropertyMetadata(string.Empty, OnActiveModuleChanged));

        public string ActiveModule
        {
            get { return (string)GetValue(ActiveModuleProperty); }
            set { SetValue(ActiveModuleProperty, value); }
        }

        private static void OnActiveModuleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationPanelUC control)
            {
                control.UpdateActiveItemHighlight((string)e.NewValue);
            }
        }

        public NavigationPanelUC()
        {
            InitializeComponent();
            this.Loaded += NavigationPanelUC_Loaded;
        }

        private void NavigationPanelUC_Loaded(object sender, RoutedEventArgs e)
        {
            // On load, ensure the initial active module is highlighted
            UpdateActiveItemHighlight(ActiveModule);
        }

        private void NavigationItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string moduleName)
            {
                // Update the ActiveModule Dependency Property
                ActiveModule = moduleName;

                // Execute the navigation command if it's bound
                if (NavigateCommand != null && NavigateCommand.CanExecute(moduleName))
                {
                    NavigateCommand.Execute(moduleName);
                }
            }
        }

        private void UpdateActiveItemHighlight(string activeModule)
        {
            // Iterate through the StackPanel's children (our navigation items)
            // Use x:Name="NavigationItemsStackPanel" in XAML for direct access
            if (NavigationItemsStackPanel != null)
            {
                foreach (UIElement item in NavigationItemsStackPanel.Children)
                {
                    if (item is Border border)
                    {
                        if (border.Tag is string itemModule && itemModule == activeModule)
                        {
                            // Apply the active style
                            border.Style = (Style)FindResource("ActiveNavigationItemStyle");
                        }
                        else
                        {
                            // Apply the default style
                            border.Style = (Style)FindResource("NavigationItemStyle");
                        }
                    }
                }
            }
        }
    }
}