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

        private int MyData;

        public MainWindow()
        {
            InitializeComponent();
            this.table = new Logic_tier.Table();
            displayPlayerChips();
            displayPlayerCards();
            displayStakes();
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            // IMPLEMENT THIS!!!
            // hideCards(getCurrentPlayer()); 

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
            player1_stakes.Text = table.getPlayer1().getStakes().ToString();
            player2_stakes.Text = table.getPlayer2().getStakes().ToString();
            player3_stakes.Text = table.getPlayer3().getStakes().ToString();
            player4_stakes.Text = table.getPlayer4().getStakes().ToString();
            player5_stakes.Text = table.getPlayer5().getStakes().ToString();
        }

        private void displayPlayerCards()
        {
            player1_card1_rank.Text = table.getPlayer1().getCards()[0].getRank().ToString();
            player1_card1_suit.Text = table.getPlayer1().getCards()[0].getSuit().ToString();
            player1_card2_rank.Text = table.getPlayer1().getCards()[1].getRank().ToString();
            player1_card2_suit.Text = table.getPlayer1().getCards()[1].getSuit().ToString();

            player2_card1_rank.Text = table.getPlayer2().getCards()[0].getRank().ToString();
            player2_card1_suit.Text = table.getPlayer2().getCards()[0].getSuit().ToString();
            player2_card2_rank.Text = table.getPlayer2().getCards()[1].getRank().ToString();
            player2_card2_suit.Text = table.getPlayer2().getCards()[1].getSuit().ToString();

            player3_card1_rank.Text = table.getPlayer3().getCards()[0].getRank().ToString();
            player3_card1_suit.Text = table.getPlayer3().getCards()[0].getSuit().ToString();
            player3_card2_rank.Text = table.getPlayer3().getCards()[1].getRank().ToString();
            player3_card2_suit.Text = table.getPlayer3().getCards()[1].getSuit().ToString();

            player4_card1_rank.Text = table.getPlayer4().getCards()[0].getRank().ToString();
            player4_card1_suit.Text = table.getPlayer4().getCards()[0].getSuit().ToString();
            player4_card2_rank.Text = table.getPlayer4().getCards()[1].getRank().ToString();
            player4_card2_suit.Text = table.getPlayer4().getCards()[1].getSuit().ToString();

            player5_card1_rank.Text = table.getPlayer5().getCards()[0].getRank().ToString();
            player5_card1_suit.Text = table.getPlayer5().getCards()[0].getSuit().ToString();
            player5_card2_rank.Text = table.getPlayer5().getCards()[1].getRank().ToString();
            player5_card2_suit.Text = table.getPlayer5().getCards()[1].getSuit().ToString();
        }

        private void displayPlayerChips()
        {
            player1_chips.Text = table.getPlayer1().getChips().ToString();
            player2_chips.Text = table.getPlayer2().getChips().ToString();
            player3_chips.Text = table.getPlayer3().getChips().ToString();
            player4_chips.Text = table.getPlayer4().getChips().ToString();
            player5_chips.Text = table.getPlayer4().getChips().ToString();
        }
    }
}
