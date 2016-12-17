using Poker.Logic_tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class GameLogic
    {
        private const int PLAYERCARDS = 2;
        private const int COMMUNITYCARDS = 5;
        private const int STARTINGCHIPS = 1000;
        private const int BIGBLIND = 20;
        private int lastRaise;
        private int minRaise; // minimum allowed raise
        private Table_entity table_entity;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private Player_entity currentPlayer;
        private TexasHoldemRules rules;
        private Data data;

        public GameLogic(Table_entity table_entity)
        {
            this.table_entity = table_entity;
            rules = new TexasHoldemRules();
            data = new Data();

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
                        
            giveStartingChips();
            newHand();

            data.loadData(table_entity);
            // FIlen kan inte finnas, fel i XML, fel i XML gentemot oss
        }

        // Move to rules?
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

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void newHand()
        {
            roundCounter = 0;

            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            table_entity.getDeck().initDeck();

            changeBlindIndexes();
            insertBlinds();

            lastRaise = BIGBLIND;
            minRaise = BIGBLIND * 2;

            currentPlayer = (indexBigBlind == table_entity.getPlayers().Count - 1) ? table_entity.getPlayers()[0] : table_entity.getPlayers()[indexBigBlind + 1];

            setCommunityCards();
            dealCards();
        }

        public void changeBlindIndexes()
        {
            indexBigBlind = (indexBigBlind == table_entity.getPlayers().Count - 1) ? indexBigBlind = 0 : ++indexBigBlind;
            indexSmallBlind = (indexSmallBlind == table_entity.getPlayers().Count - 1) ? indexSmallBlind = 0 : ++indexSmallBlind;
        }

        public void insertBlinds()
        {

            insertPlayerChips(table_entity.getPlayers()[indexBigBlind], BIGBLIND);
            insertPlayerChips(table_entity.getPlayers()[indexSmallBlind], BIGBLIND / 2);
        }

        public void giveStartingChips()
        {
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.setChips(STARTINGCHIPS);
            }
        }

        public void dealCards()
        {
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.removeCards();
                player.receiveCard(table_entity.getDeck().draw());
                player.receiveCard(table_entity.getDeck().draw());
            }
        }

        public void insertPlayerChips(Player_entity player, int chips)
        {
            if (player.getChips() < chips)
            {
                // If player dont have enough chips
                player.setStakes(player.getStakes() + player.getChips());
                player.setChips(0);
            }
            else
            {
                player.setStakes(player.getStakes() + chips);
                player.setChips(player.getChips() - chips);
            }
        }

        public bool roundIsFinished()
        {
            return (currentPlayer.getStakes() == lastRaise && currentPlayer.ActedThisRound);
        }

        public bool handIsFinished()
        {
            int activePlayers = 0;
            foreach (Player_entity player in table_entity.getPlayers())
            {
                if (player.Active == true)
                    activePlayers++;
            }
            return activePlayers == 1;
        }

        public void setCommunityCards()
        {
            table_entity.removeCards();
            for (int i = 0; i < 5; i++)
                table_entity.setCM(table_entity.getDeck().draw());
        }

        // function for fold
        public void playerFold()
        {
            currentPlayer.ActedThisRound = true;
            // Move the players stakes to the pot
            table_entity.setPot(table_entity.getPot() + currentPlayer.getStakes());
            currentPlayer.setStakes(0);

            // Get next player
            Player_entity nextPlayer = getNextActivePlayer(currentPlayer);

            // Remove player from active players
            currentPlayer.Active = false;

            currentPlayer = nextPlayer;

            if (roundIsFinished()) // Everyone has acted at least once and there is no raise
            {
                endRound(); // Move all stakes to the pot etc
            }
            if (handIsFinished()) // Check if the hand is finished (if there's only 1 active player left)
            {
                giveWinnings(currentPlayer);
                newHand();
            }

            data.saveData(table_entity, this);
        }

        public void giveWinnings(Player_entity player)
        {
            player.setChips(player.getChips() + table_entity.getPot() + player.getStakes());
            player.setStakes(0);
            table_entity.setPot(0);
        }

        // function for call
        public void playerCall()
        {
            currentPlayer.ActedThisRound = true;
            // Insert chips
            insertPlayerChips(currentPlayer, lastRaise - currentPlayer.getStakes());

            // Check if player is out of chips
            // NextPlayer
            // Get next player
            Player_entity nextPlayer = getNextActivePlayer(currentPlayer);

            currentPlayer = nextPlayer;

            if (roundIsFinished())
            {
                endRound();
                // If River
                if (roundCounter == 4)
                {
                    // Calculate who has the best hand
                    Player_entity player = rules.findWinner(table_entity, table_entity.getPlayers());
                    giveWinnings(player);
                    newHand();
                }
            }

            data.saveData(table_entity, this);
        }

        // function for raise
        public void playerRaise(int raise)
        {
            currentPlayer.ActedThisRound = true;

            minRaise = raise + (raise - lastRaise);
            lastRaise = raise;

            // Insert chips
            insertPlayerChips(currentPlayer, raise - currentPlayer.getStakes());

            // TODO: Check if player is out of chips

            // NextPlayer
            Player_entity nextPlayer = getNextActivePlayer(currentPlayer);

            currentPlayer = nextPlayer;

            data.saveData(table_entity, this);
        }

        // TODO: Move this to a better suited place
        // Find the next player, we have to check a special case if the current active player is in the end of players, then we have to
        // return the first active player found.
        public Player_entity getNextActivePlayer(Player_entity player)
        {
            Player_entity nextPlayer = new Player_entity();
            bool playerFound = false;
            bool firstActivePlayerFound = false;

            foreach (Player_entity p in table_entity.getPlayers())
            {
                if (p == player)
                {
                    playerFound = true;
                }
                else if (playerFound == true && p.Active == true)
                {
                    return p;
                }
                else if (p.Active == true && !firstActivePlayerFound)
                {
                    nextPlayer = p;
                    firstActivePlayerFound = true;
                }
            }
            return nextPlayer;
        }

        public void endRound()
        {
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.ActedThisRound = false;
                table_entity.setPot(table_entity.getPot() + player.getStakes());
                player.setStakes(0);
            }
            List<Player_entity> players = table_entity.getPlayers();
            currentPlayer = indexSmallBlind != 0 ? getNextActivePlayer(players[indexSmallBlind - 1]) : getNextActivePlayer(players[players.Count() - 1]);
            lastRaise = 0;
            minRaise = BIGBLIND;
            roundCounter++;
        }

        
    }
}