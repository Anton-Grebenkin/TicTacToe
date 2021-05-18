using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.BLL.DTO
{
    public class PlayerDTO : BaseEntityDTO
    {
        public String Name { get; set; }
        public Int32 GamesCount { get; set; }
        public Int32 WinsCount { get; set; }
        public Int32 FailuresCount { get; set; }
    }
}
