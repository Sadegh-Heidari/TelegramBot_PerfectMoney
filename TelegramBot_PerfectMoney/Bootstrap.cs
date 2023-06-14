using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramBot_PerfectMoney.DataBase;
using TelegramBot_PerfectMoney.OperationBot;
using TelegramBot_PerfectMoney.TelegramPresentation;

namespace TelegramBot_PerfectMoney
{
    public static class Bootstrap
    {
        private static string connectionString = "Server=localhost; User ID=root; Password=126543210mM$; Database=TelBot";

        public static   void AddTelegramConfig(this IHostBuilder host)
        {
            host.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<TelegramBot>();
                services.AddScoped<IOperationTelegramBot, OperationTelegramBot>();
                services.AddDbContext<TelContext>(x=>x.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
            });
        }
    }
}
