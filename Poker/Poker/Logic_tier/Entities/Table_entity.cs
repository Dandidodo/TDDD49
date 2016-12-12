using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Table_entity
    {        
        private Deck deck = new Deck();
        private int pot;
        private List<Card_entity> communityCards;
        private List<Player_entity> players = new List<Player_entity>();
        private Player_entity player1;
        private Player_entity player2;
        private Player_entity player3;
        private Player_entity player4;
        private Player_entity player5;

        public Table_entity()
        {
            player1 = new Player_entity();
            player2 = new Player_entity();
            player3 = new Player_entity();
            player4 = new Player_entity();
            player5 = new Player_entity();

            players.Add(player1);
            players.Add(player2);
            players.Add(player3);
            players.Add(player4);
            players.Add(player5);

            pot = 0;
            communityCards = new List<Card_entity>();
            setPlayers(players);
        }

        public List<Card_entity> getCommunityCards()
        {
            return communityCards;
        }

        public Player_entity getPlayer1()
        {
            return player1;
        }

        public Player_entity getPlayer2()
        {
            return player2;
        }

        public Player_entity getPlayer3()
        {
            return player3;
        }

        public Player_entity getPlayer4()
        {
            return player4;
        }

        public Player_entity getPlayer5()
        {
            return player5;
        }

        public void setPlayers(List<Player_entity> players)
        {
            this.players = players;
        }

        public List<Player_entity> getPlayers()
        {
            return players;
        }

        public Deck getDeck()
        {
            return deck;
        }

        public void setPot(int value)
        {
            pot = value;
        }

        public int getPot()
        {
            return pot;
        }


        public void setCM(Card_entity card)
        {
            Console.WriteLine(card.getRank().ToString() + card.getSuit());
            communityCards.Add(card);
        }

        public void removeCards()
        {
            communityCards.Clear();
        }


        //TODO: Dessa fem kke passar bättre i TexasHoldemRules
        public Card_entity getCM1()
        {
            return communityCards[0];
        }

        public Card_entity getCM2()
        {
            return communityCards[1];
        }

        public Card_entity getCM3()
        {
            return communityCards[2];
        }

        public Card_entity getCM4()
        {
            return communityCards[3];
        }

        public Card_entity getCM5()
        {
            return communityCards[4];
        }
    }
}