using AwesomeGIC.Bank.Domain;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Persistence;
using AwesomeGIC.Bank.Persistence.Repositories;
using AwesomeGIC.Bank.Presentation.Menus;
using AwesomeGIC.Bank.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AwesomeGIC.Bank.Startup
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);
            await ShowMenu(host);
            await host.RunAsync();
        }

        private static async Task ShowMenu(IHost host)
        {
            var mainMenu = host.Services.GetRequiredService<MainMenu>();
            await mainMenu.ProcessUserInputAsync();
        }

        private static IHost CreateHost(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            AddServices(builder.Services, builder.Configuration);
            AddMenuServices(builder.Services);

            return builder.Build();
        }

        private static void AddServices(IServiceCollection services, ConfigurationManager config)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite(
                    config.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<TransactionServiceProvider>();            
            services.AddScoped<InterestRuleServiceProvider>();
        }

        private static void AddMenuServices(IServiceCollection services)
        {
            services.AddScoped<MainMenu>();
            services.AddScoped<InputTransactionMenu>();
            services.AddScoped<DefineInterestRulesMenu>();
            services.AddScoped<PrintStatementMenu>();
        }
    }
}
