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
        static Random r = new Random();

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
            return Shuffle(temp_cards);
        }

        // Fisher-Yates shuffle
        // Loop through the entire deck and swap the place of two cards each time
        public List<Card> Shuffle(List<Card> cards)
        {
            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
            return cards;
        }

        public Card draw()
        {
            // Perhaps check if there are cards left in the deck before we do this?
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}
