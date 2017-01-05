using Poker.Logic_tier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using Poker.Data_tier.Entities;

namespace Poker
{
    class GameLogic
    {

        private Data_tier.Table_entity table_entity;
        private TexasHoldemRules rules;
        private Data_tier.Data data;

        public GameLogic(Data_tier.Table_entity table_entity, TexasHoldemRules rules, Data_tier.Data data)
        {
            this.table_entity = table_entity;
            this.rules = rules;
            this.data = data;
                        
            giveStartingChips();
            newHand();
          
        }

        public MoveResponse_entity loadPreviousGame()
        {
            MoveResponse_entity moveResponse = new MoveResponse_entity();
            moveResponse.MoveStatus = MoveResponse_entity.status.LOAD_SUCCESSFUL;
            try
            {
                data.loadData(table_entity, this);
            }
            // File missing
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                moveResponse.MoveStatus = MoveResponse_entity.status.FAILED_TO_LOAD_GAME;
            }
            // Invalid XML
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                moveResponse.MoveStatus = MoveResponse_entity.status.FAILED_TO_LOAD_GAME;
            }
            catch (XamlParseException e)
            {
                Console.WriteLine(e.Message);
                moveResponse.MoveStatus = MoveResponse_entity.status.FAILED_TO_LOAD_GAME;
            }
            // Giltig XML men inte på väntat format (antar att man gör så här)
            catch (WrongFormat e)
            {
                Console.WriteLine(e.Message);
                moveResponse.MoveStatus = MoveResponse_entity.status.FAILED_TO_LOAD_GAME;
            }

