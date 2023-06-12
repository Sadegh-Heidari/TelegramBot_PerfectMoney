using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot_PerfectMoney.Model;

namespace TelegramBot_PerfectMoney.DataBase
{
    public class UserMapping:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.CodeId).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ChatId).IsRequired();
            builder.Property(x => x.MessageId).IsRequired();
            builder.Property(x => x.Active).IsRequired();
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(200);
            builder.Property(x => x.UserNameTelegram).IsRequired().HasMaxLength(200);
        }
    }
}
