using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.DAL.Entities;

namespace TicTacToe.DAL.EF
{
    public class TicTacToeContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Movement> Movements { get; set; }

        public TicTacToeContext(DbContextOptions<TicTacToeContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(g => g.GamePlayers)
                .WithOne(gp => gp.Game)
                .HasForeignKey(g => g.GameId);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.PlayerGames)
                .WithOne(gp => gp.Player)
                .HasForeignKey(g => g.PlayerId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
