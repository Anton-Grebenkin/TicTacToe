using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.DAL.Entities
{
    public class Movement : BaseEntity
    {
        public Int16 Number { get; set; }
        public Int16 Position { get; set; }
        public Guid GameId { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayeId")]
        public Player Player { get; set; }
    }
}
