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

        public Table()
        {
            this.players = initPlayers();
            
            this.rules = new TexasHoldemRules(players, deck, chips); ;
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
    }
}