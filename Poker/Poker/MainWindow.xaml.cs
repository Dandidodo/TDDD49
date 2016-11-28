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
            displayPlayerCards();
            displayStakes();
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            Player_entity currPlayer = table_entity.getRules().getCurrentPlayer();

            table_entity.getRules().playerFold();

            pot.Text = table_entity.getPot().ToString();
            displayStakes();
            displayChips();

            Visibility player1_visibility = table_entity.getPlayer1().active ? Visibility.Visible : Visibility.Hidden;
            player1_card1_rank.Visibility = player1_visibility;
            player1_card1_suit.Visibility = player1_visibility;
            player1_card1_bg.Visibility = player1_visibility;
            player1_card2_rank.Visibility = player1_visibility;
            player1_card2_suit.Visibility = player1_visibility;
            player1_card2_bg.Visibility = player1_visibility;

            Visibility player2_visibility = table_entity.getPlayer2().active ? Visibility.Visible : Visibility.Hidden;
            player2_card2_rank.Visibility = player2_visibility;
            player2_card2_suit.Visibility = player2_visibility;
            player2_card2_bg.Visibility = player2_visibility;
            player2_card2_rank.Visibility = player2_visibility;
            player2_card2_suit.Visibility = player2_visibility;
            player2_card2_bg.Visibility = player2_visibility;

            Visibility player3_visibility = table_entity.getPlayer3().active ? Visibility.Visible : Visibility.Hidden;
            player3_card1_rank.Visibility = player3_visibility;
            player3_card1_suit.Visibility = player3_visibility;
            player3_card1_bg.Visibility = player3_visibility;
            player3_card2_rank.Visibility = player3_visibility;
            player3_card2_suit.Visibility = player3_visibility;
            player3_card2_bg.Visibility = player3_visibility;

            Visibility player4_visibility = table_entity.getPlayer4().active ? Visibility.Visible : Visibility.Hidden;
            player4_card1_rank.Visibility = player4_visibility;
            player4_card1_suit.Visibility = player4_visibility;
            player4_card1_bg.Visibility = player4_visibility;
            player4_card2_rank.Visibility = player4_visibility;
            player4_card2_suit.Visibility = player4_visibility;
            player4_card2_bg.Visibility = player4_visibility;

            Visibility player5_visibility = table_entity.getPlayer5().active ? Visibility.Visible : Visibility.Hidden;
            player5_card1_rank.Visibility = player5_visibility;
            player5_card1_suit.Visibility = player5_visibility;
            player5_card1_bg.Visibility = player5_visibility;
            player5_card2_rank.Visibility = player5_visibility;
            player5_card2_suit.Visibility = player5_visibility;
            player5_card2_bg.Visibility = player5_visibility;



            //table.playerFold();
            //hidePlayerButtons();
        }

        private void raise_button_Click(object sender, RoutedEventArgs e)
        {
            //table.playerRaise();

            hidePlayerButtons();
        }

        private void call_button_Click(object sender, RoutedEventArgs e)
        {
            //table.playerCall();

            hidePlayerButtons();
        }

        //Hide buttons until next time its the players turn
        private void hidePlayerButtons()
        {

        }

        private void showPlayerButtons()
        {

        }

        private void displayStakes()
        {
            player1_stakes.Text = table_entity.getPlayer1().getStakes().ToString();
            player2_stakes.Text = table_entity.getPlayer2().getStakes().ToString();
            player3_stakes.Text = table_entity.getPlayer3().getStakes().ToString();
            player4_stakes.Text = table_entity.getPlayer4().getStakes().ToString();
            player5_stakes.Text = table_entity.getPlayer5().getStakes().ToString();
        }

        private void displayPlayerCards()
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
            player5_chips.Text = table_entity.getPlayer4().getChips().ToString();
        }
    }
}
