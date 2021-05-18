using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string itemName) : base($"{itemName} not found.")
        {

        }
    }
}
