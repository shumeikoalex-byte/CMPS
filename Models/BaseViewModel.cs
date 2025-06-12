using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EConstruction.Models
{
    /// <summary>
    /// Базовий клас для всіх ViewModel, що реалізує INotifyPropertyChanged.
    /// Це дозволяє повідомляти UI про зміни властивостей.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Викликає подію PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Ім'я властивості, що змінилася. Автоматично визначається компілятором.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Допоміжний метод для встановлення значення властивості та виклику OnPropertyChanged,
        /// тільки якщо значення дійсно змінилося.
        /// </summary>
        /// <typeparam name="T">Тип властивості.</typeparam>
        /// <param name="storage">Посилання на поле, що зберігає значення властивості.</param>
        /// <param name="value">Нове значення властивості.</param>
        /// <param name="propertyName">Ім'я властивості. Автоматично визначається компілятором.</param>
        /// <returns>True, якщо значення змінилося; інакше false.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
