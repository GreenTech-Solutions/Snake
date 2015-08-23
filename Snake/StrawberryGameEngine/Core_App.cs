using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine
{
    namespace Core
    {
        public interface App
        {
            /// <summary>
            /// Ширина  окна
            /// </summary>
            int Width { get; set; }

            /// <summary>
            /// Высота окна
            /// </summary>
            int Height { get; set; }

            /// <summary>
            /// Заголовок окна
            /// </summary>
            string WindowName { get; set; }

            /// <summary>
            /// Сообщает, открыто ли главное окно на весь экран
            /// </summary>
            bool ISFullScreen { get; }

            /// <summary>
            /// Инициализация всех подсистем движка
            /// </summary>
            void Init();

            /// <summary>
            /// Запуск игрового цикла
            /// </summary>
            void Run();

            /// <summary>
            /// Завершение программы
            /// </summary>
            void ShutDown();

            /// <summary>
            /// Изменение размеров окна
            /// </summary>
            /// <param name="width">Новая ширина окна</param>
            /// <param name="Height">Новая высота окна</param>
            void ResizeWindow(int width, int Height);

            /// <summary>
            /// Переключает окно между полноэкранным и нормальным режимами
            /// </summary>
            void ToggleFullscreen();
        }
    }
}
