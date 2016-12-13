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

        /*
        public void loadData()
        {
            XDocument data = XDocument.Load("data.xml");
            var players = from p in data.Descendants("Table_entity").Descendants("players")
            select new
            {
                //load
            }
            foreach (var p in players)
        } 
        */       
    }
}
