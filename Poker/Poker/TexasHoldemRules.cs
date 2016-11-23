﻿using System;
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
        private List<Player_entity> activePlayers;
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
            newHand();
        }

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public List<Player_entity> getActivePlayers()
        {
            return activePlayers;
        }

        public void newHand()
        {
            activePlayers = players;
            deck.initDeck();     
            currentPlayer = players[indexBigBlind + 1];
            lastRaiserOrFirst = players[indexBigBlind];

            giveStartingChips();
            insertBlinds();
            dealCards();
            
            //playerAction(currentPlayer);
        }

        public void insertBlinds()
        {
            indexBigBlind = (indexBigBlind == players.Count - 1) ? indexBigBlind = 0 : indexBigBlind++;
            indexSmallBlind = (indexSmallBlind == players.Count - 1) ? indexSmallBlind = 0 : indexSmallBlind++;
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
                Console.WriteLine("if");
                player.setStakes(player.getChips());
                player.setChips(0);
            } else
            {
                Console.WriteLine("else");
                player.setStakes(chips);
                player.setChips(player.getChips() - chips);
            }
        }

        public void isRoundFinished(Player_entity currentPlayer)
        {
            if (lastRaiserOrFirst != currentPlayer)
            {
                // Check if it was the last round

                // Round is finished
                dealCommunityCards();
                roundCounter++;
                currentPlayer = activePlayers[indexSmallBlind];
                //playerAction(currentPlayer);
            }
        }

        public void dealCommunityCards()
        {            
            //table.addCommunityCard();

            // Deal two extra cards for the flop
            if (roundCounter == 0)
            {
                //table.addCommunityCard();
                //table.addCommunityCard();
            }
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
            activePlayers.Remove(currentPlayer);
            
            currentPlayer = nextPlayer;          
            //playerAction();
        }

        // function for call
        public void playerCall()
        {
            // Insert chips
            // Check if player is out of chips
            // NextPlayer
        }

        // function for raise
        public void playerRaise()
        {
            // Insert chips
            // Set player to lastRaiserOrFirst
            // Check if player is out of chips
            // NextPlayer
        }

        //TODO: Move this to a better suited place
        public Player_entity getNextPlayer()
        {
            // Hitta index ur activePlayers, ta nästa, om slutet ta 0te player.
            int currentPlayerIndex = activePlayers.IndexOf(currentPlayer);
            return (currentPlayerIndex == activePlayers.Count - 1) ?  activePlayers[0] : activePlayers[++currentPlayerIndex];
        }
    }
}

