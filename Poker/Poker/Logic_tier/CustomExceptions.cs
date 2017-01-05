using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic_tier
{
    public class EmptyDeckException : Exception
    {
        public EmptyDeckException()
        {

        }

        public EmptyDeckException(string message)
            : base(message)
        {
        }

        public EmptyDeckException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    

    public class WrongFormat : Exception
    {
        public WrongFormat()
        {
        }

        public WrongFormat(string message)
            : base(message)
        {
        }

        public WrongFormat(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
