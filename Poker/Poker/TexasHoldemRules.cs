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
        private bool limit;
        private List<Player> players;
        private List<Player> activePlayers;
        private Deck deck;
        private Table table;
        private int pot;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int bigBlind;
        private int roundCounter;
        private Player currentPlayer;
        

        public TexasHoldemRules(Table table, List<Player> players, Deck deck, bool limit)
        {
            this.table = table;
            this.players = players;
            this.deck = deck;
            this.limit = limit;

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
        }

        public List<Player> getActivePlayers()
        {
            return activePlayers;
        }

        public void newHand()
        {
            activePlayers = players;
            deck.initDeck();
            insertBlinds();
            currentPlayer = players[indexBigBlind + 1];
            dealCards();      
        }

        public void insertBlinds()
        {
            indexBigBlind = (indexBigBlind == players.Count - 1) ? indexBigBlind = 0 : indexBigBlind++;
            indexSmallBlind = (indexSmallBlind == players.Count - 1) ? indexSmallBlind = 0 : indexSmallBlind++;
            insertPlayerChips(players[indexBigBlind], bigBlind);
            insertPlayerChips(players[indexSmallBlind], bigBlind / 2);
        }

        public void dealCards()
        {
            foreach(Player player in players)
            {
                player.receiveCard(deck.draw());
                player.receiveCard(deck.draw());
            }
        }

        public void insertPlayerChips(Player player, int chips)
        {
            if (player.getChips() < chips)
            {
                
            }
        }

        public void playRound()
        {
            Player currentPlayer = activePlayers[indexSmallBlind];
            Player lastRaiserOrFirst = currentPlayer; //Either the first player who starts the game or the last raiser.

            do
            {            
                if (roundCounter==0)
                {
                    currentPlayer = activePlayers[indexBigBlind + 1];
                }

                playerAction(currentPlayer);
                currentPlayer = nextPlayer(currentPlayer);                
            } while (lastRaiserOrFirst != currentPlayer);

            // Put community cards on the table
            dealCommunityCards();
                        
            roundCounter++;
        }

        public void dealCommunityCards()
        {            
            table.addCommunityCard();

            // Deal two extra cards for the flop
            if (roundCounter == 0)
            {
                table.addCommunityCard();
                table.addCommunityCard();
            }
        }

        public void playerAction(Player player)
        {
            //Spelaren får göra saker
        }

        //TODO: Move this to a better suited place
        public Player nextPlayer(Player currentPlayer)
        {
            // Hitta index ur activePlayers, ta nästa, om slutet ta 0te player.
            int currentPlayerIndex = activePlayers.IndexOf(currentPlayer);
            return (currentPlayerIndex == activePlayers.Count - 1) ?  activePlayers[0] : activePlayers[currentPlayerIndex++];
        }
    }
}

