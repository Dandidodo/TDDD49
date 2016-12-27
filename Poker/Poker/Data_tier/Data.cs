using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Poker.Data_tier
{
    class Data
    {
        public Data()
        {

        }

        public void saveData(Data_tier.Table_entity table_entity, GameLogic gameLogic)
        {
            XDocument data = new XDocument(
                        new XElement("Table_entity",
                            new XElement("players"),
                            new XElement("gameLogic"),
                            new XElement("communityCards"),
                            new XElement("deck"),
                            new XElement("pot", table_entity.getPot())
                        )
                    );
            var gameLogic_node = data.Element("Table_entity").Element("gameLogic");
            gameLogic_node.Add(
                        new XElement("lastRaise", table_entity.LastRaise),
                        new XElement("minRaise", table_entity.MinRaise),
                        new XElement("indexBigBlind", table_entity.IndexBigBlind),
                        new XElement("indexSmallBlind", table_entity.IndexSmallBlind),
                        new XElement("roundCounter", table_entity.RoundCounter),
                        new XElement("currentPlayerIndex", table_entity.getPlayers().IndexOf(table_entity.CurrentPlayer))
                    );

            var communityCards = data.Element("Table_entity").Element("communityCards");
            foreach (Data_tier.Card_entity communityCard in table_entity.getCommunityCards())
            {
                communityCards.Add(
                    new XElement("communityCard",
                        new XElement("suit", communityCard.getSuit()),
                        new XElement("rank", communityCard.getRank())
                    )
                );
            }
            var players = data.Element("Table_entity").Element("players");
            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
            {
                players.Add(
                    new XElement("player",
                        new XElement("cards",
                            new XElement("card",
                                new XElement("suit", player.getCards()[0].getSuit()),
                                new XElement("rank", player.getCards()[0].getRank())
                                ),
                            new XElement("card",
                                new XElement("suit", player.getCards()[1].getSuit()),
                                new XElement("rank", player.getCards()[1].getRank())
                                )
                        ),
                        new XElement("chips", player.getChips()),
                        new XElement("stakes", player.getStakes()),
                        new XElement("active", player.Active),
                        new XElement("actedThisRound", player.ActedThisRound)
                    )
                );
            }

            var deck = data.Element("Table_entity").Element("deck");
            foreach (Data_tier.Card_entity card in table_entity.getDeck().Deck_entity.getCards())
            {
                deck.Add(
                    new XElement("card",
                        new XElement("suit", card.getSuit()),
                        new XElement("rank", card.getRank())
                    )
                );
            }
            data.Save("data.xml");
            // Lägg till exception handling, filrättigheter

        }


        public void loadData(Data_tier.Table_entity table_entity, GameLogic game_logic)
        {
            XDocument data = XDocument.Load("data.xml");

            // Load players
            var players = from p in data.Descendants("player")
                          select new
                          {
                              Cards = p.Descendants("card"),
                              Chips = p.Element("chips"),
                              Stakes = p.Element("stakes"),
                              Active = p.Element("active"),
                              ActedThisRound = p.Element("actedThisRound")
                          };
            foreach (var p in players.Select((value, i) => new { i, value }))
            {
                foreach (var card in p.value.Cards.Select((value2, i2) => new { i2, value2 }))
                {
                    // Copy rank
                    int rank = Int32.Parse(card.value2.Element("rank").Value);
                    table_entity.getPlayers()[p.i].getCards()[card.i2].setRank(rank);

                    // Copy suit
                    Data_tier.Card_entity.Suit suit = (Data_tier.Card_entity.Suit)Enum.Parse(typeof(Data_tier.Card_entity.Suit), card.value2.Element("suit").Value);
                    table_entity.getPlayers()[p.i].getCards()[card.i2].setSuit(suit);
                }


                int chips = Int32.Parse(p.value.Chips.Value);
                table_entity.getPlayers()[p.i].setChips(chips);

                int stakes = Int32.Parse(p.value.Stakes.Value);
                table_entity.getPlayers()[p.i].setStakes(stakes);
                
                bool active = Convert.ToBoolean(p.value.Active.Value);
                table_entity.getPlayers()[p.i].Active = active;

                bool actedThisRound = Convert.ToBoolean(p.value.ActedThisRound.Value);
                table_entity.getPlayers()[p.i].ActedThisRound = actedThisRound;
            }

            // Load game logic variables
            var gameLogic = from logic in data.Descendants("gameLogic")
                            select new
                            {
                                LastRaise = logic.Element("lastRaise"),
                                MinRaise = logic.Element("minRaise"),
                                IndexBigBlind = logic.Element("indexBigBlind"),
                                IndexSmallBlind = logic.Element("indexSmallBlind"),
                                RoundCounter = logic.Element("roundCounter"),
                                CurrentPlayerIndex = logic.Element("currentPlayerIndex"),
                            };
            foreach (var logic in gameLogic)
            {
                table_entity.LastRaise = Int32.Parse(logic.LastRaise.Value);
                table_entity.MinRaise = Int32.Parse(logic.MinRaise.Value);
                table_entity.IndexSmallBlind = Int32.Parse(logic.IndexSmallBlind.Value);
                table_entity.IndexBigBlind = Int32.Parse(logic.IndexBigBlind.Value);
                table_entity.RoundCounter = Int32.Parse(logic.RoundCounter.Value);
                table_entity.CurrentPlayer = table_entity.getPlayers()[Int32.Parse(logic.CurrentPlayerIndex.Value)];
            }

            // Load community cards
            var communityCards = from communityCard in data.Descendants("communityCard")
                                 select new
                                 {
                                     Suit = communityCard.Element("suit"),
                                     Rank = communityCard.Element("rank")
                                 };
            foreach (var card in communityCards.Select((value, i) => new {i, value}))
            {
                // Copy rank
                table_entity.getCommunityCards()[card.i].setRank(Int32.Parse(card.value.Rank.Value));

                // Copy suit
                Data_tier.Card_entity.Suit suit = (Data_tier.Card_entity.Suit)Enum.Parse(typeof(Data_tier.Card_entity.Suit), card.value.Suit.Value);
                table_entity.getCommunityCards()[card.i].setSuit(suit);
            }

            // Load deck
            var deck_cards = from deck in data.Descendants("deck").Descendants("card")
                                 select new
                                 {
                                     Suit = deck.Element("suit"),
                                     Rank = deck.Element("rank")
                                 };
            foreach (var card in deck_cards.Select((value, i) => new { i, value }))
            {
                // Copy rank
                table_entity.getDeck().Deck_entity.getCards()[card.i].setRank(Int32.Parse(card.value.Rank.Value));

                // Copy suit
                Data_tier.Card_entity.Suit suit = (Data_tier.Card_entity.Suit)Enum.Parse(typeof(Data_tier.Card_entity.Suit), card.value.Suit.Value);
                table_entity.getDeck().Deck_entity.getCards()[card.i].setSuit(suit);
            }

            // Load pot
            foreach (XElement element in data.Element("Table_entity").Descendants("pot"))
            {
                table_entity.setPot(Int32.Parse(element.Value));
            }
        }
    }
}
