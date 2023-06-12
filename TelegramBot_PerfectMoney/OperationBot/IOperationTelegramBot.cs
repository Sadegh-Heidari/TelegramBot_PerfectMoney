using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot_PerfectMoney.OperationBot
{
    public interface IOperationTelegramBot
    {
        Task Start(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken);
        Task AdminSection(ITelegramBotClient botClient, long ChatId, CancellationToken cancellationToken);
        Task GetContact(ITelegramBotClient botClient, long ChatId, CancellationToken cancellationToken);
    }
}
