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

namespace Poker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Table table;

        private int MyData;

        public MainWindow()
        {
            InitializeComponent();
            this.table = new Table();
        }

        private void fold_button_Click(object sender, RoutedEventArgs e)
        {
            table.playerFold();

            hidePlayerButtons();
        }

        private void raise_button_Click(object sender, RoutedEventArgs e)
        {
            table.playerRaise();

            hidePlayerButtons();
        }

        private void call_button_Click(object sender, RoutedEventArgs e)
        {
            table.playerCall();

            hidePlayerButtons();
        }

        //Hide buttons until next time its the players turn
        private void hidePlayerButtons()
        {

        }

        private void showPlayerButtons()
        {

        }
    }
}
