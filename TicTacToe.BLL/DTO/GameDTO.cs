using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.BLL.DTO
{
    public class GameDTO : BaseEntityDTO
    {
        public Boolean IsCompleted { get; set; }
        public Guid? WinnerId { get; set; }
        public ICollection<GamePlayerDTO> GamePlayers { get; set; }
        public ICollection<MovementDTO> Movements { get; set; }
        public short?[] WinNumbers { get; set; }

        private string[] _gameBoard;
        public string[] GameBoard 
        { 
            get 
            {
               foreach(var movement in Movements)
                {
                    //if (GamePlayers.Where(gp => gp.PlayerId == movement.PlayerId).Any())
                    //{
                        _gameBoard[movement.Position] = GamePlayers.Where(gp => gp.PlayerId == movement.PlayerId).First().Piece == Pieces.X ? "X" : "O";
                    //}   
                }
                return _gameBoard;
            }
        }

        public GameDTO()
        {
            Movements = new List<MovementDTO>();
            GamePlayers = new List<GamePlayerDTO>();
            WinNumbers = new short?[3];
            _gameBoard = new string[9];
        }
    }
}
