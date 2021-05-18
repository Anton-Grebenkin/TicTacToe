using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.BLL.DTO
{
    public class GamePlayerDTO : BaseEntityDTO
    {
        public Guid PlayerId { get; set; }
        //public PlayerDTO Player { get; set; }
        public Guid GameId { get; set; }
        //public GameDTO Game { get; set; }
        public Pieces Piece { get; set; }
    }
    public enum Pieces
    {
        X = 0,
        O = 1
    }
}
