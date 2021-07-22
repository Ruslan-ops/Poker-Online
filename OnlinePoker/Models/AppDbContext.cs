using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class AppDbContext : IdentityDbContext<IdentityPokerUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Winner> Winners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Table>().HasMany(t => t.Seats).WithOne(s => s.Table).HasForeignKey(s => s.TableId);

            builder.Entity<IdentityPokerUser>().HasIndex(u => u.UserName).IsUnique();

            builder.Entity<Deal>().Property(d => d.FinishedAt).IsRequired(false);
            builder.Entity<Deal>().Property(d => d.FinishedWithShowDown).IsRequired(false);

            builder.Entity<Player>().HasOne(p => p.Table).WithMany(t => t.Players).OnDelete(DeleteBehavior.SetNull);


        }

    }
}
