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

        private const int PLAYERCARDS = 2;
        private const int COMMUNITYCARDS = 5;
        private const int STARTINGCHIPS = 1000;
        private const int STRAIGHTFLUSH = 800000;
        private const int FOUROFAKIND = 700000;
        private const int FULLHOUSE = 600000;
        private const int FLUSH = 500000;
        private const int STRAIGHT = 400000;
        private const int THREEOFAKIND = 300000;
        private const int TWOPAIR = 200000;
        private const int PAIR = 100000;
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
                player.setChips(STARTINGCHIPS);
            }
        }

        public void dealCards()
        {
            foreach (Player_entity player in players)
            {
                player.removeCards();
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
            table.removeCards();
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
                // If River
                if (roundCounter == 4)
                {
                    // Calculate who has the best hand
                    Player_entity player = findWinner();
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
            //currentPlayer = getNextActivePlayer(players[indexSmallBlind - 1]); // Will not work when indexSmallBlind == 0
            currentPlayer = players[indexSmallBlind]; // Won't work if player is out of chips
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

                    // TODO: Do this in the future if we want to
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

        // TODO
        //value += 800000; //straight flush
        //value += highestrankedcardinstraightflush

        // TODO
        //value += 500000 //flush
        //value += highestcard1*10 000 + highestcard2*1000 + highestcard3*100  + highestcard4*10 + highestcard5

        // TODO        //value += 400000 //straight
        //value + highestrankedcard

        private Dictionary<int, int> countOccurencesOfRank(List<Card_entity> combinedCards)
        {
            Dictionary<int, int> occourencesOfRank = new Dictionary<int, int>();

            int occurenceCounter = 1;

            //This counter assumes that combinedCards is sorted.
            for (int card_index = 1; card_index < combinedCards.Count; ++card_index)
            {
                if (combinedCards[card_index].getRank() != combinedCards[card_index - 1].getRank())
                {
                    if (occurenceCounter > 1)
                    {
                        Card_entity card = combinedCards[card_index - 1];
                        occourencesOfRank.Add(card.getRank(), occurenceCounter);
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
                occourencesOfRank.Add(card.getRank(), occurenceCounter);
                occurenceCounter = 1;
            }
            return occourencesOfRank;
        }

        private int checkSameRank(List<Card_entity> combinedCards)
        {
            Dictionary<int, int> occourencesOfRank = countOccurencesOfRank(combinedCards);
            int pair_val = 0;
            int three_of_a_kind_val = 0;          

            foreach (KeyValuePair<int, int> rank in occourencesOfRank)
            {
                if (rank.Value == 4) // Value = number_of_cards_with_same_rank
                {
                    return calculateFourOfAKindValue(combinedCards, rank.Key);
                }
                else if (rank.Value == 3 && three_of_a_kind_val != 0)
                {
                    three_of_a_kind_val = rank.Key;
                    if (pair_val != 0) //FULL HOUSE
                        return calculateFullHouseValue(three_of_a_kind_val, pair_val);
                }
                else if (rank.Value == 2)
                {
                    if (three_of_a_kind_val != 0) //FULL HOUSE
                        return calculateFullHouseValue(three_of_a_kind_val, rank.Key);
                    else if (pair_val != 0) // We already have a pair => TWO PAIRS
                        return calculateTwoPairValuee(combinedCards, pair_val, rank.Key);
                    else
                        pair_val = rank.Key;
                }
            }

            if (three_of_a_kind_val != 0)
                return calculateThreeOfAKindValue(combinedCards, three_of_a_kind_val);
            else if (pair_val != 0)
                return calculatePairValue(combinedCards, pair_val);
            else
                return calculateHighHandValue(combinedCards);
        }

        private int calculateFullHouseValue(int three_of_a_kind_val, int pair_val)
        {
            return FULLHOUSE + three_of_a_kind_val * 100 + pair_val;
        }

        private int calculateFourOfAKindValue(List<Card_entity> combinedCards, int four_of_a_kind_val)
        {
            // Get the highest card that is not part of the four of a kind.
            int highestKicker = combinedCards[0].getRank() == four_of_a_kind_val ? combinedCards[4].getRank() : combinedCards[0].getRank();
            return FOUROFAKIND + four_of_a_kind_val * 100 + highestKicker;
        }

        private int calculateThreeOfAKindValue(List<Card_entity> combinedCards, int three_of_a_kind_val)
        {
            int highestKicker = combinedCards[0].getRank() == three_of_a_kind_val ? combinedCards[3].getRank() : combinedCards[0].getRank();
            int secondHighestKicker = combinedCards[1].getRank() == three_of_a_kind_val ? combinedCards[4].getRank() : combinedCards[1].getRank();
            return THREEOFAKIND + three_of_a_kind_val * 100 + highestKicker * 10 + secondHighestKicker;
        }

        private int calculateTwoPairValuee(List<Card_entity> combinedCards, int pair_val, int pair2_val)
        {
            int highestKicker;

            // Get the two pairs + highest kicker
            if (combinedCards[0].getRank() != pair_val)
                highestKicker = combinedCards[0].getRank();
            else if (combinedCards[2].getRank() != pair2_val)
                highestKicker = combinedCards[2].getRank();
            else
                highestKicker = combinedCards[4].getRank();

            return TWOPAIR + pair_val * 1000 + pair2_val * 100 + highestKicker;
        }

        private int calculatePairValue(List<Card_entity> combinedCards, int pair_val)
        {
            int highestKicker = combinedCards[0].getRank() == pair_val ? combinedCards[2].getRank() : combinedCards[0].getRank();
            int secondHighestKicker = combinedCards[1].getRank() == pair_val ? combinedCards[3].getRank() : combinedCards[1].getRank();
            int thirdHighestKicker = combinedCards[2].getRank() == pair_val ? combinedCards[4].getRank() : combinedCards[2].getRank();
            return PAIR + pair_val * 1000 + highestKicker * 100 + secondHighestKicker * 10 + thirdHighestKicker;
        }   

        private int calculateHighHandValue(List<Card_entity> combinedCards)
        {
            return combinedCards[0].getRank() + combinedCards[1].getRank() + combinedCards[2].getRank() + combinedCards[3].getRank() + combinedCards[4].getRank();
        }

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

