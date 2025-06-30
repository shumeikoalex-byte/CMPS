namespace CMPS.SharedKernel.Helpers;

/// <summary>
/// Статичний клас-помічник для перевірки аргументів методів.
/// Спрощує перевірку на null, порожній рядок тощо, викидаючи відповідні винятки.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Перевіряє, чи є об'єкт null.
    /// </summary>
    /// <param name="argument">Об'єкт для перевірки.</param>
    /// <param name="argumentName">Назва аргументу.</param>
    /// <exception cref="ArgumentNullException">Викидається, якщо аргумент null.</exception>
    public static void AgainstNull(object? argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

    /// <summary>
    /// Перевіряє, чи є рядок null або порожнім.
    /// </summary>
    /// <param name="argument">Рядок для перевірки.</param>
    /// <param name="argumentName">Назва аргументу.</param>
    /// <exception cref="ArgumentNullException">Викидається, якщо рядок null.</exception>
    /// <exception cref="ArgumentException">Викидається, якщо рядок порожній.</exception>
    public static void AgainstNullOrEmpty(string? argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }

        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException($"Argument '{argumentName}' cannot be empty or whitespace.", argumentName);
        }
    }

    /// <summary>
    /// Перевіряє, чи є колекція null або порожньою.
    /// </summary>
    /// <typeparam name="T">Тип елементів колекції.</typeparam>
    /// <param name="argument">Колекція для перевірки.</param>
    /// <param name="argumentName">Назва аргументу.</param>
    /// <exception cref="ArgumentNullException">Викидається, якщо колекція null.</exception>
    /// <exception cref="ArgumentException">Викидається, якщо колекція порожня.</exception>
    public static void AgainstNullOrEmpty<T>(IEnumerable<T>? argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }

        if (!argument.Any())
        {
            throw new ArgumentException($"Collection '{argumentName}' cannot be empty.", argumentName);
        }
    }

    /// <summary>
    /// Перевіряє умову і викидає виняток, якщо вона хибна.
    /// </summary>
    /// <param name="condition">Умова, яку потрібно перевірити.</param>
    /// <param name="message">Повідомлення винятку.</param>
    /// <exception cref="ArgumentException">Викидається, якщо умова хибна.</exception>
    public static void AgainstFalse(bool condition, string message)
    {
        if (!condition)
        {
            throw new ArgumentException(message);
        }
    }
}