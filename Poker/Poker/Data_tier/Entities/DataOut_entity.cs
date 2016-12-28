using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    class DataOut_entity
    {
        //Community cards
        private enum saveResponse { fileNotFound, incorrectFormat, success}
        private enum gameResponce { success, error }

        //player cards, player active, player stakes
        private List<List<string>> playerCards = new List<List<string>>();
        private List<bool> playerActive = new List<bool>();
        private List<int> playerStakes = new List<int>();
        private List<int> playerChips = new List<int>();

        //Intressant ifrån table 
        private int pot, roundCounter, currentPlayerIndex, lastRaise, minRaise, currentPlayerStakes, currentPlayerChips;
        private List<List<string>> communityCards = new List<List<string>>();

        public DataOut_entity()
        {

        }

        public List<List<string>> PlayerCards
        {
            get
            {
                return playerCards;
            }

            set
            {
                playerCards = value;
            }
        }

        public List<bool> PlayerActive
        {
            get
            {
                return playerActive;
            }

            set
            {
                playerActive = value;
            }
        }

        public List<int> PlayerStakes
        {
            get
            {
                return playerStakes;
            }

            set
            {
                playerStakes = value;
            }
        }

        public List<int> PlayerChips
        {
            get
            {
                return playerChips;
            }

            set
            {
                playerChips = value;
            }
        }

        public int Pot
        {
            get
            {
                return pot;
            }

            set
            {
                pot = value;
            }
        }

        public int RoundCounter
        {
            get
            {
                return roundCounter;
            }

            set
            {
                roundCounter = value;
            }
        }

        public int CurrentPlayerIndex
        {
            get
            {
                return currentPlayerIndex;
            }

            set
            {
                currentPlayerIndex = value;
            }
        }

        public int LastRaise
        {
            get
            {
                return lastRaise;
            }

            set
            {
                lastRaise = value;
            }
        }

        public int MinRaise
        {
            get
            {
                return minRaise;
            }

            set
            {
                minRaise = value;
            }
        }

        public int CurrentPlayerStakes
        {
            get
            {
                return currentPlayerStakes;
            }

            set
            {
                currentPlayerStakes = value;
            }
        }

        public int CurrentPlayerChips
        {
            get
            {
                return currentPlayerChips;
            }

            set
            {
                currentPlayerChips = value;
            }
        }
    }
}
