using System.Windows.Input;

namespace EConstruction.Commands
{
    /// <summary>
    /// Реалізація ICommand, що дозволяє делегувати логіку виконання та перевірки доступності команди.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Виникає при зміні умов, що впливають на те, чи може бути виконана команда.
        /// CommandManager.RequerySuggested допомагає автоматично оновлювати стан кнопок.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Ініціалізує новий екземпляр RelayCommand.
        /// </summary>
        /// <param name="execute">Делегат для виконання команди.</param>
        /// <param name="canExecute">Делегат для перевірки, чи може бути виконана команда. Може бути null, якщо команда завжди доступна.</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Визначає, чи може бути виконана команда в поточному стані.
        /// </summary>
        /// <param name="parameter">Дані, що використовуються командою. Може бути null.</param>
        /// <returns>True, якщо команду можна виконати; інакше false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Виконує команду.
        /// </summary>
        /// <param name="parameter">Дані, що використовуються командою. Може бути null.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
