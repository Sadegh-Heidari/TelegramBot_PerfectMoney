using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelegramBot_PerfectMoney.Helper
{
    public static class ConvertHelper
    {
        public static bool IsIranianPhoneNumber(string text)
        {
            // تعریف الگوی منظم برای تشخیص شماره تلفن ایرانی
            string pattern = @"^(\+98|0)?9\d{9}$";

            // استفاده از تابع Match برای تست الگو در متن ورودی
            Match match = Regex.Match(text, pattern);

            // بررسی مطابقت الگو با متن ورودی
            return match.Success;
        }
    }
}
