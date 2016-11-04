using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Card
    {
        private int rank;
        private Suit suit;

        public enum Suit { Spade, Heart, Diamond, Club };


        public Card(Suit suit, int rank)
        {
            this.suit = suit;
            this.rank = rank;
        }


        public Suit getSuit()
        {
            return suit;
        }

        public int getRank()
        {
            return rank;
        }

        public void setRank(int rank)
        {
            //TODO: Validera att rank är korrekt, 2<=rank<=14
            this.rank = rank;
        }

        public void setSuit(Suit suit)
        {
            this.suit = suit;
        }
    }
}
