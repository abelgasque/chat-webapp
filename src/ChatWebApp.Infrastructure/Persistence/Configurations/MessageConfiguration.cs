using ChatWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatWebApp.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(m => m.Content)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(m => m.SentAt)
                   .IsRequired();

            builder.HasOne(m => m.User)
                   .WithMany()
                   .HasForeignKey(m => m.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.ChatRoom)
                   .WithMany(cr => cr.Messages)
                   .HasForeignKey(m => m.ChatRoomId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}