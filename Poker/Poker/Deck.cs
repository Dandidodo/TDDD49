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
            this.cards = initDeck();
        }

        public List<Card> initDeck()
        {
            List<Card> temp_cards = new List<Card>();
            var suits = Enum.GetValues(typeof(Card.Suit));

            foreach (Card.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    temp_cards.Add(new Card(suit, i));
                    Console.WriteLine(temp_cards.Last().getRank().ToString() + temp_cards.Last().getSuit());
                }
            }
            return temp_cards;
        }


    }
}
