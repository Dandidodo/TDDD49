using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Table
    {
        List<Player> players = new List<Player>();
        TexasHoldemRules rules;
        Deck deck = new Deck();
        private int pot;
        private List<Card> communityCards;

        public Table()
        {
            this.players = initPlayers();
            this.pot = 0;
            this.communityCards = new List<Card>();
            
            this.rules = new TexasHoldemRules(this, players, deck, true); // How do we set the limits
            playGame();
        }

        public List<Player> initPlayers()
        {
            List<Player> temp_players = new List<Player>();
            Player player1 = new Player();
            Player player2 = new Player();

            temp_players.Add(player1);
            temp_players.Add(player2);

            return temp_players;
        }

        public void playGame()
        {
            while (players.Count > 1) {
                rules.newHand();
                while (rules.getActivePlayers().Count > 1)
                {
                    rules.playRound();
                }                
            }            
        }

        public void addCommunityCard()
        {
            communityCards.Add(deck.draw());
        }
    }
}