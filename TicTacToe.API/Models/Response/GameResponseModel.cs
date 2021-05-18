using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.API.Models.Response
{
    public class GameResponseModel
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerPiece { get; set; }
        public bool IsComplete { get; set; }
        public string[] GameBoard { get; set; }
        public short?[] WinNumbers { get; set; }
        public Guid? WinnerId { get; set; }

    }
}
