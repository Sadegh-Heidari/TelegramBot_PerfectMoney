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
    public class UserMapping:IEntityTypeConfiguration<userModel>
    {
        public void Configure(EntityTypeBuilder<userModel> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.FirstName).HasMaxLength(200);
            builder.Property(x => x.LastName).HasMaxLength(200);
            builder.Property(x => x.CodeId).HasMaxLength(200);
            builder.Property(x => x.ChatId);
            builder.Property(x => x.MessageId);
            builder.Property(x => x.Active);
            builder.Property(x => x.CreationDate);
            builder.Property(x => x.PhoneNumber).HasMaxLength(200);
            builder.Property(x => x.UserNameTelegram).IsRequired(false).HasMaxLength(200);
            builder.HasOne(x => x.Roles).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
        }
    }
}
