using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;
using TicTacToe.BLL.Interfaces;

namespace TicTacToe.BLL.GameUtils
{
    public class GameBot : IGameBot
    {
        public Guid Id { get; set; } = Guid.Parse("A50B0BD2-B99C-4C9A-B131-453447366EC3");
        public string Name { get; set; } = "Bot";

        public MovementDTO MakeMove(IEnumerable<MovementDTO> movements)
        {
            MovementDTO bestMove = new MovementDTO
            {
                PlayerId = Id,
                Number = (short)movements.Count()
            };
            if (movements.Count() == 0)
            {
                var random = new Random();
                bestMove.Position = (short)random.Next(0, 8);
                return bestMove;
            }
            var movementsList = movements.ToList();
           
            if (movementsList[0].PlayerId == Id)
            {
                int bestVal = -1000;
                for (short i = 0; i < 9; i++)
                {
                    if (!movementsList.Where(m => m.Position == i).Any())
                    {
                        var movement = new MovementDTO
                        {
                            Position = i,
                            PlayerId = Id
                        };

                        movementsList.Add(movement);
                        int moveVal = MiniMax(movementsList, 0, false);
                        movementsList.Remove(movement);
                        if (moveVal > bestVal)
                        {
                            bestMove.Position = i;
                            bestVal = moveVal;
                        }
                    }
                }
                return bestMove;
            }
            else
            {
                int bestVal = 1000;
                for (short i = 0; i < 9; i++)
                {
                    if (!movementsList.Where(m => m.Position == i).Any())
                    {
                        var movement = new MovementDTO
                        {
                            Position = i,
                            PlayerId = Id
                        };

                        movementsList.Add(movement);
                        int moveVal = MiniMax(movementsList, 0, true);
                        movementsList.Remove(movement);
                        if (moveVal < bestVal)
                        {
                            bestMove.Position = i;
                            bestVal = moveVal;
                        }
                    }
                }
                return bestMove;
            }
        }

        private int MiniMax(IEnumerable<MovementDTO> movements, int depth, bool isMax)
        {
            var movementsList = movements.ToList();
            var winnerId = Game.GetWinnerId(movements, out short?[] winNumbers);
            var xPlayerId = movementsList[0].PlayerId;
            var oPlayerId = Guid.Empty;
            if (movementsList.Count() > 1)
            {
                oPlayerId = movementsList[1].PlayerId;
            }
            else
            {
                oPlayerId = Id;
            }
            

            if (winnerId != Guid.Empty)
            {
                if (winnerId == xPlayerId)
                {
                    return 10;
                }
                else
                {
                    return -10;
                }
            }

            if (!Game.MovementsIsLeft(movementsList))
            {
                return 0;
            }

          
            if (isMax)
            {
                int best = -1000;

                for(short i = 0; i < 9; i++)
                {
                    if (!movementsList.Where(m => m.Position == i).Any())
                    {
                        var movement = new MovementDTO
                        {
                            Position = i,
                            PlayerId = xPlayerId,
                        };
                        movementsList.Add(movement);
                        best = Math.Max(best, MiniMax(movementsList, depth + 1, !isMax));
                        movementsList.Remove(movement);
                    }
                }

                return best;
            }

            
            else
            {
                int best = 1000;

                
                for (short i = 0; i < 9; i++)
                {
                    if (!movementsList.Where(m => m.Position == i).Any())
                    {
                        var movement = new MovementDTO
                        {
                            Position = i,
                            PlayerId = oPlayerId,
                        };
                        movementsList.Add(movement);
                        best = Math.Min(best, MiniMax(movementsList, depth + 1, !isMax));
                        movementsList.Remove(movement);
                    }
                }

                return best;
            }
        }
    }
}
