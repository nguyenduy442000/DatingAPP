using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; } // Tạo ra 1 bảng trong SQL tên là  Users (Đối tượng trong bảng sẽ bao gồm trong class AppUser)
        public DbSet<UserLike> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
            .HasKey(k => new {k.SourceUserId, k.TargetUserId});

            builder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

                 
            builder.Entity<UserLike>()
            .HasOne(s=>s.TargetUser)
            .WithMany(l=>l.LikedByUsers)
            .HasForeignKey(s=>s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u => u.Recipient) //Một tin nhắn chỉ có một người nhận
            .WithMany(m =>m.MessageReceived) //một người nhận có thể nhận nhiều tin nhắn khác nhau.
            .OnDelete(DeleteBehavior.Restrict);
            // hệ thống sẽ ngăn cản nếu vẫn tồn tại các tin nhắn liên quan đến người nhận đó. 
            // Hành vi này giữ cho các tin nhắn không bị mất đi khi người nhận bị xóa.

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}