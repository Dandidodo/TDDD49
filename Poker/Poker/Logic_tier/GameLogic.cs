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
        private Table_entity table;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private Player_entity currentPlayer;
        private TexasHoldemRules rules;

        public GameLogic(Table_entity table_entity)
        {
            table = table_entity;
            rules = new TexasHoldemRules();

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
                        
            giveStartingChips();
            newHand();

            rules.findWinner(table_entity, table.getPlayers());
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

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void newHand()
        {
            roundCounter = 0;

            foreach (Player_entity player in table.getPlayers())
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            table.getDeck().initDeck();

            changeBlindIndexes();
            insertBlinds();

            lastRaise = BIGBLIND;
            minRaise = BIGBLIND * 2;

            currentPlayer = (indexBigBlind == table.getPlayers().Count - 1) ? table.getPlayers()[0] : table.getPlayers()[indexBigBlind + 1];

            setCommunityCards();
            dealCards();
        }

        public void changeBlindIndexes()
        {
            indexBigBlind = (indexBigBlind == table.getPlayers().Count - 1) ? indexBigBlind = 0 : ++indexBigBlind;
            indexSmallBlind = (indexSmallBlind == table.getPlayers().Count - 1) ? indexSmallBlind = 0 : ++indexSmallBlind;
        }

        public void insertBlinds()
        {

            insertPlayerChips(table.getPlayers()[indexBigBlind], BIGBLIND);
            insertPlayerChips(table.getPlayers()[indexSmallBlind], BIGBLIND / 2);
        }

        public void giveStartingChips()
        {
            foreach (Player_entity player in table.getPlayers())
            {
                player.setChips(STARTINGCHIPS);
            }
        }

        public void dealCards()
        {
            foreach (Player_entity player in table.getPlayers())
            {
                player.removeCards();
                player.receiveCard(table.getDeck().draw());
                player.receiveCard(table.getDeck().draw());
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
            foreach (Player_entity player in table.getPlayers())
            {
                if (player.Active == true)
                    activePlayers++;
            }
            return activePlayers == 1;
        }

        public void setCommunityCards()
        {
            table.removeCards();
            for (int i = 0; i < 5; i++)
                table.setCM(table.getDeck().draw());
        }

        // function for fold
        public void playerFold()
        {
            currentPlayer.ActedThisRound = true;
            // Move the players stakes to the pot
            table.setPot(table.getPot() + currentPlayer.getStakes());
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
        }

        public void giveWinnings(Player_entity player)
        {
            player.setChips(player.getChips() + table.getPot() + player.getStakes());
            player.setStakes(0);
            table.setPot(0);
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
                    Player_entity player = rules.findWinner(table, table.getPlayers());
                    giveWinnings(player);
                    newHand();
                }
            }
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
        }

        // TODO: Move this to a better suited place
        // Find the next player, we have to check a special case if the current active player is in the end of players, then we have to
        // return the first active player found.
        public Player_entity getNextActivePlayer(Player_entity player)
        {
            Player_entity nextPlayer = new Player_entity();
            bool playerFound = false;
            bool firstActivePlayerFound = false;

            foreach (Player_entity p in table.getPlayers())
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
            foreach (Player_entity player in table.getPlayers())
            {
                player.ActedThisRound = false;
                table.setPot(table.getPot() + player.getStakes());
                player.setStakes(0);
            }
            List<Player_entity> players = table.getPlayers();
            currentPlayer = indexSmallBlind != 0 ? getNextActivePlayer(players[indexSmallBlind - 1]) : getNextActivePlayer(players[players.Count() - 1]);
            lastRaise = 0;
            minRaise = BIGBLIND;
            roundCounter++;
        }

        
    }
}