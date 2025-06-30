namespace CMPS.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Переконайтеся, що цей рядок викликається
        ShellRoutes.RegisterAll();
    }
}