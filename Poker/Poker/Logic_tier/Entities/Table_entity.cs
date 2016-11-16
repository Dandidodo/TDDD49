using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Table_entity
    {
        private List<Player_entity> players = new List<Player_entity>();
        private TexasHoldemRules rules;
        private Deck deck = new Deck();
        private int pot;
        private List<Card_entity> communityCards;

        public Table_entity()
        {
            //this.players = initPlayers();
            this.pot = 0;
            this.communityCards = new List<Card_entity>();
            
            this.rules = new TexasHoldemRules(this, players, deck, true); // How do we set the limits
            //playGame();
        }

        public void setPlayers(List<Player_entity> players)
        {
            this.players = players;
        }

        public List<Player_entity> getPlayers()
        {
            return players;
        }

        public void setDeck(int value)
        {
            pot = value;
        }

        public int getDeck()
        {
            return pot;
        }

        public void setPot(int value)
        {
            pot = value;
        }

        public int getPot()
        {
            return pot;
        }
        
        public TexasHoldemRules getRules()
        {
            return rules;
        }        
    }
}