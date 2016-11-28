using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Player_entity : ICloneable
    {
        private List<Card_entity> cards;
        private int chips;
        private int stakes;
        private bool _active;

        public Player_entity()
        {
            cards = new List<Card_entity>();
            chips = 0;
            stakes = 0;
            active = false;
        }

        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public List<Card_entity> getCards()
        {
            return cards;
        }

        public void setChips(int value)
        {
            chips = value;
        }

        public int getChips()
        {
            return chips;
        }

        public void setStakes(int value)
        {
            stakes = value;
        }

        public int getStakes()
        {
            return stakes;
        }

        public void receiveCard(Card_entity card)
        {
            cards.Add(card);
        }

        // is this function necessary?
        public void removeCards()
        {
            cards.Clear();
        }

        public Player_entity Clone()
        {
            var clone = (Player_entity)this.MemberwiseClone();
            //HandleCloned(clone);
            return clone;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
