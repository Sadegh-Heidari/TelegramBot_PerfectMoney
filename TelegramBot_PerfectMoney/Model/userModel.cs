using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot_PerfectMoney.Model
{
    public class userModel:Base
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserNameTelegram { get; set; }
        public string PhoneNumber { get; set; }
        public string CodeId { get; set; }
        public int MessageId { get; set; }
        public long ChatId { get; set; }
        public bool Active { get; set; }

        public userModel()
        {
            CreationDate = DateTime.Now;
            Active = true;
        }
    }
}
