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
        // Ширина и высота окна
        int Width { get; set; }
        int Height { get; set; }
        // Имя окна
        string WindowName { get; set; }
        // Открыта ли программа на весь экран
        bool ISFullScreen { get; }

        // Инициализация оконной системы
        void Init();

        // Установка дефолтного состояния при запуске или запуск игрового цикла
        void Run();

        // Завершение программы
        void ShutDown();

        // Изменение размеров окна
        void ResizeWindow(int width, int Height);

        // Переводит приложение в полноэкранный режим
        void ToggleFullscreen();
    }
    }
}
