using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic_tier
{
    class TexasHoldemRules
    {
        private const int ROYALFLUSH    = 900000;
        private const int STRAIGHTFLUSH = 800000;
        private const int FOUROFAKIND   = 700000;
        private const int FULLHOUSE     = 600000;
        private const int FLUSH         = 500000;
        private const int STRAIGHT      = 400000;
        private const int THREEOFAKIND  = 300000;
        private const int TWOPAIR       = 200000;
        private const int PAIR          = 100000;
        private string winingMessage = "";

        public TexasHoldemRules()
        {      

        }

        public string getWiningMessage()
        {
            return winingMessage;
        }


        // Rank each hand from 1-10 from best to worst, also rank the strenght of the hand,
        // say two players have a pair, which one is the strongest.
        public Data_tier.Player_entity findWinner(Data_tier.Table_entity table, List<Data_tier.Player_entity> players)
        {
            int bestHandVal = 0;
            int playerHandVal = 0;

            int playerIndex = 0;
            int bestPlayerIndex = 0;

            Data_tier.Player_entity bestPlayer = new Data_tier.Player_entity();
            foreach (Data_tier.Player_entity player in players)
            {
                ++playerIndex;

                if (player.Active)
                {
                    List<Data_tier.Card_entity> allCards = new List<Data_tier.Card_entity>(table.getCommunityCards());
                    allCards.Add(player.getCards()[0]);
                    allCards.Add(player.getCards()[1]);

                    List<Data_tier.Card_entity> sortedCards = allCards.OrderBy(c => c.getRank()).ToList();
                    sortedCards.Reverse();

                    playerHandVal = setPlayerHand(sortedCards);

                    // Keep track of best player
                    if (playerHandVal > bestHandVal)
                    {
                        bestHandVal = playerHandVal;
                        bestPlayer = player;
                        bestPlayerIndex = playerIndex;
                    }
                }
            }

            winingMessage = "Player " + bestPlayerIndex.ToString() + " wins with ";

            if (bestHandVal >= ROYALFLUSH)
                winingMessage += "Royal Flush";
            else if (bestHandVal >= STRAIGHTFLUSH)
                winingMessage += "Straight Flush";
            else if (bestHandVal >= FOUROFAKIND)
                winingMessage += "Four of a Kind";
            else if (bestHandVal >= FULLHOUSE)
                winingMessage += "Full House";
            else if (bestHandVal >= FLUSH)
                winingMessage += "Flush";
            else if (bestHandVal >= STRAIGHT)
                winingMessage += "Straight";
            else if (bestHandVal >= THREEOFAKIND)
                winingMessage += "Three of a Kind";
            else if (bestHandVal >= TWOPAIR)
                winingMessage += "Two Pair";
            else if (bestHandVal >= PAIR)
                winingMessage += "Pair";
            else if (bestHandVal >= 0)
                winingMessage += "Highest card";

            return bestPlayer;
        }

        // Will check for same rank, flush and straight. And use 
        // these together to find straight flush, royal flush etc.
        // straightValue and flushValue will be set to 0 if none could be found.
        private int setPlayerHand(List<Data_tier.Card_entity> sortedCards)
        {
            int playerHandVal = checkSameRank(sortedCards);
            int flushValue = checkFlush(sortedCards);
            int straightValue = checkStraight(sortedCards);

            bool flush = false;
            if (flushValue > 0)
            {
                flush = true;
                // Only set flushValue if its better, four of a kind could also be possible here and is better than flush
                if (flushValue > playerHandVal)
                    playerHandVal = flushValue;
            }

            if (straightValue > 0)
            {
                if (flush && (sortedCards[0].getRank() == 14))
                {
                    //Royal flush, recalculate handvalue
                    playerHandVal = ROYALFLUSH;
                }
                else if (flush)
                {
                    playerHandVal = STRAIGHTFLUSH + sortedCards[0].getRank();
                }
                else
                {
                    if (straightValue > playerHandVal)
                    {
                        playerHandVal = STRAIGHT + sortedCards[0].getRank();
                    }
                }

            }

            return playerHandVal;
        }

        private Dictionary<int, int> countOccurencesOfRank(List<Data_tier.Card_entity> combinedCards)
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
                        Data_tier.Card_entity card = combinedCards[card_index - 1];
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
                Data_tier.Card_entity card = combinedCards[combinedCards.Count - 1];
                occourencesOfRank.Add(card.getRank(), occurenceCounter);
                occurenceCounter = 1;
            }
            return occourencesOfRank;
        }

        // Finds cards with same rank, and sets appropriate value.
        private int checkSameRank(List<Data_tier.Card_entity> combinedCards)
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
                else if (rank.Value == 3)
                {
                    if (rank.Key > three_of_a_kind_val) //Overwrite if we find a bigger three of a kind value
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

        private int calculateFourOfAKindValue(List<Data_tier.Card_entity> combinedCards, int four_of_a_kind_val)
        {
            // Get the highest card that is not part of the four of a kind.
            int highestKicker = combinedCards[0].getRank() == four_of_a_kind_val ? combinedCards[4].getRank() : combinedCards[0].getRank();
            return FOUROFAKIND + four_of_a_kind_val * 100 + highestKicker;
        }

        private int calculateThreeOfAKindValue(List<Data_tier.Card_entity> combinedCards, int three_of_a_kind_val)
        {
            int highestKicker = combinedCards[0].getRank() == three_of_a_kind_val ? combinedCards[3].getRank() : combinedCards[0].getRank();
            int secondHighestKicker = combinedCards[1].getRank() == three_of_a_kind_val ? combinedCards[4].getRank() : combinedCards[1].getRank();
            return THREEOFAKIND + three_of_a_kind_val * 100 + highestKicker * 10 + secondHighestKicker;
        }

        private int calculateTwoPairValuee(List<Data_tier.Card_entity> combinedCards, int pair_val, int pair2_val)
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

        private int calculatePairValue(List<Data_tier.Card_entity> combinedCards, int pair_val)
        {
            int highestKicker = combinedCards[0].getRank() == pair_val ? combinedCards[2].getRank() : combinedCards[0].getRank();
            int secondHighestKicker = combinedCards[1].getRank() == pair_val ? combinedCards[3].getRank() : combinedCards[1].getRank();
            int thirdHighestKicker = combinedCards[2].getRank() == pair_val ? combinedCards[4].getRank() : combinedCards[2].getRank();
            return PAIR + pair_val * 1000 + highestKicker * 100 + secondHighestKicker * 10 + thirdHighestKicker;
        }

        private int calculateHighHandValue(List<Data_tier.Card_entity> combinedCards)
        {
            return combinedCards[0].getRank() + combinedCards[1].getRank() + combinedCards[2].getRank() + combinedCards[3].getRank() + combinedCards[4].getRank();
        }

        // Returns if any suit occures 5 times or more.
        private int checkFlush(List<Data_tier.Card_entity> combinedCards)
        {
            int heartCount = 0;
            int spadesCount = 0;
            int diamondCount = 0;
            int clubCount = 0;

            for (int i = 0; i < combinedCards.Count; i++)
            {
                Data_tier.Card_entity.Suit currentSuit = combinedCards[i].getSuit();

                if (currentSuit == Data_tier.Card_entity.Suit.Heart)
                    heartCount++;
                else if (currentSuit == Data_tier.Card_entity.Suit.Spade)
                    spadesCount++;
                else if (currentSuit == Data_tier.Card_entity.Suit.Diamond)
                    diamondCount++;
                else
                    clubCount++;
            }

            if ((heartCount >= 5) || (spadesCount >= 5) || (diamondCount >= 5) || (clubCount >= 5))
            {
                int val = FLUSH + combinedCards[0].getRank() * 1000 + combinedCards[1].getRank() * 100 + combinedCards[2].getRank() * 10 + combinedCards[3].getRank();
                return val;
            }
            else
                return 0;
        }

        private int checkStraight(List<Data_tier.Card_entity> combinedCards)
        {
            int previousRank = combinedCards[0].getRank();
            int currentStreak = 0; // The cards rank has to increase five times, but we dont know where the straight starts.

            for (int i = 1; i < combinedCards.Count; i++)
            {
                int currentRank = combinedCards[i].getRank();
                if ((previousRank - 1) == currentRank)
                {
                    currentStreak++;
                    if (currentStreak == 4)
                        return STRAIGHT + combinedCards[0].getRank(); //Here we have found it
                } else if(previousRank != currentRank) //When we have multiple cards with same rank, like 9,9,8,7,6,5..
                {
                    currentStreak = 0;
                }

                previousRank = currentRank;

            }
            return 0;
        }
    }
}
