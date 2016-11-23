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

        public Table(Table_entity table_entity)
        {      
            this.table_entity = table_entity;

            Console.WriteLine("Table created");
        }

        /*
        // Maybe re-implement this with an int for the number of players as an argument
        public List<Player_entity> initPlayers()
        {
            List<Player_entity> temp_players = new List<Player_entity>();            

            temp_players.Add(player1);
            temp_players.Add(player2);
            temp_players.Add(player3);
            temp_players.Add(player4);
            temp_players.Add(player5);

            return temp_players;
        }
        */

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
