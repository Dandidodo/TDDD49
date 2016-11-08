using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Player
    {
        private List<Card> cards;
        private int chips;
        private int stakes;
            
        public Player()
        {
            this.cards = new List<Card>();
            this.chips = 0;
        }

        public void setChips(int value)
        {
            chips = value;
        }

        public int getChips()
        {
            return chips;
        }

        public void receiveCard(Card card)
        {
            cards.Add(card);
        }

        public void removeCards()
        {
            cards.Clear();
        }
    }
}
