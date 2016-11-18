using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic_tier
{
    class Table
    {

        private Table_entity table_entity;

        public Table()
        {
            this.table_entity = new Table_entity();
            Console.WriteLine("Table created");
        }

        public List<Player_entity> initPlayers()
        {
            List<Player_entity> temp_players = new List<Player_entity>();
            Player_entity player1 = new Player_entity();
            Player_entity player2 = new Player_entity();
            Player_entity player3 = new Player_entity();
            Player_entity player4 = new Player_entity();
            Player_entity player5 = new Player_entity();

            temp_players.Add(player1);
            temp_players.Add(player2);

            return temp_players;
        }

        public void playGame()
        {
            while (table_entity.getPlayers().Count > 1)
            {
                table_entity.getRules().newHand();
                while (table_entity.getRules().getActivePlayers().Count > 1)
                {
                    //rules.playRound();
                }
            }
        }

        public void addCommunityCard()
        {
            //communityCards.Add(deck_entity.draw());
        }

        public void playerFold()
        {

        }

        public void playerRaise()
        {

        }

        public void playerCall()
        {

        }
    }
}
