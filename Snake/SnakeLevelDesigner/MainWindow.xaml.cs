using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }

        private int width, height;

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            width = Convert.ToInt32(tWidth.Text);
            height = Convert.ToInt32(tHeight.Text);

            Map.Children.RemoveRange(0,Map.Children.Count);

            Map.Rows = width;
            Map.Columns = height;

            for (int i = 0; i < Map.Rows; i++)
            {
                for (int j = 0; j < Map.Columns; j++)
                {
                    var cell = new Label();
                    cell.Tag = new Cell(CellType.Empty, j,i);
                    cell.Content = cell.Tag;

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

        private void MapOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                var map = sender as UniformGrid;

                CellClicked();                
            }
        }

        private void MapOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                CellClicked();
            }
        }

        private void CellClicked()
        {
            foreach (var child in Map.Children)
            {
                var border = child as Border;
                var mapButton = border.Child as Label;
                if (mapButton.IsMouseOver)
                {
                    var cell = mapButton.Tag as Cell;

                    switch (cell.cellType)
                    {
                        case CellType.Empty: 
                            cell.cellType = CellType.Brick;
                            mapButton.Background = Brushes.SaddleBrown;
                            break;
                        case CellType.Brick:
                            cell.cellType = CellType.Player;
                            mapButton.Background = Brushes.OrangeRed;
                            break;
                        case CellType.Player:
                            cell.cellType = CellType.Empty;
                            mapButton.Background = Brushes.WhiteSmoke;
                            break;
                    }
                }
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog(this) == true)
            {
                string fileName = sfd.FileName;

                string[] textToSave = new string[height];


            }
        }


    }

    public enum CellType
    {
        Empty,
        Brick,
        Player
    }

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
}
