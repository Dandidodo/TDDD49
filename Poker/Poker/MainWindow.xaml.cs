using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Poker.Logic_tier;

namespace Poker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logic_tier.Table table;
        private Table_entity table_entity;

        private int MyData;

        public MainWindow()
        {
            InitializeComponent();
            table_entity = new Table_entity();
            table = new Logic_tier.Table(table_entity);
            displayChips();
            setPlayerCards();
            setCommunityCards();
            hideCommunityCards();
            displayStakes();
            setSliderValue();
            highlightCurrentPlayerYellow();
        }

        private void highlightCurrentPlayerYellow()
        {
            Player_entity currentPlayer = table_entity.getRules().getCurrentPlayer();

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
            cm_card1_suit.Text = table_entity.getCM1().getSuit().ToString();
            cm_card1_rank.Text = table_entity.getCM1().getRank().ToString();

            cm_card2_suit.Text = table_entity.getCM2().getSuit().ToString();
            cm_card2_rank.Text = table_entity.getCM2().getRank().ToString();

            cm_card3_suit.Text = table_entity.getCM3().getSuit().ToString();
            cm_card3_rank.Text = table_entity.getCM3().getRank().ToString();

            cm_card4_suit.Text = table_entity.getCM4().getSuit().ToString();
            cm_card4_rank.Text = table_entity.getCM4().getRank().ToString();

            cm_card5_suit.Text = table_entity.getCM5().getSuit().ToString();
            cm_card5_rank.Text = table_entity.getCM5().getRank().ToString();
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
            if (table_entity.getRules().RoundCounter == 0)
            {
                hideCommunityCards();
            }

            else if (table_entity.getRules().RoundCounter == 1)
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
            }
            else if (table_entity.getRules().RoundCounter == 2)
            {
                cm_card4_suit.Visibility = Visibility.Visible;
                cm_card4_rank.Visibility = Visibility.Visible;
                cm_card4_bg.Visibility = Visibility.Visible;
            }
            else if (table_entity.getRules().RoundCounter == 3)
            {
                cm_card5_suit.Visibility = Visibility.Visible;
                cm_card5_rank.Visibility = Visibility.Visible;
                cm_card5_bg.Visibility = Visibility.Visible;
            }
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            table_entity.getRules().playerFold();
            updateGraphics();
        }

        private void call_button_Click(object sender, RoutedEventArgs e)
        {
            table_entity.getRules().playerCall(); //playerCall(table_entity)

            /*
            if (gameLogic.send(new Click(table_entity)))
            {
                // gameLogic frågar reglerna
            }
            */
            updateGraphics();

            // exception här
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

            displayStakes();
            displayChips();
            highlightCurrentPlayerYellow();
            updateCheckCallButton();
            setPlayerCards();
            setCommunityCards();
            updateCommunityCards();
            pot.Text = table_entity.getPot().ToString();
        }


        private void raise_button_Click(object sender, RoutedEventArgs e)
        {
            table_entity.getRules().playerRaise(calcSliderValue() + table_entity.getRules().getCurrentPlayer().getStakes());

            displayStakes();
            displayChips();
            setSliderValue();
            highlightCurrentPlayerYellow();
            updateCheckCallButton();
        }

        private void updateCheckCallButton()
        {
            if (table_entity.getRules().LastRaise == table_entity.getRules().getCurrentPlayer().getStakes())
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
            chipsValue.Text = (table_entity.getRules().MinRaise - table_entity.getRules().getCurrentPlayer().getStakes()).ToString();
            slider.Value = 0;
        }

        private int calcSliderValue()
        {
            Player_entity currPlayer = table_entity.getRules().getCurrentPlayer();
            double chips = currPlayer.getChips();
            double calcChips = ((slider.Value * 0.1) * chips); // Slider has bool value in range 0.0-10.0, hence * 0.1, then we multiply this with chips.
            int roundUp = ((int)Math.Round(calcChips / 10.0)) * 10; // Rounds it up to nearest 10.

            if (roundUp < table_entity.getRules().MinRaise - table_entity.getRules().getCurrentPlayer().getStakes())
            {
                roundUp = table_entity.getRules().MinRaise - table_entity.getRules().getCurrentPlayer().getStakes();
            }
            return roundUp;
        }

        private void slider_mouse_Leave(object sender, RoutedEventArgs e)
        {
            chipsValue.Text = calcSliderValue().ToString(); // Slider has bool value in range 0.0-10.0
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
