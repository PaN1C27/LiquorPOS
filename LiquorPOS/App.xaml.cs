using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LiquorPOS.Services;
using LiquorPOS.ViewModels;
using LiquorPOS.Models;

namespace LiquorPOS
{
    public partial class App : Application
    {
        public static IHost HostApp { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            HostApp = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    //--------------------------------------------------
                    //  DATABASE ACCESS
                    //--------------------------------------------------
                    const string conn =
                        "Host=100.71.49.34;Database=liquor_store_pos;Username=postgres;Password=postgres";

                    // Short-lived contexts for async work
                    services.AddDbContextFactory<LiquorDbContext>(
                        opts => opts.UseNpgsql(conn));

                    // Also a scope-long context for synchronous services
                    services.AddDbContext<LiquorDbContext>(
                        opts => opts.UseNpgsql(conn));

                    //--------------------------------------------------
                    //  DOMAIN SERVICES
                    //--------------------------------------------------
                    services.AddScoped<ITaxService, MdTaxService>();

                    //--------------------------------------------------
                    //  VIEW-MODELS & WINDOWS
                    //--------------------------------------------------
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<ProductListWindow>();   // ← NEW
                })
                .Build();

            var shell = HostApp.Services.GetRequiredService<MainWindow>();
            shell.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            HostApp.Dispose();
            base.OnExit(e);
        }
    }
}
