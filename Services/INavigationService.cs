using System;
using System.Windows.Controls;

namespace EConstruction.Services
{
    /// <summary>
    /// Інтерфейс для сервісу навігації між UserControl'ами.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Подія, що виникає після навігації до нового View.
        /// Повідомляє про тип нового View.
        /// </summary>
        event Action<Type>? Navigated;

        /// <summary>
        /// Поточний активний View.
        /// </summary>
        UserControl? CurrentView { get; }

        /// <summary>
        /// Навігує до вказаного типу View.
        /// </summary>
        /// <typeparam name="TView">Тип UserControl для навігації.</typeparam>
        void NavigateTo<TView>() where TView : UserControl, new();

        /// <summary>
        /// Навігує до View за його типом.
        /// </summary>
        /// <param name="viewType">Тип UserControl для навігації.</param>
        void NavigateTo(Type viewType);
    }
}
