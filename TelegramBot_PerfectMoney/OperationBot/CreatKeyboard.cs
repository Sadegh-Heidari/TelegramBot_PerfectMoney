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
        private static ReplyKeyboardMarkup? UserListKeyboardMarkup { get; set; }
        private static InlineKeyboardMarkup? PaginitionListMarkup { get; set; }
        private static ReplyKeyboardMarkup? BackKeyboardsMarkup { get; set; }
        private static ReplyKeyboardMarkup? ActiveKeyboardMarkup { get; set; }
        private static ReplyKeyboardMarkup? BlockKeyboardMarkup { get; set; }

        public static ReplyKeyboardMarkup BackKeyboards()
        {
            if (BackKeyboardsMarkup is null)
            {
                BackKeyboardsMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton[] { "بازگشت به مرحله قبل" },
                        new KeyboardButton[] { "صفحه اصلی" },
                    })
                    { ResizeKeyboard = true };
            }
            return BackKeyboardsMarkup;
        }
        public static InlineKeyboardMarkup PaginitionUserListKeyboard()
        {
            
            if (PaginitionListMarkup is null)
            {
                PaginitionListMarkup = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "لیست قبل", callbackData: "لیست قبل"),
                    InlineKeyboardButton.WithCallbackData(text: "لیست بعد", callbackData: "لیست بعد"),
                });
            }

            return PaginitionListMarkup;
        }
        public static ReplyKeyboardMarkup UserListKeyboard()
        {
            if (UserListKeyboardMarkup is  null)
            {
                UserListKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[] { "نمایش کاربران \U0001f9d1", "جستجو 🔎" },
                    // new KeyboardButton[] { "", "فعال کردن کاربر ✔️" },
                    new KeyboardButton[] { "بازگشت به مرحله قبل"  },
                    new KeyboardButton[] { "صفحه اصلی"  },

                }) { ResizeKeyboard = true };
            }

            return UserListKeyboardMarkup;
        }
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

        public static ReplyKeyboardMarkup ActivUser()
        {
            if (ActiveKeyboardMarkup is null)
            {
                ActiveKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton[] { "فعال کردن کاربر ✔️" },
                        new KeyboardButton[] { "ارسال پیام به کاربر 📧", "لیست سفارشات کاربر 📄" },
                        new KeyboardButton[] { "مدیریت " + "👨🏼‍💼", "صفحه اصلی" }

                    })
                    { ResizeKeyboard = true };
            }
            return ActiveKeyboardMarkup;
        }

        public static ReplyKeyboardMarkup BlockUser()
        {
            if (BlockKeyboardMarkup == null)
            {
               BlockKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton[] { "مسدود کردن کاربر 🚧" },
                        new KeyboardButton[] { "ارسال پیام به کاربر 📧", "لیست سفارشات کاربر 📄" },
                        new KeyboardButton[] { "مدیریت "+ "👨🏼‍💼", "صفحه اصلی" }
                    })
                    { ResizeKeyboard = true };
            }

            return BlockKeyboardMarkup;
        }
        public static ReplyKeyboardMarkup SetAdminMainKeyboard()
        {
            if (AdminMainKeyboradMarkup is null)
            {
                AdminMainKeyboradMarkup = new(new[]
                {
                    new KeyboardButton[]{ "لیست کاربران 📄", "ارسال پیام 📧" },
                    new KeyboardButton[]{ "توقف فروش 🛑", "در دست تعمیر 🛠️" },
                    new KeyboardButton[]{"صفحه اصلی"}

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
