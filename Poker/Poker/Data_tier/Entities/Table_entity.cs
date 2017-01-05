using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    class Table_entity
    {
        private Deck deck;
        private int pot;
        private List<Card_entity> communityCards;
        private List<Player_entity> players = new List<Player_entity>();
        private Player_entity player1;
        private Player_entity player2;
        private Player_entity player3;
        private Player_entity player4;
        private Player_entity player5;

        private Player_entity currentPlayer;

        public const int PLAYERCARDS = 2;
        public const int COMMUNITYCARDS = 5;
        public const int STARTINGCHIPS = 1000;
        public const int BIGBLIND = 20;
        
        private int lastRaise;
        private int minRaise; // minimum allowed raise
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private string foldWinner = "";

        public Table_entity(List<Player_entity> players, List<Card_entity> communityCards, Deck deck)
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

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
        }

        internal Player_entity CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        public int MinRaise
        {
            get { return minRaise; }
            set { minRaise = value; }
        }

        public int LastRaise
        {
            get { return lastRaise; }
            set { lastRaise = value; }
        }

        public int RoundCounter
        {
            get { return roundCounter; }
            set { roundCounter = value; }
        }

        public int IndexBigBlind
        {
            get
            {
                return indexBigBlind;
            }

            set
            {
                indexBigBlind = value;
            }
        }

        public int IndexSmallBlind
        {
            get
            {
                return indexSmallBlind;
            }

            set
            {
                indexSmallBlind = value;
            }
        }

        public string FoldWinner
        {
            get
            {
                return foldWinner;
            }

            set
            {
                foldWinner = value;
            }
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