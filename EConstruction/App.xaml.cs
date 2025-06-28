// C:\Projects\EConstruction\EConstruction\App.xaml.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using EConstruction.Modules.BusinessOperations.Models;
using EConstruction.Modules.BusinessOperations.Services;
using EConstruction.Modules.BusinessOperations.ViewModels;
using EConstruction.Modules.BusinessOperations.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EConstruction
{
    public partial class App : Application
    {
        public static IHost AppHost { get; } = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, cfg) =>
            {
                cfg.SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(context.Configuration
                                         .GetConnectionString("DefaultConnection")));

                // services.AddScoped<IDataService, DataService>(); // ЦЕЙ РЯДОК МАЄ БУТИ ВИДАЛЕНИЙ АБО ЗАКОМЕНТОВАНИЙ
                services.AddScoped<IOrderService, OrderService>();
                services.AddTransient<OrderViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        public App()
        {
            this.InitializeComponent();
            AppHost.Start();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args) // ВИДАЛЕНО async
        {
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Activate();
        }
    }
}