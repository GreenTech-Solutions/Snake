using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                OpenFile(args[1]);
            }
            // Создание привязки
            CommandBinding bind = new CommandBinding(ApplicationCommands.New);
            // Присоединение обработчика событий
            bind.Executed += CreateLevel_OnClick;
            // Регистрация привязки
            this.CommandBindings.Add(bind);
            bind = new CommandBinding(ApplicationCommands.Open);
            bind.Executed += Open_OnClick;
            CommandBindings.Add(bind);

            bind = new CommandBinding(ApplicationCommands.Save);
            bind.Executed += Save_OnClick;
            CommandBindings.Add(bind);

            bind = new CommandBinding(ApplicationCommands.ContextMenu);
            bind.Executed += ChangeSettings_OnClick;
            CommandBindings.Add(bind);
        }

        private int width, height;

        private bool CanSave = false;

        public Data data = new Data();

        #region Buttons and commands

        private void CreateLevel_OnClick(object sender, RoutedEventArgs e)
        {
            var settings = new CreatingLevel();
            settings.Owner = this;


            if (settings.ShowDialog() == true)
            {
                CreateMap();
                CanSave = true;
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CanSave)
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Level files (*.lvl)|*.lvl";
            sfd.FileName = "Test Level";
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

                var cellsInfo = new CellsInfo(listCells, width, height);

                var direction = data.Direction;

                var audio = data.BackgroundMusic;

                var speed = data.Speed;

                var level = new Level(cellsInfo, data.FinishingScore, direction, audio, speed);

                var bf = new BinaryFormatter();
                using (Stream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    bf.Serialize(fs, level);
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

            CanSave = true;
        }

        private void ChangeSettings_OnClick(object sender, RoutedEventArgs e)
        {
            var settings = new CreatingLevel();

            Data oldData = this.data;
            settings.Owner = this;
            settings.bCreate.Content = "Change";
            settings.ShowDialog();

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

            List<Cell> oldCellsListCutted = new List<Cell>();

            if (data.MapWidth < oldData.MapWidth || data.MapHeight < oldData.MapHeight)
            {
                foreach (var MapChild in Map.Children)
                {
                    var border = MapChild as Border;
                    var label = border.Child as Label;
                    var cell = label.Tag as Cell;
                    if (cell.Y < data.MapWidth && cell.X < data.MapHeight)
                    {
                        oldCellsListCutted.Add(cell);
                    }
                }
            }
            else
            {
                foreach (var MapChild in Map.Children)
                {
                    var border = MapChild as Border;
                    var label = border.Child as Label;
                    var cell = label.Tag as Cell;
                    oldCellsListCutted.Add(cell);
                }
            }

            CellsInfo oldCellsInfo = new CellsInfo(oldCellsListCutted, data.MapWidth, data.MapHeight);
            CreateMap(oldCellsInfo);

            //CreateMap();
            bSave.IsEnabled = true;
        }

        #endregion

        #region Level painting

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
        #endregion

        #region Controller
        private void CreateMap()
        {
            width = data.MapWidth;
            height = data.MapHeight;

            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = height;
            Map.Columns = width;

            for (int i = 0; i < Map.Rows; i++)
            {
                for (int j = 0; j < Map.Columns; j++)
                {
                    var label = new Label();
                    label.Tag = new Cell(CellType.Empty, j, i);
                    label.Content = label.Tag;
                    ChangeCellColor(label);

                    var border = new Border();
                    border.BorderThickness = new Thickness(2);
                    border.BorderBrush = Brushes.Gray;
                    border.Child = label;
                    Map.Children.Add(border);
                }
            }

            Map.MouseMove += MapOnMouseMove;
            Map.MouseDown += MapOnMouseDown;
        }

        private void CreateMap(CellsInfo cellsInfo)
        {
            if (Equals(cellsInfo, null))
            {
                CreateMap();
                return;
            }

            width = cellsInfo.width;
            height = cellsInfo.height;

            data.MapWidth = width;
            data.MapHeight = height;


            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = height;
            Map.Columns = width;


            for (int i = 0; i < Map.Rows; i++)
            {
                for (int j = 0; j < Map.Columns; j++)
                {
                    var label = new Label();
                    var cells = cellsInfo.cells;
                    label.Tag = cells.Exists(cell => cell.X == j && cell.Y == i) ? cells.Find(cell => cell.X == j && cell.Y == i) : new Cell(CellType.Empty, j, i);
                    label.Content = (label.Tag as Cell).CellType.ToString();
                    label.ToolTip = label.Tag;
                    ChangeCellColor(label);

                    var border = new Border
                    {
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.Gray,
                        Child = label
                    };
                    Map.Children.Add(border);
                }
            }

            Map.MouseMove += MapOnMouseMove;
            Map.MouseDown += MapOnMouseDown;
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

            data.FinishingScore = level.FinishingScore;
            data.Direction = level.Direction;

            data.BackgroundMusic = level.BackgroundMusic;

            data.Speed = level.Speed;
        }
        #endregion
    }
}
