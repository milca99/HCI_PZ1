using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HCIProjekat
{
    /// <summary>
    /// Interaction logic for DodajIgraca.xaml
    /// </summary>
    public partial class DodajIgraca : Window
    {
        char flag = 'n';
        string path = @"C:\Users\hp\source\repos\HCIProjekat\HCIProjekat\Images";
        RMPlayer player;
        int i;
        public DodajIgraca(int i, char c)
        {
            flag = c;
            this.i = i;
            InitializeComponent();

            switch (flag)
            {
                case 'n':
                    {
                        break;
                    }
                case 'r':
                    {
                        save.Visibility = Visibility.Hidden;
                        button1.Visibility = Visibility.Hidden;

                        player = MainWindow.players[i];
                        textBoxNumber.Text = player.Number.ToString();
                        textBoxNumber.IsReadOnly = true;
                        textBoxName.Text = player.FirstAndLastName;
                        textBoxName.IsReadOnly = true;

                        comboBoxPosition.Visibility = Visibility.Hidden;
                        textBoxPosition.Text = player.Position;
                        textBoxPosition.Visibility = Visibility.Visible;
                        textBoxPosition.IsReadOnly = true;

                        datePickerOfBirth.Visibility = Visibility.Hidden;
                        textBoxDateOfBirth.Visibility = Visibility.Visible;
                        textBoxDateOfBirth.Text = player.DateOfBirth.ToString("dd.MM.yyyy");
                        textBoxDateOfBirth.IsReadOnly = true;

                        image.Source = new BitmapImage(new Uri(player.Image));

                        loadRtf(player.BiographyFile);
                        rtbEditor.IsReadOnly = true;
                        break;
                    }
                case 'c':
                    {
                        player = MainWindow.players[i];
                        textBoxNumber.Text = player.Number.ToString();
                        textBoxNumber.IsReadOnly = true;
                        textBoxName.Text = player.FirstAndLastName;
                        comboBoxPosition.SelectedValue = player.Position;
                        datePickerOfBirth.SelectedDate = player.DateOfBirth;
                        image.Source = new BitmapImage(new Uri(player.Image));
                        op1.FileName = player.Image;
                        loadRtf(player.BiographyFile);
                        break;
                    }

                default:
                    break;
            }

            init();
        }
        private void loadRtf(string name)
        {
            try
            {
                FileStream fileStream = new FileStream(name, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);

                fileStream.Close();
            }
            catch { }
            rtbEditor.FontFamily = new FontFamily("Sagoe UI");
            cmbFontFamily.SelectedItem = new FontFamily("Segoe UI");
            rtbEditor.FontSize = 20;
            cmbFontSize.Text = "20";
            rtbEditor.Foreground = Brushes.Black;
            cmbFontColor.SelectedItem = typeof(Colors).GetProperties()[7];

        }
        private void init()
        {
            comboBoxPosition.ItemsSource = new List<string>() { "Defender", "Back", "Left wing", "Right ving", "Goalkeeper" };
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontColor.ItemsSource = typeof(Colors).GetProperties();
            cmbFontSize.ItemsSource = new List<double>() {2, 4, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56, 58, 60, 62, 64, 66, 68, 70, 72, 74, 76, 78, 80};
            textBox.Text = "Word Count: 0";
            lblCursorPosition.Text = "Line: " + "1" + " Column: " + "1";
        }
        static OpenFileDialog op1 = new OpenFileDialog();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            op1.FileName = "";
            op1.InitialDirectory = path;
            op1.Title = "Select a picture";
            op1.Filter = "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op1.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(op1.FileName));
            }
        }
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            char[] d = new char[] { ' ', '\r', '\n' };
            string[] words = text.Split(d, StringSplitOptions.RemoveEmptyEntries);
            textBox.Text = "Words: " + words.Length.ToString();
            TextPointer TextPointer1 = rtbEditor.Selection.Start.GetLineStartPosition(0);
            TextPointer TextPointer2 = rtbEditor.Selection.Start;
            int column = TextPointer1.GetOffsetToPosition(TextPointer2) + 1;
            int l, linesNum;
            rtbEditor.Selection.Start.GetLineStartPosition(-99999999, out l);
            linesNum = -l + 1;
            lblCursorPosition.Text = "Row: " + linesNum.ToString() + " Column: " + column.ToString();

            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;

            temp = rtbEditor.Selection.GetPropertyValue(Inline.ForegroundProperty);
            Color c = Color.FromRgb((temp as SolidColorBrush).Color.R, (temp as SolidColorBrush).Color.G, (temp as SolidColorBrush).Color.B);
            cmbFontColor.SelectedItem = typeof(Colors).GetProperties().First(x => Color.Equals(x, FromColor(c))); ;

        }

        private void cmbFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontColor.SelectedItem != null)
            {

                var selectedItem = (PropertyInfo)cmbFontColor.SelectedItem;
                var color = (Color)selectedItem.GetValue(null, null);

                rtbEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, color.ToString());


            }
        }
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }
        private void cmbFontSize_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            }
            catch
            {
                cmbFontSize.Text = "b";
            }
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 'n')
            {
                if (validate())
                {
                    saveRTF($"{ textBoxNumber.Text}.rtf");
                    RMPlayer newPlayer = new RMPlayer(int.Parse(textBoxNumber.Text), textBoxName.Text, comboBoxPosition.SelectedValue.ToString(), (DateTime)datePickerOfBirth.SelectedDate, op1.FileName, $"{textBoxNumber.Text}.rtf");
                    MainWindow.players.Add(newPlayer);
                    this.Close();
                }

            }
            else if (flag == 'c')
            {
                if (validate() == true)
                {
                    saveRTF($"{ textBoxNumber.Text}.rtf");
                    player = new RMPlayer();
                    player.Number = int.Parse(textBoxNumber.Text);
                    player.FirstAndLastName = textBoxName.Text;
                    player.Position = comboBoxPosition.SelectedValue.ToString();
                    player.DateOfBirth = (DateTime)datePickerOfBirth.SelectedDate;
                    player.Image = (op1.FileName != "") ? op1.FileName : player.BiographyFile;
                    player.BiographyFile = $"{textBoxNumber.Text}.rtf";
                    MainWindow.players[i] = player;
                    this.Close();
                }
            }

        }
        PropertyInfo FromColor(Color c)
        {
            var type = typeof(Colors);
            var res = type.GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.GetValue(null, null).ToString() == c.ToString())
                .Select(p => p).FirstOrDefault();
            return res;
        }
        private void saveRTF(string name)
        {
            FileStream fileStream = new FileStream(name, FileMode.OpenOrCreate);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
        }
        private bool validate()
        {
            bool rez = true;

            if (textBoxNumber.Text.Trim().Equals(""))
            {
                labelErrorNumber.Content = "Please fill in the box!";
                textBoxNumber.BorderBrush = Brushes.Red;
                rez = false;
            }
            else
            {
                int number;
                if (int.TryParse(textBoxNumber.Text, out number))
                {
                    if (number <= 0)
                    {
                        labelErrorNumber.Content = "Number must be positive!";
                        textBoxNumber.BorderBrush = Brushes.Red;
                        rez = false;
                    }
                    else
                    {
                        if (flag != 'c' && MainWindow.players.ToList().Exists(x => x.Number.Equals(number)))
                        {
                            labelErrorNumber.Content = "Number alredy exists!";
                            textBoxNumber.BorderBrush = Brushes.Red;
                            rez = false;
                        }
                        else
                        {
                            labelErrorNumber.Content = "";
                            textBoxNumber.BorderBrush = Brushes.Black;
                        }
                    }
                }
                else
                {
                    labelErrorNumber.Content = "Please fill in the box with digits!";
                    textBoxNumber.BorderBrush = Brushes.Red;
                    rez = false;
                }

            }
            if (textBoxName.Text.Trim().Equals(""))
            {
                labelErrorName.Content = "Please fill in the box!";
                textBoxName.BorderBrush = Brushes.Red;
                rez = false;
            }
            else
            {
                labelErrorName.Content = "";
                textBoxName.BorderBrush = Brushes.Black;
            }
            if (comboBoxPosition.SelectedItem == null)
            {
                labelErrorPosition.Content = "Please choose option!";
                comboBoxPosition.BorderBrush = Brushes.Red;
                rez = false;
            }
            else
            {
                labelErrorPosition.Content = "";
                comboBoxPosition.BorderBrush = Brushes.Black;
            }

            if (String.IsNullOrEmpty(op1.FileName))
            {
                labelErrorPic1.Content = "Please choose image!";
                rez = false;

            }
            else
            {
                labelErrorPic1.Content = "";
            }

            if (datePickerOfBirth.SelectedDate < DateTime.Now - new TimeSpan(18 * 365, 0, 0, 0) )
            {
                labelErrorDateOfBirth.Content = "";
                datePickerOfBirth.BorderBrush = Brushes.Black;
            }
            else
            {
                labelErrorDateOfBirth.Content = "Please insert right date!";
                datePickerOfBirth.BorderBrush = Brushes.Red;
                rez = false;
            }
            return rez;
        }
        private void x_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
