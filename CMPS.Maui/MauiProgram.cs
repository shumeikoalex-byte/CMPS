using Microsoft.Extensions.Logging;
using CMPS.Maui.Views; // Додайте using для ваших Views
using CMPS.Maui.ViewModels; // Додайте using для ваших ViewModels

namespace CMPS.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Реєстрація Views
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<NewProjectPage>(); // Transient, якщо це нова сторінка, яка може відкриватися кілька разів

        // Реєстрація всіх сторінок-заглушок як Transient або Singleton за потребою
        builder.Services.AddTransient<TimeClockPage>();
        builder.Services.AddTransient<ContactsPage>();
        builder.Services.AddTransient<LeadsPage>();
        builder.Services.AddTransient<TemplatesPage>();
        builder.Services.AddTransient<ReportsPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CommunityPage>();
        builder.Services.AddTransient<AddTodoPage>();
        builder.Services.AddTransient<SummaryPage>();
        builder.Services.AddTransient<EstimatePage>();
        builder.Services.AddTransient<ProposalsPage>();
        builder.Services.AddTransient<BidsPage>();
        builder.Services.AddTransient<PurchaseOrdersPage>();
        builder.Services.AddTransient<ChangeOrdersPage>();
        builder.Services.AddTransient<BudgetPage>();
        builder.Services.AddTransient<InvoicesPage>();
        builder.Services.AddTransient<SchedulePage>();
        builder.Services.AddTransient<HoursPage>();
        builder.Services.AddTransient<SpecsSelectionsPage>();
        builder.Services.AddTransient<MessagesPage>();
        builder.Services.AddTransient<TodosPage>();
        builder.Services.AddTransient<JobLogPage>();
        builder.Services.AddTransient<FilesPage>();
        builder.Services.AddTransient<PhotosPage>();
        builder.Services.AddTransient<WarrantyRequestsPage>();
        builder.Services.AddTransient<ProjectSetupPage>();
        builder.Services.AddTransient<RecentPhotosPage>();
        // builder.Services.AddTransient<TaskManagerPage>(); // Якщо є

        // Реєстрація ViewModels
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<NewProjectViewModel>(); // Transient, якщо це ViewModel для сторінки, яка відкривається багаторазово

        return builder.Build();
    }
}