using CMPS.Maui.Views;
using Microsoft.Maui.Controls;

namespace CMPS.Maui;

public static class ShellRoutes
{
    public static void RegisterAll()
    {
        Routing.RegisterRoute("MainPage", typeof(MainPage)); // Вже існує
        Routing.RegisterRoute("NewProjectPage", typeof(NewProjectPage)); // Вже існує

        // Реєстрація маршрутів для всіх нових сторінок-заглушок
        Routing.RegisterRoute("TimeClockPage", typeof(TimeClockPage));
        Routing.RegisterRoute("ContactsPage", typeof(ContactsPage));
        Routing.RegisterRoute("LeadsPage", typeof(LeadsPage));
        Routing.RegisterRoute("TemplatesPage", typeof(TemplatesPage));
        Routing.RegisterRoute("ReportsPage", typeof(ReportsPage));
        Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
        Routing.RegisterRoute("CommunityPage", typeof(CommunityPage));
        Routing.RegisterRoute("AddTodoPage", typeof(AddTodoPage));
        Routing.RegisterRoute("SummaryPage", typeof(SummaryPage));
        Routing.RegisterRoute("EstimatePage", typeof(EstimatePage));
        Routing.RegisterRoute("ProposalsPage", typeof(ProposalsPage));
        Routing.RegisterRoute("BidsPage", typeof(BidsPage));
        Routing.RegisterRoute("PurchaseOrdersPage", typeof(PurchaseOrdersPage));
        Routing.RegisterRoute("ChangeOrdersPage", typeof(ChangeOrdersPage));
        Routing.RegisterRoute("BudgetPage", typeof(BudgetPage));
        Routing.RegisterRoute("InvoicesPage", typeof(InvoicesPage));
        Routing.RegisterRoute("SchedulePage", typeof(SchedulePage));
        Routing.RegisterRoute("HoursPage", typeof(HoursPage));
        Routing.RegisterRoute("SpecsSelectionsPage", typeof(SpecsSelectionsPage));
        Routing.RegisterRoute("MessagesPage", typeof(MessagesPage));
        Routing.RegisterRoute("TodosPage", typeof(TodosPage));
        Routing.RegisterRoute("JobLogPage", typeof(JobLogPage));
        Routing.RegisterRoute("FilesPage", typeof(FilesPage));
        Routing.RegisterRoute("PhotosPage", typeof(PhotosPage));
        Routing.RegisterRoute("WarrantyRequestsPage", typeof(WarrantyRequestsPage));
        Routing.RegisterRoute("ProjectSetupPage", typeof(ProjectSetupPage));
        Routing.RegisterRoute("RecentPhotosPage", typeof(RecentPhotosPage));

        // Якщо у вас є TaskManagerPage (яка була в першому прикладі заглушки)
        // Routing.RegisterRoute("TaskManagerPage", typeof(TaskManagerPage));
    }
}