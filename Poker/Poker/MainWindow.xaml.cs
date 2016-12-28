using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;
using Poker.Data_tier;
using Poker.Data_tier.Entities;
using System.Collections.Generic;

namespace Poker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameLogic gameLogic;
        private Data_tier.Table_entity table_entity;

        public MainWindow()
        {
            InitializeComponent();
            initGame();
            updateGraphics();
        }

        private void initGame()
        {
            List<Player_entity> players = new List<Player_entity>();
            players.Add(new Player_entity());
            players.Add(new Player_entity());
            players.Add(new Player_entity());
            players.Add(new Player_entity());
            players.Add(new Player_entity());

            List<Card_entity> communityCards = new List<Card_entity>();

            List<Data_tier.Card_entity> deck_of_cards = new List<Data_tier.Card_entity>();
            var suits = Enum.GetValues(typeof(Data_tier.Card_entity.Suit));

            foreach (Data_tier.Card_entity.Suit suit in suits)
            {
                for (int i = 2; i < 15; i++)
                {
                    deck_of_cards.Add(new Data_tier.Card_entity(suit, i));
                }
            }

            Deck deck = new Deck(deck_of_cards, new Deck_entity());

            table_entity = new Table_entity(players, communityCards, deck);
            gameLogic = new GameLogic(table_entity, new Logic_tier.TexasHoldemRules(), new Data());
        }

        private void highlightCurrentPlayerYellow()
        {
            // TODO: Use current player index instead of expensive method calls
            Data_tier.Player_entity currentPlayer = table_entity.CurrentPlayer;

            //Reset colors
            player1.Fill = new SolidColorBrush(Colors.White);
            player2.Fill = new SolidColorBrush(Colors.White);
            player3.Fill = new SolidColorBrush(Colors.White);
            player4.Fill = new SolidColorBrush(Colors.White);
            player5.Fill = new SolidColorBrush(Colors.White);

            if (currentPlayer == table_entity.getPlayer1())
                player1.Fill = new SolidColorBrush(Colors.Yellow);
            else if (currentPlayer == table_entity.getPlayer2())
                player2.Fill = new SolidColorBrush(Colors.Yellow);
            else if (currentPlayer == table_entity.getPlayer3())
                player3.Fill = new SolidColorBrush(Colors.Yellow);
            else if (currentPlayer == table_entity.getPlayer4())
                player4.Fill = new SolidColorBrush(Colors.Yellow);
            else if (currentPlayer == table_entity.getPlayer5())
                player5.Fill = new SolidColorBrush(Colors.Yellow);
        }

        private void setCommunityCards()
        {
            cm_card1_suit.Text = table_entity.getCommunityCards()[0].getSuit().ToString();
            cm_card1_rank.Text = table_entity.getCommunityCards()[0].getRank().ToString();

            cm_card2_suit.Text = table_entity.getCommunityCards()[1].getSuit().ToString();
            cm_card2_rank.Text = table_entity.getCommunityCards()[1].getRank().ToString();

            cm_card3_suit.Text = table_entity.getCommunityCards()[2].getSuit().ToString();
            cm_card3_rank.Text = table_entity.getCommunityCards()[2].getRank().ToString();

            cm_card4_suit.Text = table_entity.getCommunityCards()[3].getSuit().ToString();
            cm_card4_rank.Text = table_entity.getCommunityCards()[3].getRank().ToString();

            cm_card5_suit.Text = table_entity.getCommunityCards()[4].getSuit().ToString();
            cm_card5_rank.Text = table_entity.getCommunityCards()[4].getRank().ToString();
        }

        private void hideCommunityCards()
        {
            cm_card1_suit.Visibility = Visibility.Hidden;
            cm_card1_rank.Visibility = Visibility.Hidden;
            cm_card1_bg.Visibility = Visibility.Hidden;

            cm_card2_suit.Visibility = Visibility.Hidden;
            cm_card2_rank.Visibility = Visibility.Hidden;
            cm_card2_bg.Visibility = Visibility.Hidden;

            cm_card3_suit.Visibility = Visibility.Hidden;
            cm_card3_rank.Visibility = Visibility.Hidden;
            cm_card3_bg.Visibility = Visibility.Hidden;

            cm_card4_suit.Visibility = Visibility.Hidden;
            cm_card4_rank.Visibility = Visibility.Hidden;
            cm_card4_bg.Visibility = Visibility.Hidden;

            cm_card5_suit.Visibility = Visibility.Hidden;
            cm_card5_rank.Visibility = Visibility.Hidden;
            cm_card5_bg.Visibility = Visibility.Hidden;
        }

        private void updateCommunityCards()
        {
            Console.WriteLine("Round counter:" + table_entity.RoundCounter);
            if (table_entity.RoundCounter == 0)
            {
                hideCommunityCards();
            }

            else if (table_entity.RoundCounter == 1)
            {
                cm_card1_suit.Visibility = Visibility.Visible;
                cm_card1_rank.Visibility = Visibility.Visible;
                cm_card1_bg.Visibility = Visibility.Visible;
                cm_card2_suit.Visibility = Visibility.Visible;
                cm_card2_rank.Visibility = Visibility.Visible;
                cm_card2_bg.Visibility = Visibility.Visible;
                cm_card3_suit.Visibility = Visibility.Visible;
                cm_card3_rank.Visibility = Visibility.Visible;
                cm_card3_bg.Visibility = Visibility.Visible;
                cm_card4_suit.Visibility = Visibility.Hidden;
                cm_card4_rank.Visibility = Visibility.Hidden;
                cm_card4_bg.Visibility = Visibility.Hidden;
                cm_card5_suit.Visibility = Visibility.Hidden;
                cm_card5_rank.Visibility = Visibility.Hidden;
                cm_card5_bg.Visibility = Visibility.Hidden;
            }
            else if (table_entity.RoundCounter == 2)
            {
                cm_card1_suit.Visibility = Visibility.Visible;
                cm_card1_rank.Visibility = Visibility.Visible;
                cm_card1_bg.Visibility = Visibility.Visible;
                cm_card2_suit.Visibility = Visibility.Visible;
                cm_card2_rank.Visibility = Visibility.Visible;
                cm_card2_bg.Visibility = Visibility.Visible;
                cm_card3_suit.Visibility = Visibility.Visible;
                cm_card3_rank.Visibility = Visibility.Visible;
                cm_card3_bg.Visibility = Visibility.Visible;
                cm_card4_suit.Visibility = Visibility.Visible;
                cm_card4_rank.Visibility = Visibility.Visible;
                cm_card4_bg.Visibility = Visibility.Visible;
                cm_card5_suit.Visibility = Visibility.Hidden;
                cm_card5_rank.Visibility = Visibility.Hidden;
                cm_card5_bg.Visibility = Visibility.Hidden;
            }
            else if (table_entity.RoundCounter == 3)
            {
                cm_card1_suit.Visibility = Visibility.Visible;
                cm_card1_rank.Visibility = Visibility.Visible;
                cm_card1_bg.Visibility = Visibility.Visible;
                cm_card2_suit.Visibility = Visibility.Visible;
                cm_card2_rank.Visibility = Visibility.Visible;
                cm_card2_bg.Visibility = Visibility.Visible;
                cm_card3_suit.Visibility = Visibility.Visible;
                cm_card3_rank.Visibility = Visibility.Visible;
                cm_card3_bg.Visibility = Visibility.Visible;
                cm_card4_suit.Visibility = Visibility.Visible;
                cm_card4_rank.Visibility = Visibility.Visible;
                cm_card4_bg.Visibility = Visibility.Visible;
                cm_card5_suit.Visibility = Visibility.Visible;
                cm_card5_rank.Visibility = Visibility.Visible;
                cm_card5_bg.Visibility = Visibility.Visible;
            }
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayerMove_entity dataIn = new PlayerMove_entity(0, PlayerMove_entity.action.fold);
                gameLogic.playerAction(dataIn);
                updateGraphics();                
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} Exception caught.", error);
            }
        }

        private void call_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayerMove_entity dataIn = new PlayerMove_entity(0, PlayerMove_entity.action.call);
                gameLogic.playerAction(dataIn);
                updateGraphics();
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} Exception caught.", error);
            }
        }

        private void raise_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int bet = calcSliderValue() + table_entity.CurrentPlayer.getStakes();
                PlayerMove_entity dataIn = new PlayerMove_entity(bet, PlayerMove_entity.action.raise);
                gameLogic.playerAction(dataIn);

                displayStakes();
                displayChips();
                setSliderValue();
                highlightCurrentPlayerYellow();
                updateCheckCallButton();
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} Exception caught.", error);
            }
        }

        private void updateGraphics()
        {
            Visibility player1_visibility = table_entity.getPlayer1().Active ? Visibility.Visible : Visibility.Hidden;
            player1_card1_rank.Visibility = player1_visibility;
            player1_card1_suit.Visibility = player1_visibility;
            player1_card1_bg.Visibility = player1_visibility;
            player1_card2_rank.Visibility = player1_visibility;
            player1_card2_suit.Visibility = player1_visibility;
            player1_card2_bg.Visibility = player1_visibility;

            Visibility player2_visibility = table_entity.getPlayer2().Active ? Visibility.Visible : Visibility.Hidden;
            player2_card1_rank.Visibility = player2_visibility;
            player2_card1_suit.Visibility = player2_visibility;
            player2_card1_bg.Visibility = player2_visibility;
            player2_card2_rank.Visibility = player2_visibility;
            player2_card2_suit.Visibility = player2_visibility;
            player2_card2_bg.Visibility = player2_visibility;

            Visibility player3_visibility = table_entity.getPlayer3().Active ? Visibility.Visible : Visibility.Hidden;
            player3_card1_rank.Visibility = player3_visibility;
            player3_card1_suit.Visibility = player3_visibility;
            player3_card1_bg.Visibility = player3_visibility;
            player3_card2_rank.Visibility = player3_visibility;
            player3_card2_suit.Visibility = player3_visibility;
            player3_card2_bg.Visibility = player3_visibility;

            Visibility player4_visibility = table_entity.getPlayer4().Active ? Visibility.Visible : Visibility.Hidden;
            player4_card1_rank.Visibility = player4_visibility;
            player4_card1_suit.Visibility = player4_visibility;
            player4_card1_bg.Visibility = player4_visibility;
            player4_card2_rank.Visibility = player4_visibility;
            player4_card2_suit.Visibility = player4_visibility;
            player4_card2_bg.Visibility = player4_visibility;

            Visibility player5_visibility = table_entity.getPlayer5().Active ? Visibility.Visible : Visibility.Hidden;
            player5_card1_rank.Visibility = player5_visibility;
            player5_card1_suit.Visibility = player5_visibility;
            player5_card1_bg.Visibility = player5_visibility;
            player5_card2_rank.Visibility = player5_visibility;
            player5_card2_suit.Visibility = player5_visibility;
            player5_card2_bg.Visibility = player5_visibility;

            if (table_entity.RoundCounter == 0)
            {
                infoText.Text = gameLogic.getWiningMessage();
                infoText.Visibility = Visibility.Visible;
            } else
            {
                infoText.Visibility = Visibility.Hidden;
            }

            displayStakes();
            displayChips();
            highlightCurrentPlayerYellow();
            updateCheckCallButton();
            setPlayerCards();
            setCommunityCards();
            updateCommunityCards();
            pot.Text = table_entity.getPot().ToString();
        }


        private void updateCheckCallButton()
        {
            if (table_entity.LastRaise == table_entity.CurrentPlayer.getStakes())
            {
                setCallButtonToCheck();
            }
            else
            {
                setCallButtonToCall();
            }
        }

        private void setCallButtonToCall()
        {
            call_button.Content = "Call";
        }

        private void setCallButtonToCheck()
        {
            call_button.Content = "Check"; 
        }

        private void setSliderValue()
        {
            chipsValue.Text = (table_entity.MinRaise - table_entity.CurrentPlayer.getStakes()).ToString();
            slider.Value = 0;
        }

        private int calcSliderValue()
        {
            Data_tier.Player_entity currPlayer = table_entity.CurrentPlayer;
            double chips = currPlayer.getChips();
            double calcChips = ((slider.Value * 0.1) * chips); // Slider has bool value in range 0.0-10.0, hence * 0.1, then we multiply this with chips.
            int roundUp = ((int)Math.Round(calcChips / 10.0)) * 10; // Rounds it up to nearest 10.

            if (roundUp < table_entity.MinRaise - table_entity.CurrentPlayer.getStakes())
            {
                roundUp = table_entity.MinRaise - table_entity.CurrentPlayer.getStakes();
            }
            return roundUp;
        }

        private void slider_mouse_Leave(object sender, RoutedEventArgs e)
        {
            try
            {
                chipsValue.Text = calcSliderValue().ToString(); // Slider has bool value in range 0.0-10.0
            }
            catch (Exception error)
            {
                Console.WriteLine("{0} Exception caught.", error);
            }
        }

        private void displayStakes()
        {
            player1_stakes.Text = table_entity.getPlayer1().getStakes().ToString();
            player2_stakes.Text = table_entity.getPlayer2().getStakes().ToString();
            player3_stakes.Text = table_entity.getPlayer3().getStakes().ToString();
            player4_stakes.Text = table_entity.getPlayer4().getStakes().ToString();
            player5_stakes.Text = table_entity.getPlayer5().getStakes().ToString();
        }

        private void setPlayerCards()
        {
            player1_card1_rank.Text = table_entity.getPlayer1().getCards()[0].getRank().ToString();
            player1_card1_suit.Text = table_entity.getPlayer1().getCards()[0].getSuit().ToString();
            player1_card2_rank.Text = table_entity.getPlayer1().getCards()[1].getRank().ToString();
            player1_card2_suit.Text = table_entity.getPlayer1().getCards()[1].getSuit().ToString();

            player2_card1_rank.Text = table_entity.getPlayer2().getCards()[0].getRank().ToString();
            player2_card1_suit.Text = table_entity.getPlayer2().getCards()[0].getSuit().ToString();
            player2_card2_rank.Text = table_entity.getPlayer2().getCards()[1].getRank().ToString();
            player2_card2_suit.Text = table_entity.getPlayer2().getCards()[1].getSuit().ToString();

            player3_card1_rank.Text = table_entity.getPlayer3().getCards()[0].getRank().ToString();
            player3_card1_suit.Text = table_entity.getPlayer3().getCards()[0].getSuit().ToString();
            player3_card2_rank.Text = table_entity.getPlayer3().getCards()[1].getRank().ToString();
            player3_card2_suit.Text = table_entity.getPlayer3().getCards()[1].getSuit().ToString();

            player4_card1_rank.Text = table_entity.getPlayer4().getCards()[0].getRank().ToString();
            player4_card1_suit.Text = table_entity.getPlayer4().getCards()[0].getSuit().ToString();
            player4_card2_rank.Text = table_entity.getPlayer4().getCards()[1].getRank().ToString();
            player4_card2_suit.Text = table_entity.getPlayer4().getCards()[1].getSuit().ToString();

            player5_card1_rank.Text = table_entity.getPlayer5().getCards()[0].getRank().ToString();
            player5_card1_suit.Text = table_entity.getPlayer5().getCards()[0].getSuit().ToString();
            player5_card2_rank.Text = table_entity.getPlayer5().getCards()[1].getRank().ToString();
            player5_card2_suit.Text = table_entity.getPlayer5().getCards()[1].getSuit().ToString();
        }

        private void displayChips()
        {
            player1_chips.Text = table_entity.getPlayer1().getChips().ToString();
            player2_chips.Text = table_entity.getPlayer2().getChips().ToString();
            player3_chips.Text = table_entity.getPlayer3().getChips().ToString();
            player4_chips.Text = table_entity.getPlayer4().getChips().ToString();
            player5_chips.Text = table_entity.getPlayer5().getChips().ToString();            
        }

        private void mouseEnter(object sender, MouseEventArgs e)
        {
                Rectangle card = sender as Rectangle;
            if (card != null)
            {
                Panel.SetZIndex(card, 0);
            }
        }

        private void mouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle card = sender as Rectangle;
            if (card != null)
            {
                Panel.SetZIndex(card, 1);
            }
        }
    }
}
