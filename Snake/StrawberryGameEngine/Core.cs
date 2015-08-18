using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine
{
    namespace Core
    {
        // Макет используемых приложением выполняемых функций
        public delegate void Func();

        public class StateManager
        {
            // Массив для хранения данных
            private Function[] _array;
            // Вместимость по умолчанию
            private const int default_capactity = 10;
            // Размер
            private int size;

            public StateManager()
            {
                this.size = 0;
                this._array = new Function[default_capactity];
            }

            // Проверка на пустоту
            public bool IsEmpty()
            {
                return this.size == 0;
            }

            // размер стека
            public virtual int Count
            {
                get
                {
                    return this.size;
                }
            }

            // Установка текущего состояния
            public bool Push(Function CallerFunction)
            {
                // Если возникло переполнение                              ---Есть проблемы с производительностью
                if (this.size == this._array.Length)
                {
                    Function[] NewArray = new Function[2 * this._array.Length];
                    Array.Copy(this._array, 0, NewArray, 0, this.size);
                    this._array = NewArray;
                }

                this._array[this.size++] = CallerFunction; 
                // если состояние получилось установить
                return true;
            }

            // Возвращение к предыдущему состоянию
            public Function Pop()
            {
                if (this.size == 0)
                {
                    throw new InvalidOperationException();
                }

                return this._array[--this.size];
            }

            // Удаление всех состояний
            public Function[] PopAll()
            {
                Function[] ReturnArray = _array;
                _array = new Function[default_capactity];
                return ReturnArray;
            }

            // Вызов функции текущего состояния
            public void Process()
            {
                Function CurrentProcess = this.Pop();
                CurrentProcess.func();
            }
        }

        // Назначение для функции
        public enum Purpose
        {
            STOP = 0,
            INIT,
            FRAME,
            NO
        }
        
        // Текущее программное состояние
        public class State
        {
            // ссылка на предыдущее состояние
            State Prev;
            // указатель на функцию
            Function function;

            public State()
            {
                function = null;
                Prev = null;
            }
        }

        // Инофрмация о функции и сама функция
        public class Function
        {
            public Func func;
            public Purpose purpose;
        }

        public class ProcessManager
        {

        }

        public class App
        {

        }
    }
}
