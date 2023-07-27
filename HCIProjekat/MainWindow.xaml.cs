using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace HCIProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BindingList<RMPlayer> players { get; set; }
        public MainWindow()
        {
            players = new BindingList<RMPlayer>();
            players.Add(new RMPlayer(13, "Andriy Lunin", "GoalKeeper", new DateTime(1999, 2, 11), @"C:\Users\hp\source\repos\HCIProjekat\HCIProjekat\Images\13.jpg", "13.rtf"));
            players.Add(new RMPlayer(1, "Thibaut Courtois", "GoalKeeper", new DateTime(1992, 5, 11), @"C:\Users\hp\source\repos\HCIProjekat\HCIProjekat\Images\1.jpg", "1.rtf"));
            players.Add(new RMPlayer(2, "Daniel Carvajal Ramos", "Defender", new DateTime(1992, 1, 11), @"C:\Users\hp\source\repos\HCIProjekat\HCIProjekat\Images\2.jpg", "2.rtf"));

            DataContext = this;
            InitializeComponent();


        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            RMPlayer player = (RMPlayer)dataGridPlayers.CurrentItem;

            File.Delete(player.BiographyFile);

            File.Delete($"{player.Number}.rtf");
            players.Remove(player);
        }
        private void change_Click(object sender, RoutedEventArgs e)
        {
            int i = dataGridPlayers.SelectedIndex;
            DodajIgraca window = new DodajIgraca(i, 'c');
            window.ShowDialog();
        }

        private void read_Click(object sender, RoutedEventArgs e)
        {
            int i = dataGridPlayers.SelectedIndex;

            DodajIgraca window = new DodajIgraca(i, 'r');
            window.ShowDialog();
        }
        private void new_Click(object sender, RoutedEventArgs e)
        {
            DodajIgraca window = new DodajIgraca(-1, 'n');
            window.ShowDialog();
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
