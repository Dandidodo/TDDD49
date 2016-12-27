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
        Deck_entity deck_entity;

        internal Deck_entity Deck_entity
        {
            get
            {
                return deck_entity;
            }
        }

        public Deck(List<Data_tier.Card_entity> deck_of_cards, Deck_entity deck_entity)
        {
            this.deck_entity = deck_entity;
            deck_entity.setCards(Shuffle(deck_of_cards));        
        }
       
        // Fisher-Yates shuffle
        // Loop through the entire deck_entity and swap the place of two cards each time
        public List<Data_tier.Card_entity> Shuffle(List<Data_tier.Card_entity> cards)
        {
            Random r = new Random();

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
