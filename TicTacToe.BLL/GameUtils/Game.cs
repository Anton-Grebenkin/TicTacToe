using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;

namespace TicTacToe.BLL.GameUtils
{
    public static class Game
    {
        public static bool MovementsIsLeft(IEnumerable<MovementDTO> movements)
        {
            if (movements.Count() < 9)
            {
                return true;
            }
            return false;
        }

        public static Guid GetWinnerId(IEnumerable<MovementDTO> movements, out short?[] winNumbers)
        {
            foreach (var playerId in movements.Select(m => m.PlayerId).Distinct())
            {
                var playerMoves = movements.Where(m => m.PlayerId == playerId).Select(m => m.Position);
                for (short i = 0, j = 0; i < 3; i++, j += 3)
                {
                    if (playerMoves.Contains(i) && playerMoves.Contains((short)(i + 3)) && playerMoves.Contains((short)(i + 6)))
                    {
                        winNumbers = new short?[3] {i, (short)(i+3), (short)(i+6) };
                        return playerId;
                    }
                    if (playerMoves.Contains(j) && playerMoves.Contains((short)(j + 1)) && playerMoves.Contains((short)(j + 2)))
                    {
                        winNumbers = new short?[3] { j, (short)(j + 1), (short)(j + 2) };
                        return playerId;
                    }
                }
                if (playerMoves.Contains((short)0) && playerMoves.Contains((short)4) && playerMoves.Contains((short)8))
                {
                    winNumbers = new short?[3] { 0, 4, 8 };
                    return playerId;
                }
                if (playerMoves.Contains((short)2) && playerMoves.Contains((short)4) && playerMoves.Contains((short)6))
                {
                    winNumbers = new short?[3] { 2, 4, 6 };
                    return playerId;
                }
            }
            winNumbers = new short?[3];
            return Guid.Empty;
        }

        public static bool GameIsValid(IEnumerable<MovementDTO> movements, out string message)
        {
            if (movements.Count() > 9)
            {
                message = "There can be no more than 9 movements.";
                return false;
            }
            if (movements.Select(m => m.PlayerId).Distinct().Count() > 2)
            {
                message = "There can be no more than 2 players.";
                return false;
            }

            var positions = movements.Select(m => m.Position);
            if (positions.Where(p => p > 8 || p < 0).Any())
            {
                message = "The position of the movement can take values from 0 to 8.";
                return false;
            }
            if (positions.Distinct().Count() < positions.Count())
            {
                message = "There can make movement one position only once.";
                return false;
            }

            var movementsList = movements.OrderBy(m => m.Number).ToList();
            for (int i = 1; i < movementsList.Count(); i++)
            {
                if (movementsList[i].PlayerId == movementsList[i - 1].PlayerId)
                {
                    message = "Players must make movements in turn.";
                    return false;
                }
            }

            message = "Game is valid.";
            return true;
        }
    }
}
