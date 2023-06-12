using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_PerfectMoney.OperationBot
{
    internal static class CreatKeyboard
    {
        private static ReplyKeyboardMarkup? mainKeyboardMarkup { get; set; }
        private static ReplyKeyboardMarkup? AdminMainKeyboradMarkup { get; set; }
        private static ReplyKeyboardMarkup? ShareContactKeyboradMarkup { get; set; }
        public static ReplyKeyboardMarkup SetMainKeyboardMarkup()
        {
            if (mainKeyboardMarkup == null)
            {
                mainKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[] { "خرید 💸", "موجودی 💳"},
                    new KeyboardButton[] { "احراز هویت 🔒", "قوانین ⚖️" },
                    new KeyboardButton[] { "مدیریت " + "👨🏼‍💼" },
                })
                {
                    ResizeKeyboard = true
                };
            }
            return mainKeyboardMarkup;

        }

        public static ReplyKeyboardMarkup SetAdminMainKeyboard()
        {
            if (AdminMainKeyboradMarkup is null)
            {
                AdminMainKeyboradMarkup = new(new[]
                {
                    new KeyboardButton[]{ "لیست کاربران 📄", "ارسال پیام 📧" },
                    new KeyboardButton[]{ "توقف فروش 🛑", "در دست تعمیر 🛠️" },
                    new KeyboardButton[]{"بازگشت به صفحه اصلی"}

                }) { ResizeKeyboard = true };
            }

            return AdminMainKeyboradMarkup;
        }

        public static ReplyKeyboardMarkup GetContactKeyboard()
        {
            if (ShareContactKeyboradMarkup is null)
            {
                ShareContactKeyboradMarkup = new(new[]
                {
                    KeyboardButton.WithRequestContact("ارسال شماره تلفن ☎️")
                }) { ResizeKeyboard = true };
            }

            return ShareContactKeyboradMarkup;
        }
    }
    
}
