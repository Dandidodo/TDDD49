using Poker.Data_tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Deck
    {
        //TODO: Dependency inject new Deck_entity()
        Deck_entity deck_entity = new Deck_entity();

        internal Deck_entity Deck_entity
        {
            get
            {
                return deck_entity;
            }
        }

        public Deck()
        {
            initDeck();         
        }

        public void initDeck()
        {
            List<Data_tier.Card_entity> temp_cards = new List<Data_tier.Card_entity>();
            var suits = Enum.GetValues(typeof(Data_tier.Card_entity.Suit));

            foreach (Data_tier.Card_entity.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    temp_cards.Add(new Data_tier.Card_entity(suit, i));
                }
            }
            deck_entity.setCards(Shuffle(temp_cards));
        }

        // Fisher-Yates shuffle
        // Loop through the entire deck_entity and swap the place of two cards each time
        public List<Data_tier.Card_entity> Shuffle(List<Data_tier.Card_entity> cards)
        {
            Random r = new Random(); // Does not work apperently

            for (int n = cards.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Data_tier.Card_entity temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }
            return cards;
        }

        public Data_tier.Card_entity draw()
        {
            Data_tier.Card_entity card = deck_entity.getCards()[0];
            deck_entity.getCards().RemoveAt(0);
            return card;
        }
    }
}
