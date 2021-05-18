using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.DAL.Entities
{
    public class Player : BaseEntity
    {
        public String Name { get; set; }
        public ICollection<GamePlayer> PlayerGames { get; set; }
        public Int32 GamesCount { get; set; }
        public Int32 WinsCount { get; set; }
        public Int32 FailuresCount { get; set; }
        public Player()
        {
            PlayerGames = new List<GamePlayer>();
        }
    }
}
