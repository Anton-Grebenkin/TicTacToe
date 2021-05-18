using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.API.Models.Response
{
    public class PlayerResponseModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Int32 GamesCount { get; set; }
        public Int32 WinsCount { get; set; }
        public Int32 FailuresCount { get; set; }
    }
}
