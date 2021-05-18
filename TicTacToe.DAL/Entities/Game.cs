using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.DAL.Entities
{
    public class Game : BaseEntity
    {
        public Boolean IsComplited { get; set; }
        public Guid? WinnerId { get; set; }
        public Player Winner { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }
        public ICollection<Movement> Movements { get; set; }
        public Game()
        {
            GamePlayers = new List<GamePlayer>();
            Movements = new List<Movement>();
        }
    }
}
