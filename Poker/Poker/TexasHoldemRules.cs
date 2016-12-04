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
        private int bigBlind = 20;
        private int lastRaise;
        private int minRaise; // minimum allowed raise
        private bool limit;
        private List<Player_entity> players;
        private Deck deck;
        private Table_entity table;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private Player_entity currentPlayer;

        // Move to table_entity
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

        public TexasHoldemRules(Table_entity table, List<Player_entity> players, Deck deck, bool limit)
        {
            this.table = table;
            this.players = players;
            this.deck = deck;
            this.limit = limit;

            lastRaise = bigBlind;
            minRaise = bigBlind * 2;

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
            Console.WriteLine("Texas created");

            setCommunityCards();
            giveStartingChips();
            newHand();
            compareHands(); //testing this atm
        }

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void newHand()
        {
            foreach (Player_entity player in players)
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            deck.initDeck();

            changeBlindIndexes();
            insertBlinds();

            currentPlayer = (indexBigBlind == players.Count - 1) ? players[0] : players[indexBigBlind + 1];
            
            dealCards();
        }

        public void changeBlindIndexes()
        {
            indexBigBlind = (indexBigBlind == players.Count - 1) ? indexBigBlind = 0 : ++indexBigBlind;
            indexSmallBlind = (indexSmallBlind == players.Count - 1) ? indexSmallBlind = 0 : ++indexSmallBlind;
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
            foreach (Player_entity player in players)
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
                player.setStakes(player.getStakes() + player.getChips());
                player.setChips(0);
            } else
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
            foreach (Player_entity player in players)
            {
                if (player.Active == true)
                    activePlayers++;
            }
            return activePlayers == 1;
        }

        public void setCommunityCards()
        {
            for (int i = 0; i < 5; i++)
                table.setCM(deck.draw());
        }
        
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

            // Check if the hand is finished
            if (handIsFinished())
            {
                giveWinnings();
                newHand();
            }            
        }

        public void giveWinnings()
        {
            currentPlayer.setChips(currentPlayer.getChips() + table.getPot() + currentPlayer.getStakes());
            currentPlayer.setStakes(0);
            table.setPot(0);
        }
        
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
                newRound();
                // River
                if (roundCounter == 3)
                {
                    // Calculate who has the best hand
                    //showDown();
                    newHand(); // remove this when showdown is implemented
                }
                else {
                    roundCounter++;
                }     
            }
        }

        public void newRound()
        {
            foreach (Player_entity player in players)
            {
                player.ActedThisRound = false;
                table.setPot(table.getPot() + player.getStakes());
                player.setStakes(0);
            }
            currentPlayer = getNextActivePlayer(players[indexSmallBlind - 1]); // Will not work when indexSmallBlind == 0
            lastRaise = 0;
            minRaise = bigBlind;
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

            foreach (Player_entity p in players)
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

        private void compareHands()
        {
            /*
            foreach (Player_entity player in players)
            {
                if (player.Active)
                {
                    //Tilldela rank baserad på om spelare har ngt av dessa?
                    //TODO: Highest card måste vi ta hänsyn till, om ingen spelare har ngn av nedanstående.
                    List<Card_entity> allCards = new List<Card_entity>(table.getCommunityCards());
                    allCards.Add(player.getCards()[0]);
                    allCards.Add(player.getCards()[1]);

                    checkSameRank(allCards);
                    checkFlush(allCards);
                    checkStraight(allCards);
                }
            }*/

            //Fake shit for testing purposes only
            List<Card_entity> testStraight = new List<Card_entity>();
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 1));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 4));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 5));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 6));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 7));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 8));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 2));

            checkFlush(testStraight);
        }


        private void checkSameRank(List<Card_entity> allCards)
        {
            Dictionary<int, int> rankCounter = new Dictionary<int, int>();

            for (int card1_index = 0; card1_index < allCards.Count; card1_index++)
            {
                for (int card2_index = card1_index + 1; card2_index < allCards.Count; card2_index++)
                {
                    Card_entity card1 = allCards[card1_index];
                    Card_entity card2 = allCards[card2_index];

                    if (card1!= card2)
                    {
                        if (card1.getRank() == card2.getRank())
                        {
                            if (rankCounter.ContainsKey(card1.getRank()))
                            {
                                int value = rankCounter[card1.getRank()];
                                rankCounter[card1.getRank()] = value + 1; //Something more than a pair, three of a kind etc...
                            }
                            else
                            {
                                rankCounter.Add(card1.getRank(), 1); //We found a pair
                            }
                        }
                    }
                }
            }
            Console.WriteLine(rankCounter);
        }

        private bool checkFlush(List<Card_entity> allCards)
        {
            for (int card1_index = 0; card1_index < allCards.Count; card1_index++)
            {
                for (int card2_index = card1_index + 1; card2_index < allCards.Count; card2_index++)
                {
                    Card_entity card1 = allCards[card1_index];
                    Card_entity card2 = allCards[card2_index];

                    if (card1 != card2)
                    {
                        if (card1.getSuit() != card2.getSuit())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //TODO: Doesnt work atm
        private bool checkStraight(List<Card_entity> allCards)
        {
            List<Card_entity> sortedCards = allCards.OrderBy(c => c.getRank()).ToList();

            int previousRank = sortedCards[0].getRank();
            int currentStreak = 0; // The cards rank has to increase five times, but we dont know where the straight starts.

            for(int i = 1; i < allCards.Count; i++)
            {
                int currentRank = sortedCards[i].getRank();
                if ((previousRank + 1) == currentRank)
                {
                    currentStreak++;
                    if (currentStreak == 4)
                        return true; //Here we have found it
                }
                else
                {
                    currentStreak = 0;
                }

                previousRank = currentRank;

            }
            return false;
        }
    }
}

