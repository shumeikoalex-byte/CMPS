using System;
using System.Windows.Controls;

namespace EConstruction.Services
{
    /// <summary>
    /// Реалізація сервісу навігації, що керує відображенням різних UserControl'ів
    /// у головній області контенту.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private UserControl? _currentView;

        /// <summary>
        /// Поточний активний View.
        /// </summary>
        public UserControl? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                Navigated?.Invoke(_currentView?.GetType() ?? typeof(object)); // Викликаємо подію з типом View
            }
        }

        public event Action<Type>? Navigated;

        /// <summary>
        /// Навігує до вказаного типу View шляхом створення нового екземпляра.
        /// </summary>
        /// <typeparam name="TView">Тип UserControl для навігації.</typeparam>
        public void NavigateTo<TView>() where TView : UserControl, new()
        {
            CurrentView = new TView();
        }

        /// <summary>
        /// Навігує до View за його типом шляхом створення нового екземпляра.
        /// </summary>
        /// <param name="viewType">Тип UserControl для навігації.</param>
        public void NavigateTo(Type viewType)
        {
            if (viewType != null && typeof(UserControl).IsAssignableFrom(viewType))
            {
                var view = Activator.CreateInstance(viewType) as UserControl;
                if (view != null)
                {
                    CurrentView = view;
                }
            }
        }
    }
}
