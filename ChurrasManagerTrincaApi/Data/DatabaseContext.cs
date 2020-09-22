using ChurrasManagerTrincaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurrasManagerTrincaApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Churrasco> Churrascos { get; set; }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<ChurrascoUser> ChurrascoUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChurrascoUser>()
                .HasKey(chuser => new { chuser.ChurrascoId, chuser.UserId });
            modelBuilder.Entity<ChurrascoUser>()
                .HasOne(chuser => chuser.Churrasco)
                .WithMany(c => c.Convidados)
                .HasForeignKey(chuser => chuser.ChurrascoId);
            modelBuilder.Entity<ChurrascoUser>()
                .HasOne(chuser => chuser.User)
                .WithMany(u => u.Churrascos)
                .HasForeignKey(chuser => chuser.UserId);
        }
    }
}