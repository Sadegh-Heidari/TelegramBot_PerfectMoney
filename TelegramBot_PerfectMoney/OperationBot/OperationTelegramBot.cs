using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_PerfectMoney.DataBase;
using TelegramBot_PerfectMoney.Helper;
using TelegramBot_PerfectMoney.Model;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramBot_PerfectMoney.OperationBot
{
    public class OperationTelegramBot:IOperationTelegramBot
    {
        // private static int number = 1;
        private static string SavePhonNumber { get; set; }
        private TelContext _context { get; set; }
        private PersianCalendar Persian { get; set; }
        public OperationTelegramBot(TelContext context)
        {
            _context = context;
            Persian = new PersianCalendar();
            
        }

        public  async Task Start(ITelegramBotClient botClient,Update update, CancellationToken cancellationToken)
        {
            // var user = new userModel()
            // {
            //     ChatId = update.Message!.Chat.Id,
            //     CodeId = "1111111",
            //     FirstName = "ali",
            //     LastName = "hey",
            //     MessageId = update.Message.MessageId,
            //     PhoneNumber = "09104",
            //     UserNameTelegram = update.Message.Chat.Username!
            // };
            // _context.Users.Add(user);
            // _context.SaveChanges();
            var mainKeyboardMarkup= CreatKeyboard.SetMainKeyboardMarkup();
            UserStepHandler.DeleteAll(update.Message.Chat.Id.ToString());
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "به بات خرید پرفکت مانی خوش آمدید.",
                replyMarkup: mainKeyboardMarkup,
                cancellationToken: cancellationToken);
            
        }

        public async Task AdminMainSection(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var Adminkeyboard = CreatKeyboard.SetAdminMainKeyboard();
            UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),Adminkeyboard);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "پنل مدیریت",
                replyMarkup: Adminkeyboard,
                cancellationToken: cancellationToken);
        }

        public async Task GetContact(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var ShareContactKeyboard = CreatKeyboard.GetContactKeyboard();
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "لطفا شماره تلفن خودروا ارسال کنید",
                replyMarkup: ShareContactKeyboard,
                cancellationToken: cancellationToken);
        }

        public async Task AdminUserListSection(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var AdminUser = CreatKeyboard.UserListKeyboard();
            UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),AdminUser);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "بخش مدیریت کاربران",
                replyMarkup: AdminUser,
                cancellationToken: cancellationToken);
        }

        public async Task BackToMainSection(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UserStepHandler.DeleteAll(update.Message.Chat.Id.ToString());
           var mainKeyboardMarkup = CreatKeyboard.SetMainKeyboardMarkup();

           Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: update.Message!.Chat.Id,
               text: "صفحه اصلی",
               replyMarkup: mainKeyboardMarkup,
               cancellationToken: cancellationToken);
        }

        public async Task BackToPreviousnStep(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;

            UserStepHandler.RemoveCurentUserLastStep(chatId.ToString());

            var lastStep = UserStepHandler.GetUserLastStep(chatId.ToString());
            
            if (lastStep != null)
            {
                await botClient.SendTextMessageAsync(chatId, $"یکی از گزینه های زیر را انتخاب کنید",replyMarkup:lastStep, cancellationToken: cancellationToken);
                
            }
            else
            {
                // در صورت عدم وجود مرحله قبلی
               await botClient.SendTextMessageAsync(chatId, "مرحله قبلی یافت نشد.", cancellationToken: cancellationToken);
            }
        }

        public async Task GetUserList(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken,string page)
        {
            
            var key = CreatKeyboard.PaginitionUserListKeyboard();

            
            var result = await _context.Users.Skip((Convert.ToInt32(page) - 1) * 10).Take(10).ToListAsync();
            if (result.Count == 0)
            {
                await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "اطلاعات وجود ندارد", cancellationToken: cancellationToken);
                return;
            }

            var text = new StringBuilder();
           ;
            foreach (var item in result)
            {
                var activeText = item.Active == true ? "فعال" : "مسدود";
                var year = Persian.GetYear(item.CreationDate).ToString();
                var month = Persian.GetMonth(item.CreationDate).ToString();
                var day = Persian.GetDayOfMonth(item.CreationDate).ToString();
                var hour = Persian.GetHour(item.CreationDate);
                var minute = Persian.GetMinute(item.CreationDate);
                text.AppendLine($"{result.IndexOf(item)+1}- نام : {item.FirstName} ------ نام خانوادگی : {item.LastName}");
                text.AppendLine($"نام کاربری : {item.UserNameTelegram} ------ شماره همراه : {item.PhoneNumber}");
                text.AppendLine($"کد ملی : {item.CodeId} ------ وضعیت فعالی : {activeText}");
                text.AppendLine($"تاریخ ثبت نام : {year}/{month}/{day} ------ ساعت ثبت نام : {hour}:{minute}");
                text.AppendLine();
                text.AppendLine("------------------------------------------------");
                text.AppendLine();
            }

            if (result.Count < 10)
            {
                await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, text.ToString(), cancellationToken: cancellationToken);
                await botClient.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId, new InlineKeyboardMarkup(new InlineKeyboardButton("لیست قبل") { CallbackData = "لیست قبل" }), cancellationToken: cancellationToken);
                return;
            }

          
            if (update.CallbackQuery is not null)
            {
                if (Convert.ToInt32(page) == 1)
                {
                    await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, text.ToString(), cancellationToken: cancellationToken);
                    await botClient.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id,
                        update.CallbackQuery.Message.MessageId, new InlineKeyboardMarkup(new InlineKeyboardButton("لیست بعد") { CallbackData = "لیست بعد" }), cancellationToken: cancellationToken);
                    return;
                }
                await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId, text.ToString(), cancellationToken: cancellationToken);
                await botClient.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId, key, cancellationToken: cancellationToken);
            }
            else
            {
                var count = _context.Users.Count();
                await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    $"مجموع کاربران : {count}",replyMarkup: CreatKeyboard.BackKeyboards(), cancellationToken: cancellationToken);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, text.ToString(), replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton("لیست بعد"){CallbackData = "لیست بعد"}),
                    cancellationToken: cancellationToken);
                UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),CreatKeyboard.BackKeyboards());
            }
            
           

        }

        public async Task SendNumberRequest(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("لطفا شماره کاربر را با فرمت زیر وارد کنید.");
            stringBuilder.AppendLine("شماره همراه : ****09");
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,stringBuilder.ToString(),
                cancellationToken: cancellationToken,replyMarkup:CreatKeyboard.BackKeyboards());
            UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),CreatKeyboard.BackKeyboards());
        }

        public async Task SearchUserByPhoneNumber(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var extractNumber = ConvertHelper.ExtractNumberFromText(update.Message.Text);
            
            var checkPhone = ConvertHelper.IsIranianPhoneNumber(extractNumber);
            if (!checkPhone)
            {
               await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "فرمت شماره تلفن اشتباه هست لطفا دوباره ارسال کنید.", cancellationToken: cancellationToken);
                return;
            }
         
         
            var result = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == extractNumber);
            if (result is null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "این کاربر در سیستم موجود نمیباشد.میتوانید شماره دیگری را ارسال کنید.", cancellationToken: cancellationToken);
                return;
            }
            var activeText = result.Active == true ? "فعال" : "مسدود";
            var year = Persian.GetYear(result.CreationDate).ToString();
            var month = Persian.GetMonth(result.CreationDate).ToString();
            var day = Persian.GetDayOfMonth(result.CreationDate).ToString();
            var hour = Persian.GetHour(result.CreationDate);
            var minute = Persian.GetMinute(result.CreationDate);
            var text = new StringBuilder();
            text.AppendLine($"نام : {result.FirstName} ------ نام خانوادگی : {result.LastName}");
            text.AppendLine($"نام کاربری : {result.UserNameTelegram} ------ شماره همراه : {result.PhoneNumber}");
            text.AppendLine($"کد ملی : {result.CodeId} ------ وضعیت فعالی : {activeText}");
            text.AppendLine($"تاریخ ثبت نام : {year}/{month}/{day} ------ ساعت ثبت نام : {hour}:{minute}");
            text.AppendLine($"موجودی:---- مجموع سفارشات:-----");
            text.AppendLine();
            SavePhonNumber = result.PhoneNumber;
            if (result.Active)
            {
                var button = CreatKeyboard.BlockUser();
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, text.ToString(),
                    cancellationToken: cancellationToken, replyMarkup: button);
                UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),button);
                return;
            }
            else
            {
                var button = CreatKeyboard.ActivUser();
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, text.ToString(),
                    cancellationToken: cancellationToken, replyMarkup: button);
                UserStepHandler.AddUserStep(update.Message.Chat.Id.ToString(),button);
                return;
            }
           
        }

        public async Task ActiveUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == SavePhonNumber);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "این کاربر وجود ندارد",
                    cancellationToken: cancellationToken);
                return;
            }

            user.Active = true;
            _context.SaveChanges();
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "کاربر با موفقیت فعال شد",
                cancellationToken: cancellationToken,replyMarkup:CreatKeyboard.BlockUser());

        }

        public async Task BlockUser(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == SavePhonNumber);
            if (user == null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "این کاربر وجود ندارد",
                    cancellationToken: cancellationToken);
                return;
            }

            user.Active = false;
            _context.SaveChanges();
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "کاربر با مسدود شد",
                cancellationToken: cancellationToken, replyMarkup: CreatKeyboard.ActivUser());
        }
    }
}
