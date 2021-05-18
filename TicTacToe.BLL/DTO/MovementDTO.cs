using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.BLL.DTO
{
    public class MovementDTO : BaseEntityDTO
    {
        public Int16 Number { get; set; }
        public Int16 Position { get; set; }
        public Guid GameId { get; set; }
        //public GameDTO Game { get; set; }
        public Guid PlayerId { get; set; }
        //public PlayerDTO Player { get; set; }
    }
}
