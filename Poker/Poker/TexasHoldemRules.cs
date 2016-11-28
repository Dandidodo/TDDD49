using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class TexasHoldemRules
    {
        // How many cards players have
        // Number of community cards
        // How the blinds work
        // Ante
        // Maximum/minimum bet
        // Throw cards
        // Value of hands

        private const int playerCards = 2;
        private const int communityCards = 5;
        private const int startingChips = 1000;
        private const int bigBlind = 20;
        private bool limit;
        private List<Player_entity> players;
        private Deck deck;
        private Table_entity table;
        private int pot;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private Player_entity currentPlayer;
        private Player_entity lastRaiserOrFirst;
        

        public enum Choice { FOLD, CHECK, RAISE };


        public TexasHoldemRules(Table_entity table, List<Player_entity> players, Deck deck, bool limit)
        {
            this.table = table;
            this.players = players;
            this.deck = deck;
            this.limit = limit;

            indexBigBlind = 2;
            indexSmallBlind = 1;
            roundCounter = 0;
            Console.WriteLine("Texas created");

            dealCommunityCards();
            newHand();
        }

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void newHand()
        {
            //activePlayers = players;
            foreach(Player_entity player in players)
            {
                player.active = true;
            }

            deck.initDeck();
            changeBlindIndexes();
            currentPlayer = players[indexBigBlind + 1];
            lastRaiserOrFirst = players[indexBigBlind];

            giveStartingChips();
            insertBlinds();
            dealCards();
            
            //playerAction(currentPlayer);
        }

        public void insertBlinds()
        {
            
            insertPlayerChips(players[indexBigBlind], bigBlind);
            insertPlayerChips(players[indexSmallBlind], bigBlind / 2);
        }

        public void giveStartingChips()
        {
            foreach (Player_entity player in players)
            {
                player.setChips(startingChips);            
            }
        }

        public void dealCards()
        {
            foreach(Player_entity player in players)
            {
                player.receiveCard(deck.draw());
                player.receiveCard(deck.draw());
            }
        }

        public void insertPlayerChips(Player_entity player, int chips)
        {
            if (player.getChips() < chips)
            {
                // If player dont have enough chips
                player.setStakes(player.getChips());
                player.setChips(0);
            } else
            {
                player.setStakes(chips);
                player.setChips(player.getChips() - chips);
            }
        }

        public void isRoundFinished()
        {
            int activePlayers = 0;
            foreach (Player_entity player in players)
            {
                if (player.active == true)
                    activePlayers++;
            }
            if (activePlayers == 1)
            {
                currentPlayer.setChips(currentPlayer.getChips() + table.getPot() + currentPlayer.getStakes());
                currentPlayer.setStakes(0);
                table.setPot(0);
                //newHand();
            }
        }

        public void dealCommunityCards()
        {
            for (int i = 0; i < 5; i++)
                table.setCM(deck.draw());
        }

        public void playerAction()
        {
            //1) Fold, 2) Call, 3) Raise
            // Show graphical options to current player


            /* 
            while(true)
            {             
               if (playerChoice == Choice.FOLD)
               {

               } else if (playerChoice == Choice.CHECK)
               {

               } else if (playerChoice == Choice.RAISE)
               {

               }           
            }
            */

        }

        // function for fold
        public void playerFold()
        {
            // Move the players stakes to the pot
            table.setPot(table.getPot() + currentPlayer.getStakes());
            currentPlayer.setStakes(0);

            // Get next player
            Player_entity nextPlayer = getNextPlayer();

            // Remove player from active players
            currentPlayer.active = false;
            
            currentPlayer = nextPlayer;

            // Check if winner
            isRoundFinished();

            //playerAction();
        }

        // function for call
        public void playerCall()
        {
            // Insert chips
            insertPlayerChips(currentPlayer, bigBlind);

            // Check if player is out of chips
            // NextPlayer
            // Get next player
            Player_entity nextPlayer = getNextPlayer();

            currentPlayer = nextPlayer;
            Console.WriteLine("CALL");
        }

        // function for raise
        public void playerRaise()
        {
            // Insert chips
            // Set player to lastRaiserOrFirst
            // Check if player is out of chips
            // NextPlayer
        }

        public void changeBlindIndexes()
        {
            indexBigBlind = (indexBigBlind == players.Count - 1) ? indexBigBlind = 0 : indexBigBlind++;
            indexSmallBlind = (indexSmallBlind == players.Count - 1) ? indexSmallBlind = 0 : indexSmallBlind++;
        }


        // TODO: Move this to a better suited place
        // Find the next player, we have to check a special case if the current active player is in the end of players, then we have to
        // return the first active player found.
        public Player_entity getNextPlayer()
        {
            Player_entity nextPlayer = new Player_entity();
            bool currentPlayerFound = false;
            bool firstActivePlayerFound = false;

            foreach (Player_entity player in players)
            {
                if (player == currentPlayer)
                {
                    currentPlayerFound = true;
                }
                else if (currentPlayerFound == true)
                {
                    return player;
                }
                else if (player.active == true && !firstActivePlayerFound)
                {
                    nextPlayer = player;
                    firstActivePlayerFound = true;
                }                
            }
            return nextPlayer;
        }
    }
}

