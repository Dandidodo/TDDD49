using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Poker.Logic_tier
{
    class Data
    {
        public Data()
        {

        }

        public void saveData(Table_entity table_entity, GameLogic gameLogic)
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
                        new XElement("lastRaise", gameLogic.LastRaise),
                        new XElement("minRaise", gameLogic.MinRaise),
                        new XElement("indexBigBlind", gameLogic.IndexBigBlind),
                        new XElement("indexSmallBlind", gameLogic.IndexSmallBlind),
                        new XElement("roundCounter", gameLogic.RoundCounter),
                        new XElement("currentPlayerIndex", table_entity.getPlayers().IndexOf(gameLogic.getCurrentPlayer()))
                    );

            var communityCards = data.Element("Table_entity").Element("communityCards");
            foreach (Card_entity communityCard in table_entity.getCommunityCards())
            {
                communityCards.Add(
                    new XElement("communityCard",
                        new XElement("suit", communityCard.getSuit()),
                        new XElement("rank", communityCard.getRank())
                    )
                );
            }
            var players = data.Element("Table_entity").Element("players");
            foreach (Player_entity player in table_entity.getPlayers())
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
            foreach (Card_entity card in table_entity.getDeck().Deck_entity.getCards())
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

        
        public void loadData(Table_entity table_entity)
        {
            XDocument data = XDocument.Load("data.xml");
            var players = from p in data.Descendants("Table_entity").Descendants("players")
                          select new
                          {
                              Cards = p.Descendants("cards"),
                              Chips = p.Element("chips"),
                              Stakes = p.Element("stakes"),
                              Active = p.Element("active"),
                              ActedThisRound = p.Element("ActedThisRound")
                          };
            foreach (var p in players.Select((value, i) => new { i, value}))
            {
                /*
                foreach (var card in p.value.Cards.Select((value2, i2) => new {i2, value2}))
                {
                    // Remove the cards
                    table_entity.getPlayers()[p.i].removeCards();

                    // Copy rank
                    int rank = Int32.Parse(card.value2.Element("rank").Value);
                    table_entity.getPlayers()[p.i].getCards()[card.i2].setRank(rank);

                    // Copy suit
                    Card_entity.Suit suit = (Card_entity.Suit)Enum.Parse(typeof(Card_entity.Suit), card.value2.Element("suit").Value);
                    table_entity.getPlayers()[p.i].getCards()[card.i2].setSuit(suit);
                }
                */
                int chips = Int32.Parse(p.value.Chips.Value);
                table_entity.getPlayers()[p.i].setChips(chips);

                int stakes = Int32.Parse(p.value.Stakes.Value);
                table_entity.getPlayers()[p.i].setStakes(stakes);

                bool active = Convert.ToBoolean(p.value.Active.Value);
                table_entity.getPlayers()[p.i].Active = active;

                bool actedThisRound = Convert.ToBoolean(p.value.ActedThisRound.Value);
                table_entity.getPlayers()[p.i].Active = actedThisRound;
            }
        }             
    }
}
