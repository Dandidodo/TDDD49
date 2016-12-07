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

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
            Console.WriteLine("Texas created");
                        
            giveStartingChips();
            newHand();

            findWinner(); //testing this atm
        }

        public Player_entity getCurrentPlayer()
        {
            return currentPlayer;
        }

        public void newHand()
        {
            roundCounter = 0;

            foreach (Player_entity player in players)
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            deck.initDeck();

            changeBlindIndexes();
            insertBlinds();

            lastRaise = bigBlind;
            minRaise = bigBlind * 2;

            currentPlayer = (indexBigBlind == players.Count - 1) ? players[0] : players[indexBigBlind + 1];

            setCommunityCards();
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
                // River
                if (roundCounter == 4)
                {
                    // Calculate who has the best hand
                    Player_entity player = findWinner();
                    giveWinnings(player);
                    newHand(); // remove this when showdown is implemented
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

        public void endRound()
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
            roundCounter++;
        }

        // Rank each hand from 1-10 from best to worst, also rank the strenght of the hand,
        // say two players have a pair, which one is the strongest.
        private Player_entity findWinner()
        {
            int bestHandVal = 0;
            int playerHandVal = 0;
            Player_entity bestPlayer = new Player_entity();
            foreach (Player_entity player in players)
            {
                if (player.Active)
                {
                    List<Card_entity> allCards = new List<Card_entity>(table.getCommunityCards());
                    allCards.Add(player.getCards()[0]);
                    allCards.Add(player.getCards()[1]);
                    List<Card_entity> sortedCards = allCards.OrderBy(c => c.getRank()).ToList();
                    sortedCards.Reverse();
                    playerHandVal = checkSameRank(sortedCards);

                    if (playerHandVal > bestHandVal)
                    {
                        bestHandVal = playerHandVal;
                        bestPlayer = player;
                    }

                    // Do this in the future if we want to
                    //checkFlush(allCards);
                    //checkStraight(allCards);
                }
            }
            return bestPlayer;
            /*
            //Fake shit for testing purposes only
            List<Card_entity> testStraight = new List<Card_entity>();
            testStraight.Add(new Card_entity(Card_entity.Suit.Spade, 3));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 3));
            testStraight.Add(new Card_entity(Card_entity.Suit.Diamond, 3));
            testStraight.Add(new Card_entity(Card_entity.Suit.Spade, 3));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 14));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 10));
            testStraight.Add(new Card_entity(Card_entity.Suit.Club, 10));

            List<Card_entity> sortedCards = testStraight.OrderBy(c => c.getRank()).ToList();
            sortedCards.Reverse();

            int playerVal = checkSameRank(sortedCards);
            */
        }

        //int value;
        //value += 900000; //royal flush
        // can only be one royal flush at the same time

        //int value;
        //value += 800000; //straight flush
        //value += highestrankedcardinstraightflush

        // DONE
        //value += 700000; //4 of a kind
        //value += fourofakindvalue*10 + highestrankedcard //value four of a kind more than the highest ranked card

        // DONE
        //value += 600000; //full house
        //value += value of three of a kind + pair

        //value += 500000 //flush
        //value += highestcard1*10 000 + highestcard2*1000 + highestcard3*100  + highestcard4*10 + highestcard5

        //value += 400000 //straight
        //value + highestrankedcard

        // DONE
        //value += 300000 //three of a kind
        //threeofakindval*100 + highestcard1*10 + highestcard2 //value three of a kind more than the highest ranked card

        // DONE
        //value += 200000 //two pairs
        //first pair*100 + second pair*10 + highestrankedcard //value three of a kind more than the highest ranked card

        // DONE
        //value += 100000 //one pair
        //value + pair_val*1000 + highestcard1*100 + highestcard2*10 + highestcard3

        // DONE
        // highest card
        //highestcard1*10 000 + highestcard2*1000 + highestcard3*100  + highestcard4*10 + highestcard5



        private int checkSameRank(List<Card_entity> combinedCards)
        {
            Dictionary<int, int> rankCounter = new Dictionary<int, int>();

            int occurenceCounter = 1;

            //This counter assumes that combinedCards is sorted.
            for (int card_index = 1; card_index < combinedCards.Count; ++card_index)
            {
                if (combinedCards[card_index].getRank() != combinedCards[card_index - 1].getRank())
                {
                    if (occurenceCounter > 1)
                    {
                        Card_entity card = combinedCards[card_index-1];
                        rankCounter.Add(card.getRank(), occurenceCounter);
                        occurenceCounter = 1;
                    }                    
                }
                else
                {
                    occurenceCounter++;
                }
            }
            if (occurenceCounter > 1)
            {
                Card_entity card = combinedCards[combinedCards.Count - 1];
                rankCounter.Add(card.getRank(), occurenceCounter);
                occurenceCounter = 1;
            }

            /*
            //Detta är en dum lösning, vi kan loopa en gång och incrementa samma siffra om den redan finns....
            for (int card1_index = 0; card1_index < combinedCards.Count; card1_index++)
            {
                for (int card2_index = card1_index + 1; card2_index < combinedCards.Count; card2_index++)
                {
                    Card_entity card1 = combinedCards[card1_index];
                    Card_entity card2 = combinedCards[card2_index];

                    if (card1.getRank() == card2.getRank())
                    {
                        Console.WriteLine("card 1 rank: " + card1.getRank());
                        if (rankCounter.ContainsKey(card1.getRank()))
                        {
                            int number_of_cards_with_same_rank = rankCounter[card1.getRank()];
                            rankCounter[card1.getRank()] = number_of_cards_with_same_rank + 1; //Something more than a pair, three of a kind etc...
                        }
                        else
                        {
                            rankCounter.Add(card1.getRank(), 2); //We found a pair
                        }
                    }
                }
            }
            */

            bool three_of_a_kind = false;
            int three_of_a_kind_val = 0;

            bool pair = false;
            int pair_val = 0;

            foreach (KeyValuePair<int, int> rank in rankCounter)
            {
                if (rank.Value == 4) // Value = number_of_cards_with_same_rank
                {
                    // Get the highest card that is not part of the four of a kind.
                    int highestKicker = combinedCards[0].getRank() == rank.Key ? combinedCards[4].getRank() : combinedCards[0].getRank();
                    return 700000 + rank.Key * 100 + highestKicker;
                }
                else if (rank.Value == 3 && !three_of_a_kind)
                {
                    three_of_a_kind_val = rank.Key;
                    three_of_a_kind = true;

                    //If full house
                    if (pair)
                        return 600000 + three_of_a_kind_val * 100 + pair_val;
                }
                else if (rank.Value == 2)
                {
                    //If full house
                    if (three_of_a_kind)
                    {
                        return 600000 + three_of_a_kind_val * 100 + rank.Key;
                    }
                    else if (pair) // We already have a pair => Two pairs
                    {
                        int highestKicker;

                        // Get the two pairs + highest kicker
                        if (combinedCards[0].getRank() != pair_val)
                            highestKicker = combinedCards[0].getRank();
                        else if (combinedCards[2].getRank() != rank.Key)
                            highestKicker = combinedCards[2].getRank();
                        else
                            highestKicker = combinedCards[4].getRank();

                        return 200000 + pair_val * 1000 + rank.Key * 100 + highestKicker;
                    }
                    else
                    {
                        pair_val = rank.Key;
                        pair = true;
                    }
                }
            }

            if (three_of_a_kind)
            {
                Console.WriteLine(combinedCards[0].getRank());
                Console.WriteLine(combinedCards[1].getRank());
                Console.WriteLine(combinedCards[2].getRank());
                Console.WriteLine(combinedCards[3].getRank());
                Console.WriteLine(combinedCards[4].getRank());
                Console.WriteLine(combinedCards[5].getRank());
                Console.WriteLine(combinedCards[6].getRank());
                int highestKicker = combinedCards[0].getRank() == three_of_a_kind_val ? combinedCards[3].getRank() : combinedCards[0].getRank();
                int secondHighestKicker = combinedCards[1].getRank() == three_of_a_kind_val ? combinedCards[4].getRank() : combinedCards[1].getRank();
                return 300000 + three_of_a_kind_val * 100 + highestKicker * 10 + secondHighestKicker;
            }
            else if (pair)
            {
                int highestKicker = combinedCards[0].getRank() == pair_val ? combinedCards[2].getRank() : combinedCards[0].getRank();
                int secondHighestKicker = combinedCards[1].getRank() == pair_val ? combinedCards[3].getRank() : combinedCards[1].getRank();
                int thirdHighestKicker = combinedCards[2].getRank() == pair_val ? combinedCards[4].getRank() : combinedCards[2].getRank();
                return 100000 + three_of_a_kind_val * 1000 + highestKicker * 100 + secondHighestKicker * 10 + thirdHighestKicker;
            }
            else
            {
                return combinedCards[0].getRank() + combinedCards[1].getRank() + combinedCards[2].getRank() + combinedCards[3].getRank() + combinedCards[4].getRank();
            }
        }

        /*
        private int[,] evalHand(Dictionary<int,int> rankCounter)
        {
        }*/

        // Returns if any suit occures 5 times or more.
        private bool checkFlush(List<Card_entity> combinedCards)
        {
            int heartCount = 0;
            int spadesCount = 0;
            int diamondCount = 0;
            int clubCount = 0;


            for (int i = 0; i < combinedCards.Count; i++)
            {
                Card_entity.Suit currentSuit = combinedCards[i].getSuit();

                if (currentSuit == Card_entity.Suit.Heart)
                    heartCount++;
                else if (currentSuit == Card_entity.Suit.Spade)
                    spadesCount++;
                else if (currentSuit == Card_entity.Suit.Diamond)
                    diamondCount++;
                else
                    clubCount++;
            }

            return ((heartCount >= 5) || (spadesCount >= 5) || (diamondCount >= 5) || (clubCount >= 5));
        }

        private bool checkStraight(List<Card_entity> combinedCards)
        {


            int previousRank = combinedCards[0].getRank();
            int currentStreak = 0; // The cards rank has to increase five times, but we dont know where the straight starts.

            for (int i = 1; i < combinedCards.Count; i++)
            {
                int currentRank = combinedCards[i].getRank();
                if ((previousRank + 1) == currentRank)
                {
                    currentStreak++;
                    if (currentStreak == 4)
                        //TODO: Check if last card is Ace -> Set royal flag
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

