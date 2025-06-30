using CMPS.ProjectManagement.Domain.ValueObjects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CMPS.Maui.Views;

public partial class NewProjectPage : ContentPage
{
    public NewProjectPage()
    {
        InitializeComponent();
        BindingContext = new NewProjectViewModel();
    }

    // ───────── Button handlers ─────────
    private void OnToggleOptional(object sender, EventArgs e)
    {
        // Якщо додасте StackLayout x:Name="OptionalSection" у XAML,
        // то тут легко керувати видимістю.
        if (this.FindByName<StackLayout>("OptionalSection") is StackLayout section)
        {
            section.IsVisible = !section.IsVisible;
            ((Button)sender).Text = section.IsVisible ? "Hide optional settings" : "Show optional settings";
        }
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        // Повернення на попередню сторінку
        await Shell.Current.GoToAsync("..");
    }

    private async void OnAddProject(object sender, EventArgs e)
    {
        if (BindingContext is NewProjectViewModel vm)
        {
            var result = await vm.TrySaveAsync();
            if (result)
                await Shell.Current.GoToAsync(".."); // back to dashboard
            else
                await DisplayAlert("Validation", "Please fill in the required fields.", "OK");
        }
    }
}

// ───────────────────────────────────────────────────────────────────────
//  Simple ViewModel (stub). Replace with your real MediatR/Service logic
// ───────────────────────────────────────────────────────────────────────
public partial class NewProjectViewModel : ObservableObject
{
    // ─── Constructor ───
    public NewProjectViewModel()
    {
        // Demo lists – у реальному додатку завантажуйте з сервісів/БД
        GroupOptions = new ObservableCollection<string>
        {
            "Full Projects", "In Development", "Shells", "Custom", "Remodel", "Design"
        };

        ScheduleTemplates = new ObservableCollection<string>
        {
            "None", "Remodel Template", "Starter Schedule Template"
        };
    }

    // ─── Bindable collections ───
    public ObservableCollection<string> GroupOptions { get; }
    public ObservableCollection<string> ScheduleTemplates { get; }

    // ─── Bound properties (simplified) ───
    [ObservableProperty]
    private string projectName = string.Empty;

    [ObservableProperty]
    private string address = string.Empty;

    // Додайте інші властивості за потреби (Phase, Finance, City, State …)

    // ─── Save logic (stub) ───
    public async Task<bool> TrySaveAsync()
    {
        // 1) Primitive validation
        if (string.IsNullOrWhiteSpace(ProjectName))
            return false;

        // 2) TODO: call MediatR command / API service
        await Task.Delay(500); // імітація збереження

        return true;
    }
}
