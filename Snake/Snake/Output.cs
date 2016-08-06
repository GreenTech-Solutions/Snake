using System;
using System.Collections.Generic;

namespace Snake
{
    /// <summary>
    /// Графический модуль
    /// </summary>
    class Output
    {
        /// <summary>
        /// Символ головы змеи
        /// </summary>
        private static char SnakeHead = (char)64;

        /// <summary>
        /// Символ элемента туловища змеи
        /// </summary>
        private static char SnakeBody = (char)42;

        /// <summary>
        /// Символ элемента карты
        /// </summary>
        private static char MapCell = (char)183;

        private static char Obstacle = (char) 140;

        /// <summary>
        /// Символ пищи
        /// </summary>
        private static char Food = (char)35;

        /// <summary>
        /// Задний фон карты
        /// </summary>
        private static ConsoleColor BackGround = ConsoleColor.DarkGreen;

        /// <summary>
        /// Очистка консоли
        /// </summary>
        static public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Отрисовывает карту указанного размера
        /// </summary>
        static public void DrawMap(Coord size, Cell[,] Map)
        {
            if (size == null) throw new ArgumentNullException(nameof(size));

            // Рисование игрового поля
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    Console.SetCursorPosition(j + Padding, i + PaddingTop);
                    if (Map[j, i].CellType == CellType.Empty)
                    {
                        Console.BackgroundColor = BackGround;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(MapCell);
                    }
                    else if (Map[j,i].CellType == CellType.Brick)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(Obstacle);
                    }
                }
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает игрока, с помощью указанных координат
        /// </summary>
        static public void DrawPlayer(LinkedList<Coord> playerCoords)
        {
            if (playerCoords == null) throw new ArgumentNullException(nameof(playerCoords));

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Red;

            // Рисование змеи
            var cell = playerCoords.Last;
            while (cell!=null)
            {
                Console.SetCursorPosition(cell.Value.X + Padding, cell.Value.Y + PaddingTop);
                Console.Write(cell == playerCoords.First ? SnakeHead : SnakeBody);
                cell = cell.Previous;
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Отображает на указанных координатах знак MapCell
        /// </summary>
        /// <param name="cell">Координаты</param>
        static public void RedrawMapcell(Coord cell)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(cell.X + Padding,cell.Y + PaddingTop);
            Console.Write(MapCell);

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает еду на указанных координатах
        /// </summary>
        static public void DrawFood(Coord coords)
        {
            if (coords == null) throw new ArgumentNullException(nameof(coords));

            Console.BackgroundColor = BackGround;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(coords.X + Padding,coords.Y + PaddingTop);
            Console.Write(Food);

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает набранные очки
        /// </summary>
        /// <param name="score"></param>
        /// <param name="mapSize"></param>
        static public void DrawScores(int score, Coord mapSize)
        {
            int x = mapSize.X;
            int y = mapSize.Y;
            Console.SetCursorPosition(x + 1 + Padding, 0 + PaddingTop);
            Console.Write($"{score}pts");
        }

        /// <summary>
        /// Отрисовывает экран конца игры
        /// </summary>
        static public void DrawGameover(Coord mapSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(mapSize.X + 2 + Padding, 3 + PaddingTop);
            Console.Write("Game over!");

            Console.SetCursorPosition(mapSize.X + 2 + Padding, 4 + PaddingTop);
            Console.Write("Quit to main menu..");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// Вывод текста в центр консоли с переходом на следующую строку
        /// </summary>
        /// <param name="text">Текст для вывода</param>
        static public void WriteLineCenter(string text)
        {
            Console.SetCursorPosition(Padding,Console.CursorTop);
            Console.WriteLine(text);
        }

        /// <summary>
        /// Вывод текста в центр консоли
        /// </summary>
        /// <param name="text">Текст для вывода</param>
        static public void WriteCenter(string text)
        {
            Console.SetCursorPosition(Padding,Console.CursorTop);
            Console.Write(text);
        }

        /// <summary>
        /// Отступ для центрирования текста
        /// </summary>
        static public int Padding { get
            {
                var width = Console.WindowWidth;
                return width *3/ 8;
            } }

        /// <summary>
        /// Вертикальный отступ для центрирования текста
        /// </summary>
        public static int PaddingTop
        {
            get
            {
                var height = Console.WindowHeight;
                return height * 1/4;
            }
        }
    }
}
