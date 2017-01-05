using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    class Player_entity : ICloneable
    {
        private List<Card_entity> cards;
        private int chips;
        private int stakes;
        private bool active;
        private bool actedThisRound;

        public Player_entity()
        {
            cards = new List<Card_entity>();
            chips = 0;
            stakes = 0;
            active = false;
            actedThisRound = false;
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public bool ActedThisRound
        {
            get { return actedThisRound; }
            set { actedThisRound = value; }
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
        
        public void removeCards()
        {
            cards.Clear();
        }

        public Player_entity Clone()
        {
            var clone = (Player_entity)this.MemberwiseClone();
            return clone;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
