using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Win32;
using Snake;

namespace SnakeLevelDesigner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            rObstacle.IsChecked = true;
            bSave.IsEnabled = false;

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                OpenFile(args[1]);
                bSave.IsEnabled = true;
            }
            cbDirection.SelectedIndex = 0;

            // Создание привязки
            CommandBinding bind = new CommandBinding(ApplicationCommands.New);
            // Присоединение обработчика событий
            bind.Executed += Create_OnClick;
            // Регистрация привязки
            this.CommandBindings.Add(bind);
            bind = new CommandBinding(ApplicationCommands.Open);
            bind.Executed += Open_OnClick;
            CommandBindings.Add(bind);

            bind = new CommandBinding(ApplicationCommands.Save);
            bind.Executed += Save_OnClick;
            CommandBindings.Add(bind);
        }

        private int width, height;

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var settings = new CreatingLevel();
            settings.ShowDialog();
            CreateMap();
            bSave.IsEnabled = true;
            this.IsEnabled = true;
        }

        private void CreateMap()
        {
            width = Convert.ToInt32(tWidth.Text);
            height = Convert.ToInt32(tHeight.Text);

            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = height;
            Map.Columns = width;

            for (int i = 0; i < Map.Rows; i++)
            {
                for (int j = 0; j < Map.Columns; j++)
                {
                    var cell = new Label();
                    cell.Tag = new Cell(CellType.Empty, j, i);
                    cell.Content = cell.Tag;
                    ChangeCellColor(cell);

                    var border = new Border();
                    border.BorderThickness = new Thickness(2);
                    border.BorderBrush = Brushes.Gray;
                    border.Child = cell;
                    Map.Children.Add(border);
                }
            }

            Map.MouseMove += MapOnMouseMove;
            Map.MouseDown += MapOnMouseDown;
        }

        private void CreateMap(CellsInfo cells = null)
        {
            if (Equals(cells, null))
            {
                CreateMap();
                return;
            }

            width = cells.width;
            height = cells.height;

            tWidth.Text = width.ToString();
            tHeight.Text = height.ToString();


            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = height;
            Map.Columns = width;

            foreach (var tag in cells.cells)
            {
                var cell = new Label {Tag = tag};
                cell.Content = cell.Tag;

                ChangeCellColor(cell);


                var border = new Border
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = Brushes.Gray,
                    Child = cell
                };
                Map.Children.Add(border);
            }

            Map.MouseMove += MapOnMouseMove;
            Map.MouseDown += MapOnMouseDown;
        }

        private void MapOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                CellClicked();                
            }
        }

        private void MapOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                CellClicked();
            }
            mouseEventArgs.Handled = true;

        }

        private void CellClicked()
        {
            foreach (var child in Map.Children)
            {
                var border = child as Border;
                var label = border.Child as Label;
                if (label.IsMouseOver)
                {
                    var cell = label.Tag as Cell;

                    if (rEmpty.IsChecked == true)
                    {
                        cell.CellType = CellType.Empty;
                    }
                    else if (rObstacle.IsChecked == true)
                    {
                        cell.CellType = CellType.Brick;
                    }
                    else if (rPlayer.IsChecked == true)
                    {
                        cell.CellType = CellType.Player;
                    }

                    ChangeCellColor(label);

                    label.Tag = cell;
                    label.Content = label.Tag.ToString();
                }
            }
        }

        private void ChangeCellColor(Label label)
        {
            var cell = label.Tag as Cell;
            switch (cell.CellType)
            {
                case CellType.Empty:
                    label.Background = Brushes.WhiteSmoke;
                    break;
                case CellType.Brick:
                    label.Background = Brushes.DarkSlateGray;
                    break;
                case CellType.Player:
                    label.Background = Brushes.YellowGreen;
                    break;
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Level files (*.lvl)|*.lvl";

            if (sfd.ShowDialog(this) == true)
            {
                var fileName = sfd.FileName;

                var listCells = new List<Cell>();

                foreach (var child in Map.Children)
                {
                    var border = child as Border;
                    var label = border.Child as Label;
                    listCells.Add(label.Tag as Cell);
                }

                var cellsInfo = new CellsInfo(listCells,width,height);

                var direction = (Direction)(cbDirection.SelectedIndex);

                var audio = new Audio(_backgroundMusic);
                audio.Name = lBackgroundMusic.Content.ToString();

                var speed = Convert.ToInt32(tSpeed.Text);

                var level = new Level(cellsInfo,Convert.ToInt32(tFinishingScore.Text), direction, audio,speed);

                var bf = new BinaryFormatter();
                using (Stream fs = new FileStream(fileName,FileMode.Create,FileAccess.Write,FileShare.None))
                {
                    bf.Serialize(fs,level);
                }

                MessageBox.Show("File was successfully saved!");

            }
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Level files (*.lvl)|*.lvl";
            if (ofd.ShowDialog(this) == true)
            {
                string fileName = ofd.FileName;

                OpenFile(fileName);
            }

            bSave.IsEnabled = true;
        }

        private void OpenFile(string path)
        {
            Level level;

            var bf = new BinaryFormatter();
            using (Stream fs = File.OpenRead(path))
            {
                level = bf.Deserialize(fs) as Level;
            }

            CreateMap(level.MapCellsInfo);

            tFinishingScore.Text = level.FinishingScore.ToString();
            cbDirection.SelectedIndex = (int) level.Direction;

            lBackgroundMusic.Content = level.BackgroundMusic;

            tSpeed.Text = level.Speed.ToString();
        }

        private byte[] _backgroundMusic;

        private void LoadBackgroundMusic_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav"
            };

            if (ofd.ShowDialog() == true)
            {

                using (var file = File.OpenRead(ofd.FileName))
                {
                    _backgroundMusic = new byte[file.Length];
                    file.Read(_backgroundMusic, 0, _backgroundMusic.Length);
                }

                lBackgroundMusic.Content = ofd.FileName;
            }

        }
    }
}
