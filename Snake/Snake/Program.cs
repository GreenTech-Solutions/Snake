using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// TODO Вывод результатов
// TODO Добавить коллизии с телом змеи
// TODO Добавить Меню и возможность изменять размер карты в настройках
// TODO Возможность изменения стилей змеи (знака генерации тела и змеи)

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Core core = new Core();
            core.Main();
        }
    }
}
