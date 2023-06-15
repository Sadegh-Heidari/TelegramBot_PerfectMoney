using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_PerfectMoney.Helper;
using TelegramBot_PerfectMoney.OperationBot;

namespace TelegramBot_PerfectMoney.TelegramPresentation
{
    public class TelegramBot
    {
        private UserStepHandler stepHandler { get; set; }

        private int PageNumber { get; set; }
        private IOperationTelegramBot _operation { get; set; }
        private CancellationTokenSource cts { get; }
        public TelegramBot(IOperationTelegramBot operation)
        {
            _operation = operation;
            cts = new();
            stepHandler = new();
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
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            

            if (update?.Message is not null || update.CallbackQuery is not null)
            {
                if (update.Message?.Text == "/start")
                {
                    // جهت چک کردن اینکه شماره تلفن کاربر ثبت شده یا نه
                    // if (Identity)
                    // {
                    //      
                    // }
                    await _operation.Start(botClient, update, cancellationToken);
                    UserStepHandler.DeleteAll(update.Message.Chat.Id.ToString());


                }
                else if(update.Type == UpdateType.CallbackQuery)
                {
                     if (update.CallbackQuery?.Data == "لیست بعد")
                    {
                        PageNumber += 1;
                        var result = UserStepHandler.GetUserLastStep(update.CallbackQuery.Message.Chat.Id.ToString());
                        await _operation.GetUserList(botClient, update, cancellationToken, PageNumber.ToString());
                    }
                    else if (update.CallbackQuery?.Data == "لیست قبل")
                    {
                        PageNumber -= 1;
                        if (PageNumber < 1)
                            PageNumber = 1;

                        await _operation.GetUserList(botClient, update, cancellationToken, PageNumber.ToString());
                    }
                }
                

               else if (update.Type == UpdateType.Message)
                {
                    var typkeyborad = UserStepHandler.GetUserLastStep(update.Message.Chat.Id.ToString());
                    #region About Admin Panel


                    if (update.Message?.Text == "لیست کاربران 📄" && typkeyborad == CreatKeyboard.SetAdminActiveSellingMainKeyboard())
                    {
                        await _operation.AdminUserListSection(botClient, update, cancellationToken);
                    }
                    else if (update.Message?.Text == "مدیریت " + "👨🏼‍💼" )
                    {
                        await _operation.AdminMainSection(botClient, update, cancellationToken);

                    }
                    else if (update.Message?.Text == "نمایش کاربران 🧑" && typkeyborad == CreatKeyboard.UserListKeyboard())
                    {
                        if (PageNumber > 1 || PageNumber == 0)
                            PageNumber = 1;
                        await _operation.GetUserList(botClient, update, cancellationToken, PageNumber.ToString());
                    }
                    else if (update.Message.Text == "ارسال پیام همگانی 📧" && typkeyborad == CreatKeyboard.SetAdminActiveSellingMainKeyboard())
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("لطفا پیام خودرا با فرمت زیر وارد کنید.");
                        stringBuilder.AppendLine("پیام به تمامی اعضا :");
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, stringBuilder.ToString(),
                            cancellationToken: cancellationToken, replyMarkup: CreatKeyboard.BackKeyboards());
                        UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(), CreatKeyboard.BackKeyboards());
                    }
                    else if(update.Message.Text.Contains("پیام") && typkeyborad==CreatKeyboard.BackKeyboards())
                    {
                       await _operation.SendMessageToAllUsers(botClient, update, cancellationToken);
                    }
                    else if (update.Message.Text == "شروع فروش ✔️")
                    {
                        if (typkeyborad == CreatKeyboard.SetAdminStopSellingKeyboard() ||
                            typkeyborad == CreatKeyboard.SetAdminActiveSellingMainKeyboard())
                        {
                           await _operation.ActivSelling(botClient, update, cancellationToken);
                            return;
                        }
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                            cancellationToken: cancellationToken);
                    }
                    else if (update.Message.Text == "توقف فروش \U0001f6d1")
                    {
                        if (typkeyborad == CreatKeyboard.SetAdminStopSellingKeyboard() ||
                            typkeyborad == CreatKeyboard.SetAdminActiveSellingMainKeyboard())
                        {
                           await _operation.StopSelling(botClient, update, cancellationToken);
                            return;
                        }
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                            cancellationToken: cancellationToken);
                    }
                    else if (update.Message?.Text == "جستجو 🔎" && typkeyborad == CreatKeyboard.UserListKeyboard())
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("لطفا شماره کاربر را با فرمت زیر وارد کنید.");
                        stringBuilder.AppendLine("شماره همراه : ****09");
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, stringBuilder.ToString(),
                            cancellationToken: cancellationToken, replyMarkup: CreatKeyboard.BackKeyboards());
                        UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(), CreatKeyboard.BackKeyboards());
                    }
                    else if(update.Message?.Text == "ارسال پیام به کاربر 📧" )
                    {
                        if (typkeyborad == CreatKeyboard.BlockUser() || typkeyborad == CreatKeyboard.ActivUser())
                        {
                            var stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine("لطفا پیام خودرا با فرمت زیر وارد کنید.");
                            stringBuilder.AppendLine("پیام از طرف مدیر بات :");
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, stringBuilder.ToString(),
                                cancellationToken: cancellationToken, replyMarkup: CreatKeyboard.BackKeyboards());
                            UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(), CreatKeyboard.BackKeyboards());
                            
                            return;
                        }
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                            cancellationToken: cancellationToken);
                    }
                    else if (update.Message.Text.Contains("پیام از طرف مدیر بات") && typkeyborad == CreatKeyboard.BackKeyboards())
                    {
                       await _operation.SendMessageToUser(botClient,update,cancellationToken);
                    }
                    else if (update.Message.Text.Contains("شماره همراه")&& typkeyborad == CreatKeyboard.BackKeyboards())
                    {
                        await _operation.SearchUserByPhoneNumber(botClient, update, cancellationToken);
                    }
                    else if(update.Message.Text == "مسدود کردن کاربر 🚧" )
                    {
                        if (typkeyborad == CreatKeyboard.BlockUser() || typkeyborad == CreatKeyboard.ActivUser())
                        {
                            await _operation.BlockUser(botClient, update, cancellationToken);
                            return;
                        }
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                            cancellationToken: cancellationToken);
                    }
                    else if(update.Message.Text == "فعال کردن کاربر ✔️")
                    {
                        if (typkeyborad == CreatKeyboard.BlockUser() || typkeyborad == CreatKeyboard.ActivUser())
                        {
                            await _operation.ActiveUser(botClient, update, cancellationToken);
                            return;
                        }
                        await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                            cancellationToken: cancellationToken);
                    }
                   
                    #endregion


                    else if (update.Message?.Text == "بازگشت به مرحله قبل")
                     {
                         await _operation.BackToPreviousnStep(botClient, update, cancellationToken);
                     }
                     else if (update.Message?.Text == "صفحه اصلی")
                     {
                         await _operation.BackToMainSection(botClient, update, cancellationToken);
                     }
                     else
                     {
                         await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, "عملیات نا معتبر",
                             cancellationToken: cancellationToken);
                     }
                }
               
                



              
            }



        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {


            Console.Write("Error");
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $" : [{apiRequestException.ErrorCode}]  {apiRequestException.Message}",
                _ => " : " + exception.ToString()
            };
            Console.ResetColor();
            Console.WriteLine(ErrorMessage);
            Console.WriteLine("\n\n Press any key for close program......");
            Console.ReadKey();
            return Task.CompletedTask;
        }

        public void Stop()
        {
            cts.Cancel();
        }
    }
}
