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
        private int pot;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int bigBlind;
        private Player currentPlayer;
        
        public TexasHoldemRules(bool limit, List<Player> players, Deck deck)
        {
            this.limit = limit;
            this.players = players;
            this.deck = deck;
            
            indexBigBlind = 1;
            indexSmallBlind = 0;
            newHand();
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

        public void 

        public void insertPlayerChips(Player player, int chips)
        {
            if (player.getChips() < chips)
            {
                
            }
        } 
    }
}
