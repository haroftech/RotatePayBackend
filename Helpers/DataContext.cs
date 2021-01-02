using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserUpload> UserUploads { get; set; }
        public DbSet<PaymentNotification> PaymentNotifications { get; set; }
    }
}