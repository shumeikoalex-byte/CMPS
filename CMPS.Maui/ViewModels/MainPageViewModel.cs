using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls; // Для Shell.Current

namespace CMPS.Maui.ViewModels;

// Припустимо, у вас є модель даних для Project
public class Project
{
    public string Name { get; set; }
    public string Route { get; set; } // Можливо, унікальний ідентифікатор або маршрут
    // Інші властивості проекту
}

public partial class MainPageViewModel : ObservableObject
{
    // Колекція для відображення проектів у навігації
    public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();

    [ObservableProperty]
    private Project _selectedProject;

    public MainPageViewModel()
    {
        // Заглушка для проектів. У реальному додатку вони завантажуватимуться
        Projects.Add(new Project { Name = "Project Alpha", Route = "ProjectAlphaOverview" });
        Projects.Add(new Project { Name = "Project Beta", Route = "ProjectBetaOverview" });
        Projects.Add(new Project { Name = "Project Gamma", Route = "ProjectGammaOverview" });

        // Приклад обробника для "New Project" (якщо використовуєте TapGestureRecognizer)
        // Альтернативно, можна використовувати RelayCommand на кнопці або Label,
        // якщо додати TapGestureRecognizer в XAML, як показано вище.
        // public static readonly TapGestureRecognizer NewProjectTapped = new TapGestureRecognizer { Command = new Command(async () => await Shell.Current.GoToAsync("NewProjectPage")) };
    }

    [RelayCommand]
    private async Task AddTodo()
    {
        await Shell.Current.GoToAsync("AddTodoPage");
    }

    [RelayCommand]
    private async Task Navigate(string route)
    {
        if (!string.IsNullOrWhiteSpace(route))
        {
            await Shell.Current.GoToAsync(route);
        }
    }

    partial void OnSelectedProjectChanged(Project value)
    {
        if (value != null)
        {
            // Логіка, коли вибрано інший проект
            // Наприклад, завантажити дані для SelectedProject на MainPage
            // або перейти на сторінку огляду конкретного проекту
            // Shell.Current.GoToAsync($"ProjectDetailsPage?id={value.Id}");
        }
    }
}