using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Player_entity
    {
        private List<Card_entity> cards;
        private int chips;
        private int stakes;
            
        public Player_entity()
        {
            this.cards = new List<Card_entity>();
            this.chips = 0;
        }

        public List<Card_entity> getCards()
        {
            return cards;
        }

        public void setChips(int value)
        {
            chips = value;
        }

        public int getChips()
        {
            return chips;
        }

        public void setStakes(int value)
        {
            stakes = value;
        }

        public int getStakes()
        {
            return stakes;
        }

        public void receiveCard(Card_entity card)
        {
            Console.WriteLine(card.getRank());
            Console.WriteLine(card.getSuit());
            cards.Add(card);
        }

        // is this function necessary?
        public void removeCards()
        {
            cards.Clear();
        }
    }
}
