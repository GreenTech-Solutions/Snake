using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using Snake;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

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
            var mainWindow = this.Owner as MainWindow;
            var data = mainWindow.data;

            if (data.IsInitialized)
            {
                tMapWidth.Text = data.MapWidth.ToString();
                tMapHeight.Text = data.MapHeight.ToString();
                tFinishingScore.Text = data.FinishingScore.ToString();
                tSpeed.Text = data.Speed.ToString();
                cbDirection.SelectedIndex = (int)data.Direction;
                lChoosenFile.Content = data.BackgroundMusic.Name;
                _backgroundMusic = data.BackgroundMusic;
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
                textBox.GotFocus += (o, args) => { textBox.SelectAll(); };
            }

            iPlayerButtonImage.Source = CreateSourceFromBitmap(SnakeLevelDesigner.Resources.Play);
            iLoading.Source = CreateSourceFromBitmap(SnakeLevelDesigner.Resources.Loading);
            pbLoading.Visibility = Visibility.Collapsed;
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
                iLoading.Visibility = Visibility.Collapsed;
                pbLoading.IsIndeterminate = true;
                pbLoading.Visibility = Visibility.Visible;

                MessageBox.Show("Animating");
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
                pbLoading.IsIndeterminate = false;
                pbLoading.Visibility = Visibility.Collapsed;
                iLoading.Visibility = Visibility.Visible;
                if (IsPlaying)
                {
                    OnPlayStop_Click(sender,e);
                }
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


            var mainWindow = this.Owner as MainWindow;
            var data = mainWindow.data;
            data.SetProperties(MapSize,finishingScore,speed,direction,_backgroundMusic);
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool IsPlaying = false;

        private SoundPlayer player = new SoundPlayer();

        private Stream PlayingSong;

        private void OnPlayStop_Click(object sender, RoutedEventArgs e)
        {
            if (!AudioLoaded)
            {
                return;
            }
            if (IsPlaying)
            {
                player.Stop();
                iPlayerButtonImage.Source = CreateSourceFromBitmap(SnakeLevelDesigner.Resources.Play);
            }
            else
            {
                player.Stream = _backgroundMusic.File;
                player.Play();
                iPlayerButtonImage.Source = CreateSourceFromBitmap(SnakeLevelDesigner.Resources.Stop);
            }

            IsPlaying = !IsPlaying;
        }

        public static BitmapSource CreateSourceFromBitmap(Bitmap bmp)
        {
            System.Drawing.Bitmap br = bmp;
            return Imaging.CreateBitmapSourceFromHBitmap(
                       br.GetHbitmap(),
                       IntPtr.Zero,
                       Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
        }

        private void CreatingLevel_OnClosing(object sender, CancelEventArgs e)
        {
            player.Stop();
        }
    }

    class RoundProgressPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (values?.Contains(DependencyProperty.UnsetValue) != false)
                return DependencyProperty.UnsetValue;

            var v = (double)values[0]; // значение слайдера
            var min = (double)values[1]; // минимальное значение
            var max = (double)values[2]; // максимальное

            var ratio = (v - min) / (max - min); // какую долю окружности закрашивать
            var isFull = ratio >= 1; // для случая полной окружности нужна особая обработка
            var angleRadians = 2 * Math.PI * ratio;
            var angleDegrees = 360 * ratio;

            // внешний радиус примем за 1, растянем в XAML'е.
            var outerR = 1;
            // как параметр передадим долю радиуса, которую занимает внутренняя часть
            var innerR =
                  System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture) * outerR;
            // вспомогательные штуки: вектор направления вверх
            var vector1 = new Vector(0, -1);
            // ... и на конечную точку дуги
            var vector2 = new Vector(Math.Sin(angleRadians), -Math.Cos(angleRadians));
            var center = new Point();

            var geo = new StreamGeometry();
            geo.FillRule = FillRule.EvenOdd;

            using (var ctx = geo.Open())
            {
                System.Windows.Size outerSize = new System.Windows.Size(outerR, outerR),
                     innerSize = new System.Windows.Size(innerR, innerR);

                if (!isFull)
                {
                    Point p1 = center + vector1 * outerR, p2 = center + vector2 * outerR,
                          p3 = center + vector2 * innerR, p4 = center + vector1 * innerR;

                    ctx.BeginFigure(p1, isFilled: true, isClosed: true);
                    ctx.ArcTo(p2, outerSize, angleDegrees, isLargeArc: angleDegrees > 180,
                        sweepDirection: SweepDirection.Clockwise, isStroked: true,
                        isSmoothJoin: false);
                    ctx.LineTo(p3, isStroked: true, isSmoothJoin: false);
                    ctx.ArcTo(p4, innerSize, -angleDegrees, isLargeArc: angleDegrees > 180,
                        sweepDirection: SweepDirection.Counterclockwise, isStroked: true,
                        isSmoothJoin: false);

                    Point diag1 = new Point(-outerR, -outerR),
                          diag2 = new Point(outerR, outerR);
                    ctx.BeginFigure(diag1, isFilled: false, isClosed: false);
                    ctx.LineTo(diag2, isStroked: false, isSmoothJoin: false);
                }
                else
                {
                    Point p1 = center + vector1 * outerR, p2 = center - vector1 * outerR,
                          p3 = center + vector1 * innerR, p4 = center - vector1 * innerR;

                    ctx.BeginFigure(p1, isFilled: true, isClosed: true);
                    ctx.ArcTo(p2, outerSize, 180, isLargeArc: false,
                        sweepDirection: SweepDirection.Clockwise, isStroked: true,
                        isSmoothJoin: false);
                    ctx.ArcTo(p1, outerSize, 180, isLargeArc: false,
                        sweepDirection: SweepDirection.Clockwise, isStroked: true,
                        isSmoothJoin: false);
                    ctx.BeginFigure(p3, isFilled: true, isClosed: true);
                    ctx.ArcTo(p4, innerSize, 180, isLargeArc: false,
                        sweepDirection: SweepDirection.Clockwise, isStroked: true,
                        isSmoothJoin: false);
                    ctx.ArcTo(p3, innerSize, 180, isLargeArc: false,
                        sweepDirection: SweepDirection.Clockwise, isStroked: true,
                        isSmoothJoin: false);
                }
            }

            geo.Freeze();
            return geo;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
                                    CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
