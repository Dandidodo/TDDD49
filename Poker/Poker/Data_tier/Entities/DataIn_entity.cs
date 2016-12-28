using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    //Better name: playerAction_entity
    class DataIn_entity
    {
        public enum action { fold,  raise, call};

        private int bet;
        private action playerAction;

        public DataIn_entity(int bet, action playerAction)
        {
            this.Bet = bet;
            this.PlayerAction = playerAction;
        }

        public action PlayerAction
        {
            get
            {
                return playerAction;
            }

            set
            {
                playerAction = value;
            }
        }

        public int Bet
        {
            get
            {
                return bet;
            }

            set
            {
                bet = value;
            }
        }
    }
}
