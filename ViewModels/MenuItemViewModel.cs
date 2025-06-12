using EConstruction.Commands;
using EConstruction.Models;
using EConstruction.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq; // Для використання Linq
using System.Windows.Input;

namespace EConstruction.ViewModels
{
    /// <summary>
    /// ViewModel для окремого пункту меню, що керує його станом та поведінкою.
    /// </summary>
    public class MenuItemViewModel : BaseViewModel
    {
        private readonly MenuItem _menuItem;
        private readonly INavigationService _navigationService;
        private bool _isActive;
        private bool _isExpanded;

        public string Header => _menuItem.Header;
        public string IconPathData => _menuItem.IconPathData;
        public string EmojiIcon => _menuItem.EmojiIcon;
        public Type? TargetViewType => _menuItem.TargetViewType;

        /// <summary>
        /// Колекція підменю, що може бути порожньою.
        /// </summary>
        public ObservableCollection<MenuItemViewModel> SubMenuItems { get; }

        /// <summary>
        /// Вказує, чи є пункт меню активним (вибраним).
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        /// <summary>
        /// Вказує, чи розгорнуто підменю даного пункту.
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        /// <summary>
        /// Вказує, чи має пункт підменю І чи меню НЕ згорнуте (для видимості іконки розгортання).
        /// </summary>
        public bool HasSubMenuItemsAndNotCollapsed
        {
            get
            {
                // Ця властивість буде оновлюватися, коли змінюється IsCollapsed батьківського ViewModel
                // або коли SubMenuItems змінюються.
                // Для коректного оновлення потрібен механізм оновлення залежностей.
                // Наразі припустимо, що вона викликається UI при прив'язці.
                return SubMenuItems.Any() && !(_navigationService as NavigationService)?.CurrentView?.DataContext is SlidebarMenuViewModel vm && !vm.IsCollapsed;
            }
        }


        public ICommand NavigateCommand { get; }
        public ICommand ToggleExpandCommand { get; } // Команда для розгортання/згортання підменю

        public MenuItemViewModel(MenuItem menuItem, INavigationService navigationService)
        {
            _menuItem = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            SubMenuItems = new ObservableCollection<MenuItemViewModel>();
            if (_menuItem.SubMenuItems != null)
            {
                foreach (var subItem in _menuItem.SubMenuItems)
                {
                    SubMenuItems.Add(new MenuItemViewModel(subItem, _navigationService));
                }
            }

            NavigateCommand = new RelayCommand(ExecuteNavigate, CanExecuteNavigate);
            ToggleExpandCommand = new RelayCommand(ExecuteToggleExpand, CanExecuteToggleExpand);

            // Підписуємося на подію зміни навігації, щоб оновлювати IsActive
            _navigationService.Navigated += OnNavigated;

            // Ініціалізація HasSubMenuItemsAndNotCollapsed
            _navigationService.Navigated += (type) => OnPropertyChanged(nameof(HasSubMenuItemsAndNotCollapsed));

        }

        // Відписка від події при звільненні об'єкта (якщо ViewModel очищається)
        ~MenuItemViewModel()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanExecuteNavigate(object? parameter)
        {
            // Команда навігації доступна, якщо є цільовий View або є підменю (для розгортання)
            return TargetViewType != null || SubMenuItems.Any();
        }

        private void ExecuteNavigate(object? parameter)
        {
            if (TargetViewType != null)
            {
                _navigationService.NavigateTo(TargetViewType);
            }
            // Якщо пункт має підменю, але немає власного TargetView,
            // ми просто розгортаємо/згортаємо його.
            if (SubMenuItems.Any())
            {
                IsExpanded = !IsExpanded;
            }
        }

        private bool CanExecuteToggleExpand(object? parameter)
        {
            return SubMenuItems.Any(); // Можна розгортати/згортати, якщо є підменю
        }

        private void ExecuteToggleExpand(object? parameter)
        {
            IsExpanded = !IsExpanded;
        }

        /// <summary>
        /// Обробник події навігації. Оновлює стан активності для цього пункту меню та його підменю.
        /// </summary>
        /// <param name="activeViewType">Тип View, який зараз активний.</param>
        private void OnNavigated(Type activeViewType)
        {
            IsActive = (TargetViewType == activeViewType);

            // Рекурсивно перевіряємо підменю
            foreach (var subItem in SubMenuItems)
            {
                subItem.UpdateActiveState(activeViewType);
                // Якщо будь-який підпункт активний, батьківський пункт також має бути розгорнутий
                if (subItem.IsActive)
                {
                    IsExpanded = true;
                }
            }
        }

        /// <summary>
        /// Допоміжний метод для оновлення стану активності (використовується рекурсивно).
        /// </summary>
        public void UpdateActiveState(Type activeViewType)
        {
            // Якщо це головний елемент, перевіряємо його TargetViewType
            IsActive = (TargetViewType == activeViewType);

            // Скидаємо розгорнутість за замовчуванням, якщо жоден з підпунктів не активний
            // Це дозволяє згорнути меню, якщо ми перейшли на інший основний пункт
            bool anySubItemActive = false;
            foreach (var subItem in SubMenuItems)
            {
                subItem.UpdateActiveState(activeViewType);
                if (subItem.IsActive)
                {
                    anySubItemActive = true;
                }
            }
            // Розгортаємо батьківський елемент, якщо активний його TargetViewType або активний будь-який підпункт
            IsExpanded = IsActive || anySubItemActive;

            // Оновлюємо властивість, яка залежить від IsCollapsed батьківського меню
            OnPropertyChanged(nameof(HasSubMenuItemsAndNotCollapsed));
        }
    }
}