            return moveResponse;
        }

        //Winner wins by fold or best hand
        public string getWiningMessage()
        {
            if(table_entity.FoldWinner != "")
            {
                string tmpMessage = table_entity.FoldWinner;
                table_entity.FoldWinner = ""; //Reset once it has been used
                return tmpMessage;
            } else
            {
                return rules.getWiningMessage();
            }
        }

        private int getCurrentPlayerIndex()
        {
            int playerIndex = 0;
            Data_tier.Player_entity currentPlayer = table_entity.CurrentPlayer;
            foreach (Data_tier.Player_entity p in table_entity.getPlayers())
            {
                ++playerIndex;
                if (p == currentPlayer)
                {
                    break;
                }

            }

            return playerIndex;
        }

        //Loops over all players to find correct index of winner
        private void setFoldWinner(Data_tier.Player_entity currentPlayer)
        {
            int playerIndex = 0;
            int winner = 0;
            foreach (Data_tier.Player_entity p in table_entity.getPlayers())
            {
                ++playerIndex;
                if (p == currentPlayer)
                {
                    winner = playerIndex;
                }

            }
            table_entity.FoldWinner = "Player " + winner.ToString() + " wins";
        }

        public void newHand()
        {
            table_entity.getDeck().generateNewDeck(); //Generate new deck during each hand, otherwise the deck will become empty.

            table_entity.RoundCounter = 0;

            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
            {
                player.Active = true;
                player.ActedThisRound = false;
            }

            table_entity.getDeck();

            changeBlindIndexes();
            insertBlinds();

            table_entity.LastRaise = Data_tier.Table_entity.BIGBLIND;
            table_entity.MinRaise = Data_tier.Table_entity.BIGBLIND * 2;

            table_entity.CurrentPlayer = (table_entity.IndexBigBlind == table_entity.getPlayers().Count - 1) ? table_entity.getPlayers()[0] : table_entity.getPlayers()[table_entity.IndexBigBlind + 1];

            setCommunityCards();
            dealCards();
        }

        public void changeBlindIndexes()
        {
            int indexBigBlind = table_entity.IndexBigBlind;
            table_entity.IndexBigBlind = (indexBigBlind == table_entity.getPlayers().Count - 1) ? indexBigBlind = 0 : ++indexBigBlind;

            int indexSmallBlind = table_entity.IndexSmallBlind;
            table_entity.IndexSmallBlind = (indexSmallBlind == table_entity.getPlayers().Count - 1) ? indexSmallBlind = 0 : ++indexSmallBlind;
        }

        public void insertBlinds()
        {

            insertPlayerChips(table_entity.getPlayers()[table_entity.IndexBigBlind], Data_tier.Table_entity.BIGBLIND);
            insertPlayerChips(table_entity.getPlayers()[table_entity.IndexSmallBlind], Data_tier.Table_entity.BIGBLIND / 2);
        }

        public void giveStartingChips()
        {
            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
            {
                player.setChips(Data_tier.Table_entity.STARTINGCHIPS);
            }
        }

        public void dealCards()
        {
            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
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

        public void insertPlayerChips(Data_tier.Player_entity player, int chips)
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
            return (table_entity.CurrentPlayer.getStakes() == table_entity.LastRaise && table_entity.CurrentPlayer.ActedThisRound);
        }

        public bool handIsFinished()
        {
            int activePlayers = 0;
            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
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

        public MoveResponse_entity playerAction(Data_tier.Entities.PlayerMove_entity dataIn)
        {
            PlayerMove_entity.action playerAction = dataIn.PlayerAction;

            MoveResponse_entity response = new MoveResponse_entity();
            response.MoveStatus = MoveResponse_entity.status.SAVE_SUCCESSFUL; //Overwrite if failure occurs

            try
            {
                if (playerAction == PlayerMove_entity.action.fold)
                    playerFold();
                else if (playerAction == PlayerMove_entity.action.raise)
                    playerRaise(dataIn.Bet);
                else
                    playerCall();

                data.saveData(table_entity);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);

                response.MoveStatus = MoveResponse_entity.status.FAILED_TO_SAVE;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);

                response.MoveStatus = MoveResponse_entity.status.FAILED_TO_SAVE;
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);

                response.MoveStatus = MoveResponse_entity.status.FAILED_TO_SAVE;
            }
            catch (EmptyDeckException e)
            {
                Console.WriteLine(e.Message);

                response.MoveStatus = MoveResponse_entity.status.GAME_FAILURE;
            }
            finally
            {
            }

            return response;
        }
        
        private void playerFold()
        {
            table_entity.CurrentPlayer.ActedThisRound = true;
            // Move the players stakes to the pot
            table_entity.setPot(table_entity.getPot() + table_entity.CurrentPlayer.getStakes());
            table_entity.CurrentPlayer.setStakes(0);

            // Get next player
            Data_tier.Player_entity nextPlayer = getNextActivePlayer(table_entity.CurrentPlayer);

            // Remove player from active players
            table_entity.CurrentPlayer.Active = false;

            table_entity.CurrentPlayer = nextPlayer;

            if (roundIsFinished()) // Everyone has acted at least once and there is no raise
            {
                endRound(); // Move all stakes to the pot etc
            }
            if (handIsFinished()) // Check if the hand is finished (if there's only 1 active player left)
            {
                giveWinnings(table_entity.CurrentPlayer);
                setFoldWinner(table_entity.CurrentPlayer);
                newHand();
            }
            
        }

        public void giveWinnings(Data_tier.Player_entity player)
        {
            player.setChips(player.getChips() + table_entity.getPot() + player.getStakes());
            player.setStakes(0);
            table_entity.setPot(0);
        }

        private void playerCall()
        {
            table_entity.CurrentPlayer.ActedThisRound = true;
            // Insert chips
            insertPlayerChips(table_entity.CurrentPlayer, table_entity.LastRaise - table_entity.CurrentPlayer.getStakes());

            // Check if player is out of chips
            // NextPlayer
            // Get next player
            Data_tier.Player_entity nextPlayer = getNextActivePlayer(table_entity.CurrentPlayer);

            table_entity.CurrentPlayer = nextPlayer;

            if (roundIsFinished())
            {
                endRound();
                // If River
                if (table_entity.RoundCounter == 4)
                {
                    // Calculate who has the best hand
                    Data_tier.Player_entity player = rules.findWinner(table_entity, table_entity.getPlayers());
                    giveWinnings(player);
                    newHand();
                }
            }
        }

        private void playerRaise(int raise)
        {
            table_entity.CurrentPlayer.ActedThisRound = true;

            table_entity.MinRaise = raise + (raise - table_entity.LastRaise);
            table_entity.LastRaise = raise;

            // Insert chips
            insertPlayerChips(table_entity.CurrentPlayer, raise - table_entity.CurrentPlayer.getStakes());

            // TODO: Check if player is out of chips

            // NextPlayer
            Data_tier.Player_entity nextPlayer = getNextActivePlayer(table_entity.CurrentPlayer);

            table_entity.CurrentPlayer = nextPlayer;
        }
        
        // Find the next player, we have to check a special case if the current active player is in the end of players, then we have to
        // return the first active player found.
        public Data_tier.Player_entity getNextActivePlayer(Data_tier.Player_entity player)
        {
            Data_tier.Player_entity nextPlayer = new Data_tier.Player_entity();
            bool playerFound = false;
            bool firstActivePlayerFound = false;

            foreach (Data_tier.Player_entity p in table_entity.getPlayers())
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
            foreach (Data_tier.Player_entity player in table_entity.getPlayers())
            {
                player.ActedThisRound = false;
                table_entity.setPot(table_entity.getPot() + player.getStakes());
                player.setStakes(0);
            }
            List<Data_tier.Player_entity> players = table_entity.getPlayers();
            table_entity.CurrentPlayer = table_entity.IndexSmallBlind != 0 ? getNextActivePlayer(players[table_entity.IndexSmallBlind - 1]) : getNextActivePlayer(players[players.Count() - 1]);
            table_entity.LastRaise = 0;
            table_entity.MinRaise = Data_tier.Table_entity.BIGBLIND;
            table_entity.RoundCounter++;
        }

        
    }
}
