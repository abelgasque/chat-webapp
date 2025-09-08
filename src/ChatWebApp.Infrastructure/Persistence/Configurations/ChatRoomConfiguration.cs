using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChatWebApp.Domain.Entities;

namespace ChatWebApp.Infrastructure.Persistence.Configurations
{
    public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.ToTable("ChatRooms");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(cr => cr.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(cr => cr.CreatedAt)
                   .IsRequired();

            builder.HasMany(cr => cr.Messages)
                   .WithOne(m => m.ChatRoom)
                   .HasForeignKey(m => m.ChatRoomId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}