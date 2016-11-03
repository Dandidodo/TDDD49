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
        public enum Suit { Spade, Heart, Diamond, Club };

        private Suit suit;

        /*
        public Card() {
            x = 0;
            y = 0;
        }
        */           

        public Card(Suit suit, int rank){
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
    }
}
