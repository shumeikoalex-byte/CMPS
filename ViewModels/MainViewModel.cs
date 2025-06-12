using EConstruction.Models;
using EConstruction.Services;
using System.Windows.Controls;

namespace EConstruction.ViewModels
{
    /// <summary>
    /// ViewModel для головного вікна додатку (MainWindow).
    /// Відповідає за загальну структуру UI, навігацію та керування бічним меню.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private UserControl? _currentView;

        public SlidebarMenuViewModel SlidebarMenuVM { get; }

        /// <summary>
        /// Поточний UserControl, що відображається у головній області контенту.
        /// </summary>
        public UserControl? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel()
        {
            _navigationService = new NavigationService(); // Створюємо екземпляр сервісу навігації

            // Підписуємося на подію Navigated NavigationService.
            // Коли NavigationService навігує до нового View, оновлюємо CurrentView.
            _navigationService.Navigated += (viewType) =>
            {
                CurrentView = _navigationService.CurrentView;
            };

            // Передача NavigationService до SlidebarMenuViewModel
            SlidebarMenuVM = new SlidebarMenuViewModel(_navigationService);

            // Встановлюємо початковий View (наприклад, Інформаційну панель)
            _navigationService.NavigateTo<Views.DashboardView>();
        }
    }
}
