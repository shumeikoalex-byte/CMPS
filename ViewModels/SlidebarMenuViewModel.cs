using EConstruction.Commands;
using EConstruction.Models;
using EConstruction.Services;
using EConstruction.Views; // Для типів View
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EConstruction.ViewModels
{
    /// <summary>
    /// ViewModel для бічної навігаційної панелі (SlidebarMenu).
    /// Керує станом меню (згорнуто/розгорнуто) та колекцією пунктів меню.
    /// </summary>
    public class SlidebarMenuViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private bool _isCollapsed;

        /// <summary>
        /// Колекція пунктів основного меню.
        /// </summary>
        public ObservableCollection<MenuItemViewModel> MenuItems { get; }

        /// <summary>
        /// Вказує, чи згорнуте меню (true) або розгорнуте (false).
        /// </summary>
        public bool IsCollapsed
        {
            get => _isCollapsed;
            set
            {
                if (SetProperty(ref _isCollapsed, value))
                {
                    // Оновлюємо HasSubMenuItemsAndNotCollapsed для всіх пунктів меню
                    // коли змінюється стан згортання батьківського меню.
                    foreach (var menuItem in MenuItems)
                    {
                        menuItem.OnPropertyChanged(nameof(MenuItemViewModel.HasSubMenuItemsAndNotCollapsed));
                        foreach (var subItem in menuItem.SubMenuItems)
                        {
                            subItem.OnPropertyChanged(nameof(MenuItemViewModel.HasSubMenuItemsAndNotCollapsed));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Команда для перемикання стану згортання/розгортання меню.
        /// </summary>
        public ICommand ToggleMenuCommand { get; }

        public SlidebarMenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _navigationService.Navigated += OnNavigated; // Підписуємося на подію навігації

            MenuItems = new ObservableCollection<MenuItemViewModel>();
            LoadMenuItems(); // Завантажуємо пункти меню

            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
        }

        /// <summary>
        /// Заповнює колекцію пунктів меню. Тут можна використовувати дані з файлу, БД тощо.
        /// Наразі це хардкоджені дані.
        /// </summary>
        private void LoadMenuItems()
        {
            // Використовуємо Path Data для іконок (для прикладу, можна знайти SVG to Path Data конвертери)
            // Або просто Emoji-іконки, як альтернатива.
            // Приклад Path Data для іконок: https://fonts.google.com/icons (виберіть іконку, потім "Icon font" -> "SVG" -> "Path data")

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Інформаційна панель",
                IconPathData = "M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z", // Material Icons: Dashboard
                TargetViewType = typeof(DashboardView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Ліди",
                IconPathData = "M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-4 0h-.09c-1.63 0-3.15.52-4.42 1.39-2.31 1.5-3.52 3.84-3.52 6.07V22h16v-2c0-2.66-5.33-4-8-4z", // Material Icons: Person Add
                TargetViewType = typeof(LeadsView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Оцінки",
                IconPathData = "M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zM6 20V4h7v5h5v11H6z", // Material Icons: Description (File Text)
                TargetViewType = typeof(EstimatesView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Завдання",
                EmojiIcon = "📝", // Emoji icon
                TargetViewType = typeof(TasksView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Запити пропозицій",
                EmojiIcon = "🛒", // Emoji icon
                TargetViewType = typeof(RFQsView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Клієнти / Контакти",
                EmojiIcon = "👥", // Emoji icon
                TargetViewType = typeof(ClientsView)
            }, _navigationService));

            // Приклад пункту з підменю
            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Комунікації",
                EmojiIcon = "💬", // Emoji icon
                SubMenuItems = new System.Collections.Generic.List<MenuItem>
                {
                    new MenuItem { Header = "Чат", EmojiIcon = "📨", TargetViewType = typeof(CommunicationsView) },
                    new MenuItem { Header = "Журнал дзвінків", EmojiIcon = "📞", TargetViewType = typeof(AnotherCommunicationView) }
                }
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Каталоги",
                EmojiIcon = "📚", // Emoji icon
                TargetViewType = typeof(CatalogsView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Інтеграції",
                EmojiIcon = "🔗", // Emoji icon
                TargetViewType = typeof(IntegrationsView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Налаштування",
                EmojiIcon = "⚙️", // Emoji icon
                TargetViewType = typeof(SettingsView)
            }, _navigationService));

            MenuItems.Add(new MenuItemViewModel(new MenuItem
            {
                Header = "Перегляд функцій",
                EmojiIcon = "🧪", // Emoji icon
                TargetViewType = typeof(PreviewView)
            }, _navigationService));
        }

        /// <summary>
        /// Обробник події навігації. Оновлює активний пункт меню.
        /// </summary>
        /// <param name="navigatedViewType">Тип View, до якого щойно відбулася навігація.</param>
        private void OnNavigated(Type navigatedViewType)
        {
            // Знімаємо активність з усіх пунктів меню
            foreach (var menuItem in MenuItems)
            {
                menuItem.UpdateActiveState(navigatedViewType); // Рекурсивне оновлення стану
            }
        }

        private void ExecuteToggleMenu(object? parameter)
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}
