using System;

namespace Snake
{
    /// <summary>
    /// Модуль ввода
    /// </summary>
    class Input
    {
        // Клавиши управления
        public ConsoleKey UpKey;

        public ConsoleKey DownKey;

        public ConsoleKey LeftKey;

        public ConsoleKey RightKey;
        
        // TODO В дальнейшем создать отдельный класс с полями клавишами и передавать его в этот конструктор
        /// <summary>
        /// Создаёт новый модуль ввода с указанными стандартными клавишами
        /// </summary>
        /// <param name="upKey"></param>
        /// <param name="downKey"></param>
        /// <param name="leftKey"></param>
        /// <param name="rightKey"></param>
        public Input(ConsoleKey upKey, ConsoleKey downKey, ConsoleKey leftKey, ConsoleKey rightKey)
        {
            UpKey = upKey;
            DownKey = downKey;
            LeftKey = leftKey;
            RightKey = rightKey;
        }

        /// <summary>
        /// Создаёт новый модуль ввода с клавишами по умолчанию
        /// </summary>
        public Input()
        {
            UpKey = Config.UpKey;
            DownKey = Config.DownKey;
            LeftKey = Config.LeftKey;
            RightKey = Config.RightKey;
        }

        /// <summary>
        /// Интерпретирует нажатую клавишу и возвращает информациию о действии
        /// </summary>
        /// <param name="kInfo">Информация о клавише</param>
        public ActionType KeyHandler(ConsoleKeyInfo kInfo)
        {
            if (kInfo.Key == RightKey)
            {
                return ActionType.Right;
            }
            if (kInfo.Key == LeftKey)
            {
                return ActionType.Left;
            }
            if (kInfo.Key == UpKey)
            {
                return ActionType.Up;
            }
            if (kInfo.Key == DownKey)
            {
                return ActionType.Down;
            }
            if (kInfo.Key == ConsoleKey.Escape)
            {
                return ActionType.Exit;
            }
            return ActionType.None;
        }

        /// <summary>
        /// Ожидает нажатия клавиши и возвращает ассоциативный тип действия
        /// </summary>
        /// <returns></returns>
        public ActionType AskForInput()
        {
            if (Console.KeyAvailable)
            {
                var kInfo = Console.ReadKey(true);
                return KeyHandler(kInfo);
            }
            return ActionType.None;
        }
    }

    /// <summary>
    /// Класс-хранилище клавиш управления
    /// </summary>
    static class Config
    {
        public static ConsoleKey UpKey;

        public static ConsoleKey DownKey;

        public static ConsoleKey LeftKey;

        public static ConsoleKey RightKey;
    }
}
