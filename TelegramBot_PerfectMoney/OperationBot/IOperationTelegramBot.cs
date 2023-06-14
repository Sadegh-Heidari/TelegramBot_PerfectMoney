using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_PerfectMoney.OperationBot
{
    public interface IOperationTelegramBot
    {
        Task Start(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task AdminMainSection(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task GetContact(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task AdminUserListSection(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task BackToMainSection(ITelegramBotClient  botClient, Update update, CancellationToken cancellationToken);
        Task BackToPreviousnStep(ITelegramBotClient  botClient, Update update, CancellationToken cancellationToken);
        Task GetUserList(ITelegramBotClient botClient, Update update , CancellationToken cancellationToken, string page);
        Task SendNumberRequest(ITelegramBotClient botClient,Update  update, CancellationToken cancellationToken);
        Task SearchUserByPhoneNumber(ITelegramBotClient botClient,Update  update, CancellationToken cancellationToken);
    }
}
