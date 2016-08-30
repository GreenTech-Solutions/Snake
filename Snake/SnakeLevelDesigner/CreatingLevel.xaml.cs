using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Snake;

namespace SnakeLevelDesigner
{
    /// <summary>
    /// Логика взаимодействия для CreatingLevel.xaml
    /// </summary>
    public partial class CreatingLevel : Window
    {
        public CreatingLevel()
        {
            InitializeComponent();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void CreatingLevel_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Data.IsInitialized)
            {
                tMapWidth.Text = Data.MapWidth.ToString();
                tMapHeight.Text = Data.MapHeight.ToString();
                tFinishingScore.Text = Data.FinishingScore.ToString();
                tSpeed.Text = Data.Speed.ToString();
                cbDirection.SelectedIndex = (int)Data.Direction;
                lChoosenFile.Content = Data.BackgroundMusic.Name;
                _backgroundMusic = Data.BackgroundMusic;
                CanCreate = true;
                AudioLoaded = true;
            }
            else
            {
                tMapWidth.Text = "10";
                tMapHeight.Text = "10";
                tFinishingScore.Text = "100";
                tSpeed.Text = "1";
                cbDirection.SelectedIndex = 0;
            }

            var textBoxes = FindVisualChildren<TextBox>(this);
            foreach (var textBox in textBoxes)
            {
                CheckTextbox(textBox);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            CheckTextbox(sender as TextBox);
        }

        void CheckTextbox(TextBox textbox)
        {
            var text = textbox.Text;

            Regex r = new Regex("^[1-9][0-9]*$");
            if (text=="")
            {
                textbox.BorderBrush = Brushes.Red;
                textbox.ToolTip = "Поле не должно быть пустым.";
                CanCreate = false;
                return;
            }
            else if (!r.IsMatch(text))
            {
                textbox.BorderBrush = Brushes.Red;
                textbox.ToolTip = "Вводить можно только числа.";
                CanCreate = false;
                return;
            }
            else
            {
                textbox.BorderBrush = Brushes.Green;
            }

            if (textbox.Name == "tSpeed")
            {
                var value = Convert.ToInt32(textbox.Text);
                if (value > 10)
                {
                    textbox.ToolTip = "Значение должно быть в пределах 1-10";
                    textbox.BorderBrush = Brushes.Red;
                    CanCreate = false;
                    return;
                }
            }
            textbox.ToolTip = null;
            CanCreate = true;
        }


        private void LoadMusic_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav"
            };

            if (ofd.ShowDialog() == true)
            {

                using (var file = File.OpenRead(ofd.FileName))
                {
                    var musicBytes = new byte[file.Length];
                    file.Read(musicBytes, 0, musicBytes.Length);

                    string fileName = ofd.SafeFileName;
                    fileName = fileName.Split('.').First();
                    _backgroundMusic = new Audio(fileName, musicBytes);
                }


                lChoosenFile.Content = _backgroundMusic.Name;
                lChoosenFile.ToolTip = ofd.FileName;

                AudioLoaded = true;
            }
        }

        private Audio _backgroundMusic;

        private bool CanCreate = false;

        private bool AudioLoaded = false;

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!CanCreate)
            {
                MessageBox.Show("Неправильно заполнена информация.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (!AudioLoaded)
            {
                MessageBox.Show("Для уровня должна быть загружена фоновая музыка.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var MapSize = new Coord(Convert.ToInt32(tMapWidth.Text),Convert.ToInt32(tMapHeight.Text));
            var speed = Convert.ToInt32(tSpeed.Text);
            var finishingScore = Convert.ToInt32(tFinishingScore.Text);
            var direction = (Direction) cbDirection.SelectedIndex;

            Data.SetProperties(MapSize,finishingScore,speed,direction,_backgroundMusic);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
