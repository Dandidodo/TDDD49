using Poker.Logic_tier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;

namespace Poker
{
    class GameLogic
    {
        private const int PLAYERCARDS = 2;
        private const int COMMUNITYCARDS = 5;
        private const int STARTINGCHIPS = 1000;
        private const int BIGBLIND = 20;
        
        // Allt det här måste flyttas till table_entity
        private int lastRaise;
        private int minRaise; // minimum allowed raise
        private Table_entity table_entity;
        private int indexBigBlind;
        private int indexSmallBlind;
        private int roundCounter;
        private Player_entity currentPlayer;
        private TexasHoldemRules rules;
        private Data data;
        private string foldWinner = "";

        public GameLogic(Table_entity table_entity)
        {
            this.table_entity = table_entity;
            rules = new TexasHoldemRules();
            data = new Data();

            indexBigBlind = 1;
            indexSmallBlind = 0;
            roundCounter = 0;
                        
            giveStartingChips();
            newHand();

            // Spara all data här på något sätt (efter att allt från gameLogic förts över till data_entity)
            // Så att det kan laddas in om inladdningen av datan går fel halvvägs in
            
            try
            {
                data.loadData(table_entity, this);
            }
            // Filen saknas
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            // Ogiltig XML
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Ta bort I suppose
            catch (XamlParseException e)
            {
                Console.WriteLine(e.Message);
            }
            // Giltig XML men inte på väntat format (antar att man gör så här)
            catch (WrongFormat e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public class WrongFormat: Exception
{
    public WrongFormat()
    {
        // Nån smart check som kollar att det är rätt format bör vara här antar jag
    }

    public WrongFormat(string message)
        : base(message)
    {
    }

    public WrongFormat(string message, Exception inner)
        : base(message, inner)
    {
    }
}

        public int MinRaise
        {
            get { return minRaise; }
            set { minRaise = value; }
        }

        public int LastRaise
        {
            get { return lastRaise; }
            set { lastRaise = value; }
        }

        public int RoundCounter
        {
            get { return roundCounter; }
            set { roundCounter = value; }
        }

        public int IndexBigBlind
        {
            get
            {
                return indexBigBlind;
            }

            set
            {
                indexBigBlind = value;
            }
        }

        public int IndexSmallBlind
        {
            get
            {
                return indexSmallBlind;
            }

            set
            {
                indexSmallBlind = value;
            }
        }

        internal Player_entity CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        public string FoldWinner
        {
            get
            {
                return foldWinner;
            }

            set
            {
                foldWinner = value;
            }
        }

        //Winner wins by fold or best hand
        public string getWiningMessage()
        {
            if(foldWinner != "")
            {
                string tmpMessage = foldWinner;
                foldWinner = ""; //Reset once it has been used
                return tmpMessage;
            } else
            {
                return rules.getWiningMessage();
            }
        }

        //Loops over all players to find correct index of winner
        private void setFoldWinner(Player_entity currentPlayer)
        {
            int playerIndex = 0;
            int winner = 0;
            foreach (Player_entity p in table_entity.getPlayers())
            {
                ++playerIndex;
                if (p == currentPlayer)
                {
                    winner = playerIndex;
                }

            }
            foldWinner = "Player " + winner.ToString() + " wins";
        }

        public void newHand()
        {
            roundCounter = 0;

            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            table_entity.getDeck().initDeck();

            changeBlindIndexes();
            insertBlinds();

            lastRaise = BIGBLIND;
            minRaise = BIGBLIND * 2;

            currentPlayer = (indexBigBlind == table_entity.getPlayers().Count - 1) ? table_entity.getPlayers()[0] : table_entity.getPlayers()[indexBigBlind + 1];

            setCommunityCards();
            dealCards();
        }

        public void changeBlindIndexes()
        {
            indexBigBlind = (indexBigBlind == table_entity.getPlayers().Count - 1) ? indexBigBlind = 0 : ++indexBigBlind;
            indexSmallBlind = (indexSmallBlind == table_entity.getPlayers().Count - 1) ? indexSmallBlind = 0 : ++indexSmallBlind;
        }

        public void insertBlinds()
        {

            insertPlayerChips(table_entity.getPlayers()[indexBigBlind], BIGBLIND);
            insertPlayerChips(table_entity.getPlayers()[indexSmallBlind], BIGBLIND / 2);
        }

        public void giveStartingChips()
        {
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.setChips(STARTINGCHIPS);
            }
        }

        public void dealCards()
        {
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.removeCards();

                //Try to draw new card, deck could be empty
                try
                {
                    player.receiveCard(table_entity.getDeck().draw());
                    player.receiveCard(table_entity.getDeck().draw());
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
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
            foreach (Player_entity player in table_entity.getPlayers())
            {
                if (player.Active == true)
                    activePlayers++;
            }
            return activePlayers == 1;
        }

        public void setCommunityCards()
        {
            table_entity.removeCards();
            for (int i = 0; i < 5; i++)
            {
                //Could draw from empty deck.
                try
                {
                    table_entity.setCM(table_entity.getDeck().draw());
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
        public void playerFold()
        {
            currentPlayer.ActedThisRound = true;
            // Move the players stakes to the pot
            table_entity.setPot(table_entity.getPot() + currentPlayer.getStakes());
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
                setFoldWinner(currentPlayer);
                newHand();
            }

            try
            {
                data.saveData(table_entity, this);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public void giveWinnings(Player_entity player)
        {
            player.setChips(player.getChips() + table_entity.getPot() + player.getStakes());
            player.setStakes(0);
            table_entity.setPot(0);
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
                endRound();
                // If River
                if (roundCounter == 4)
                {
                    // Calculate who has the best hand
                    Player_entity player = rules.findWinner(table_entity, table_entity.getPlayers());
                    giveWinnings(player);
                    newHand();
                }
            }

            try
            {
                data.saveData(table_entity, this);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
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

            try
            {
                data.saveData(table_entity, this);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        // Find the next player, we have to check a special case if the current active player is in the end of players, then we have to
        // return the first active player found.
        public Player_entity getNextActivePlayer(Player_entity player)
        {
            Player_entity nextPlayer = new Player_entity();
            bool playerFound = false;
            bool firstActivePlayerFound = false;

            foreach (Player_entity p in table_entity.getPlayers())
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
            foreach (Player_entity player in table_entity.getPlayers())
            {
                player.ActedThisRound = false;
                table_entity.setPot(table_entity.getPot() + player.getStakes());
                player.setStakes(0);
            }
            List<Player_entity> players = table_entity.getPlayers();
            currentPlayer = indexSmallBlind != 0 ? getNextActivePlayer(players[indexSmallBlind - 1]) : getNextActivePlayer(players[players.Count() - 1]);
            lastRaise = 0;
            minRaise = BIGBLIND;
            roundCounter++;
        }

        
    }
}
