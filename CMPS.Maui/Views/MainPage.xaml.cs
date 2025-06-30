namespace CMPS.Maui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(ViewModels.MainPageViewModel viewModel) // Впроваджуємо ViewModel через конструктор
    {
        InitializeComponent();
        this.BindingContext = viewModel; // Прив'язуємо ViewModel
    }
}