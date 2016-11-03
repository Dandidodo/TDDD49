using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker;

namespace PokerTest
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void CreateCard()
        {
            Card card = new Card(Card.Suit.Club, 2);

            Assert.AreEqual(Card.Suit.Club, card.getSuit());
        }
    }
}
