using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier
{
    class Table_entity
    {
        private Deck deck;
        private int pot;
        private List<Data_tier.Card_entity> communityCards;
        private List<Player_entity> players = new List<Player_entity>();
        private Player_entity player1;
        private Player_entity player2;
        private Player_entity player3;
        private Player_entity player4;
        private Player_entity player5;

        public Table_entity(List<Player_entity> players, List<Data_tier.Card_entity> communityCards, Deck deck)
        {
            this.players = players;

            this.player1 = players[0];
            this.player2 = players[1];
            this.player3 = players[2];
            this.player4 = players[3];
            this.player5 = players[4];

            this.deck = deck;
            this.communityCards = communityCards;

            pot = 0;
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
            communityCards.Add(card);
        }

        public void removeCards()
        {
            communityCards.Clear();
        }

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