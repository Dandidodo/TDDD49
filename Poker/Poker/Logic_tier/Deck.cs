using Poker.Data_tier;
using Poker.Data_tier.Entities;
using Poker.Logic_tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Poker
{
    class Deck
    {
        Deck_entity deck_entity;

        internal Deck_entity Deck_entity
        {
            get
            {
                return deck_entity;
            }
        }

        public Deck(List<Card_entity> deck_of_cards, Deck_entity deck_entity)
        {
            this.deck_entity = deck_entity;
            deck_entity.setCards(Shuffle(deck_of_cards));        
        }

        public void generateNewDeck()
        {
            List<Card_entity> deck_of_cards = new List<Card_entity>();
            var suits = Enum.GetValues(typeof(Card_entity.Suit));

            foreach (Card_entity.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    deck_of_cards.Add(new Card_entity(suit, i));
                }
            }
            deck_entity.setCards(Shuffle(deck_of_cards));
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
            Card_entity card = deck_entity.getCards()[0];
            deck_entity.getCards().RemoveAt(0);

            if(deck_entity.getCards().Count() == 0)
            {
                throw new EmptyDeckException("Deck is empty");
            }
            return card;
        }
    }

}
