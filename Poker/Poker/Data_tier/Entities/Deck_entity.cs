using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier
{
    class Deck_entity
    {
        private List<Card_entity> cards = new List<Card_entity>();

        public void setCards(List<Card_entity> cards)
        {
            this.cards = cards;
        }

        public List<Card_entity> getCards()
        {
            return cards;
        }
    }
}
