using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic_tier
{
    class TexasHoldemRules
    {        
        private const int STRAIGHTFLUSH = 800000;
        private const int FOUROFAKIND = 700000;
        private const int FULLHOUSE = 600000;
        private const int FLUSH = 500000;
        private const int STRAIGHT = 400000;
        private const int THREEOFAKIND = 300000;
        private const int TWOPAIR = 200000;
        private const int PAIR = 100000;

        public TexasHoldemRules()
        {      

        }


        // Rank each hand from 1-10 from best to worst, also rank the strenght of the hand,
        // say two players have a pair, which one is the strongest.
        public Player_entity findWinner(Table_entity table, List<Player_entity> players)
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
