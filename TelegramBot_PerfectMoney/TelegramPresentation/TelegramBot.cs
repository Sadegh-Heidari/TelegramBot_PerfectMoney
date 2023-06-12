using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_PerfectMoney.OperationBot;

namespace TelegramBot_PerfectMoney.TelegramPresentation
{
    public class TelegramBot
    {
        private long chatid { get; set; }
        private int massageId { get; set; }
        private IOperationTelegramBot _operation { get; set; }
        private CancellationTokenSource cts { get; }
        public TelegramBot(IOperationTelegramBot operation)
        {
            _operation = operation;
            cts = new();
        }

        public async Task Run(string TokenBot)
        {


            var botClient = new TelegramBotClient(TokenBot);

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            var me = await botClient.GetMeAsync();
            Console.Write("Start listening for");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" @{me.Username}");
            Console.ResetColor();




            // Send cancellation request to stop bot
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
           
            if (update?.Message is not null)
            {
                chatid = update.Message.Chat.Id;
                massageId = update.Message.MessageId;
            }

            if (update is not null || update?.Message is  not null)
            {
                if (update.Message!.Text!.Contains("/start".ToLower()))
                {
                    // جهت چک کردن اینکه شماره تلفن کاربر ثبت شده یا نه
                    // if (Identity)
                    // {
                    //      
                    // }
                    await _operation.Start(botClient, chatid, cancellationToken);

                }
                else if(update.Message.Text.Contains("مدیریت"))
                {
                    await _operation.AdminSection(botClient, chatid, cancellationToken);
                }
                else if(update.Message.Text.Contains("بازگشت به صفحه اصلی"))
                {
                    await _operation.Start(botClient, chatid, cancellationToken);
                }
            }



        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // var ErrorMessage = exception switch
            // {
            //     ApiRequestException apiRequestException
            //         => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            //     _ => exception.InnerException.InnerException.Message.ToString()
            // };
            //
            // Console.WriteLine(ErrorMessage);
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.Write("Error");
            // var ErrorMessage = exception switch
            // {
            //     ApiRequestException apiRequestException
            //         => $" : [{apiRequestException.ErrorCode}]  {apiRequestException.Message}",
            //     _ => " : "+ exception.InnerException.InnerException.Message.ToString()
            // };
            // Console.ResetColor();
            // Console.WriteLine(ErrorMessage);
            // Console.WriteLine("\n\n Press any key for close program......");
            // Console.ReadKey();
            return Task.CompletedTask;
        }

        public void Stop()
        {
            cts.Cancel();
        }
    }
}
