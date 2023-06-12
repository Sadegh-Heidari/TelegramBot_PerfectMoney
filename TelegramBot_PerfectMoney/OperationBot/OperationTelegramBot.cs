using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_PerfectMoney.OperationBot
{
    public class OperationTelegramBot:IOperationTelegramBot
    {
       
        public  async Task Start(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {

           var mainKeyboardMarkup= CreatKeyboard.SetMainKeyboardMarkup();
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose a response",
                replyMarkup: mainKeyboardMarkup,
                cancellationToken: cancellationToken);

        }

        public async Task AdminSection(ITelegramBotClient botClient, long ChatId, CancellationToken cancellationToken)
        {
            var Adminkeyboard = CreatKeyboard.SetAdminMainKeyboard();
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "Choose a response",
                replyMarkup: Adminkeyboard,
                cancellationToken: cancellationToken);
        }

        public async Task GetContact(ITelegramBotClient botClient, long ChatId, CancellationToken cancellationToken)
        {
            var ShareContactKeyboard = CreatKeyboard.GetContactKeyboard();
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "لطفا شماره تلفن خودروا ارسال کنید",
                replyMarkup: ShareContactKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
