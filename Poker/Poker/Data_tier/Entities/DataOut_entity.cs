using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Data_tier.Entities
{
    class DataOut_entity
    {
        //Community cards

        //player cards, player active, player stakes
        private List<List<string>> playerCards = new List<List<string>>();
        private List<bool> playerActive = new List<bool>();
        private List<int> playerStakes = new List<int>();
        private List<int> playerChips = new List<int>();

        //Intressant ifrån table 
        private int pot;
        private List<List<string>> communityCards = new List<List<string>>();

        public DataOut_entity()
        {

        }
    }
}
