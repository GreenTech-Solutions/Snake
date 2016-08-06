﻿using System;
using System.Collections.Generic;
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
        }

        private int width, height;

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            CreateMap();
            bSave.IsEnabled = true;
        }

        private void CreateMap()
        {
            width = Convert.ToInt32(tWidth.Text);
            height = Convert.ToInt32(tHeight.Text);

            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = width;
            Map.Columns = height;

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

        private void CreateMap(Cells cells = null)
        {
            if (Equals(cells, null))
            {
                CreateMap();
                return;
            }

            width = cells.width;
            height = cells.height;

            Map.Children.RemoveRange(0, Map.Children.Count);

            Map.Rows = width;
            Map.Columns = height;

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
                        cell.cellType = CellType.Empty;
                    }
                    else if (rObstacle.IsChecked == true)
                    {
                        cell.cellType = CellType.Brick;
                    }
                    else if (rPlayer.IsChecked == true)
                    {
                        cell.cellType = CellType.Player;
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
            switch (cell.cellType)
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
                string fileName = sfd.FileName;

                List<Cell> listCells = new List<Cell>();

                foreach (var child in Map.Children)
                {
                    var border = child as Border;
                    var label = border.Child as Label;
                    listCells.Add(label.Tag as Cell);
                }

                var cells = new Cells(listCells,width,height);

                var bf = new BinaryFormatter();
                using (Stream fs = new FileStream(fileName,FileMode.Create,FileAccess.Write,FileShare.None))
                {
                    bf.Serialize(fs,cells);
                }

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
            Cells cells;

            var bf = new BinaryFormatter();
            using (Stream fs = File.OpenRead(path))
            {
                cells = bf.Deserialize(fs) as Cells;
            }

            CreateMap(cells);
        }
    }

    public enum CellType
    {
        Empty,
        Brick,
        Player
    }

    [Serializable]
    class Cell
    {
        public CellType cellType;

        public int X;
        public int Y;

        public Cell(CellType cellType, int x, int y)
        {
            this.cellType = cellType;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X};{Y};{cellType}";
        }
    }

    [Serializable]
    class Cells
    {
        public List<Cell> cells;

        public int width;
        public int height;

        public Cells(List<Cell> cells, int width, int height)
        {
            this.cells = cells;
            this.width = width;
            this.height = height;
        }
    }
}