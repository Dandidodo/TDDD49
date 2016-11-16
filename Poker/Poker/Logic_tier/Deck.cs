using Poker.Logic_tier.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Deck
    {
        Deck_entity deck_entity = new Deck_entity();

        public Deck()
        {
            this.deck_entity.setCards(initDeck());            
        }

        public List<Card_entity> initDeck()
        {
            List<Card_entity> temp_cards = new List<Card_entity>();
            var suits = Enum.GetValues(typeof(Card_entity.Suit));

            foreach (Card_entity.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    temp_cards.Add(new Card_entity(suit, i));
                    //Console.WriteLine(temp_cards.Last().getRank().ToString() + temp_cards.Last().getSuit());
                }
            }
            return Shuffle(temp_cards);
        }

        // Fisher-Yates shuffle
        // Loop through the entire deck_entity and swap the place of two cards each time
        public List<Card_entity> Shuffle(List<Card_entity> cards)
        {
            Random r = new Random();

            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card_entity temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
            return cards;
        }

        public Card_entity draw()
        {
            // Perhaps check if there are cards left in the deck_entity before we do this?
            Card_entity card = deck_entity.getCards()[0];
            deck_entity.getCards().RemoveAt(0);
            return card;
        }
    }
}
