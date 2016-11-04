using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Deck
    {
        List<Card> cards = new List<Card>();

        public Deck()
        {
            var suits = Enum.GetValues(typeof(Card.Suit));

            foreach (Card.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    cards.Add(new Card(suit, i));
                }
            }
        }


    }
}
