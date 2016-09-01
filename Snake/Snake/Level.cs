using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;

namespace Snake
{
    [Serializable]
    public class Level
    {
        public CellsInfo MapCellsInfo;

        /// <summary>
        /// Количество очков для прохождения уровня
        /// </summary>
        public int FinishingScore;

        public int MapWidth;
        public int MapHeight;

        public Coord MapSize;

        public Direction Direction;

        public List<Coord> PlayerInitCoords = new List<Coord>();

        public List<Coord> ObstaclesCoords = new List<Coord>();

        public Audio BackgroundMusic;

        public int Speed;

        public Level(CellsInfo cellsInfo, int finishingScore, Direction direction, Audio backgroundMusic, int speed)
        {
            MapCellsInfo = cellsInfo;
            FinishingScore = finishingScore;
            Speed = speed;

            foreach (var cell in cellsInfo.cells)
            {
                switch (cell.CellType)
                {
                    case CellType.Player:
                        PlayerInitCoords.Add(new Coord(cell.X, cell.Y));
                        break;
                    case CellType.Brick:
                        ObstaclesCoords.Add(new Coord(cell.X, cell.Y));
                        break;
                }
            }

            MapWidth = cellsInfo.width;
            MapHeight = cellsInfo.height;

            MapSize = new Coord(MapWidth,MapHeight);

            Direction = direction;

            BackgroundMusic = backgroundMusic;
        }

        public static Level GetLevelFromFile(string path)
        {
            var bf = new BinaryFormatter();
            Level level = null;
            using (Stream s = File.OpenRead(path))
            {
                try
                {
                    level = bf.Deserialize(s) as Level;

                }
                catch
                {
                    throw new FileFormatException("Ошибка при чтении файла.");
                    return null;
                }
            }
            return level;
        }
    }

    public enum CellType
    {
        Empty,
        Brick,
        Player
    }

    [Serializable]
    public class Cell
    {
        public CellType CellType;

        public int X;
        public int Y;

        public Cell(CellType cellType, int x, int y)
        {
            this.CellType = cellType;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X};{Y};{CellType}";
        }
    }

    [Serializable]
    public class CellsInfo
    {
        public List<Cell> cells;

        public int width;
        public int height;

        public CellsInfo(List<Cell> cells, int width, int height)
        {
            this.width = width;
            this.height = height;

            if (cells.Count<(width*height))
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (!cells.Exists(cell => cell.X == j && cell.Y == i))
                        {
                            int indexOfPrevious =
                                cells.FindIndex(cell => cell.X == j - 1 || (cell.X == width - 1 && cell.Y == i - 1));
                            cells.Insert(indexOfPrevious,new Cell(CellType.Empty, j,i));
                        }
                    }
                }
            }

            this.cells = cells;
        }
    }
}
