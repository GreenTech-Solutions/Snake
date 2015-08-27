using System;

namespace StrawberryGameEngine
{
    namespace Core
    {
        /// <summary>
        /// Макет используемых приложением выполняемых функций
        /// </summary>
        public delegate void Func();

        /// <summary>
        /// Менеджер состояний
        /// </summary>                      
        public class StateManager
        {
            /// <summary>
            /// Вершина стека состояний
            /// </summary>
            private State _currentState;

            /// <summary>
            /// Размер стека
            /// </summary>
            private int _size;

            /// <summary>
            /// Создаёт новый менеджер состояний
            /// </summary>
            public StateManager()
            {
                Clear();
            }

            /// <summary>
            /// Обнуляет поля
            /// </summary>
            private void Clear()
            {
                _currentState = null;
                _size = 0;
            }

            /// <summary>
            /// Проверяет пустой ли список
            /// </summary>
            public bool IsEmpty => _size == 0;

            // размер стека
            public virtual int Count => _size;

            // Установка текущего состояния
            public bool Push(Function callerFunction)
            {
                if (IsEmpty)
                {
                    _currentState = new State(callerFunction);
                    _size++;
                    return true;
                }
                _currentState.Prev = _currentState;
                _currentState.Function = callerFunction;
                return true;
            }

            // Возвращение к предыдущему состоянию
            public Function Pop()
            {
                try
                {
                    return IsEmpty ? null : _currentState.Function;
                }
                finally
                {
                    _currentState = _currentState.Prev;
                    _size--;
                }
            }

            public void Pop(ref State state)
            {
                try
                {
                    if (_size == 0)
                    {
                        throw new InvalidOperationException();
                    }
                    state = _currentState;

                }
                finally
                {
                    _currentState = _currentState.Prev;
                    _size--;
                }
            }

            // Удаление всех состояний
            public Function[] PopAll()
            {
                try
                {
                    if (IsEmpty)
                    {
                        return null;
                    }
                    var returnArray = new Function[_size];
                    for (var i = 0; i < _size; i++)
                    {
                        returnArray[i] = Pop();
                    }
                    return returnArray;
                }
                finally
                {
                    Clear();
                }
            }

            public void PopAll(ref State[] states)
            {
                try
                {
                    if (IsEmpty)
                    {
                        states = null;
                        return;
                    }
                    var returnArray = new State[_size];
                    for (var i = 0; i < _size; i++)
                    {
                        Pop(ref returnArray[i]);
                    }
                }
                finally
                {
                    Clear();
                }
            }

            // Вызов функции текущего состояния
            public void Process()
            {
                var currentProcess = Pop();
                currentProcess.Func();
            }
        }

        // Назначение для функции
        public enum Purpose
        {
            Stop = 0,
            Init,
            Frame,
            No
        }
        
        // Текущее программное состояние
        public class State
        {
            // ссылка на предыдущее состояние
            public State Prev;
            // указатель на функцию
            public Function Function;

            public State()
            {
                Function = null;
                Prev = null;
            }

            public State(Function func)
            {
                Function = func;
                Prev = null;
            }
        }

        // Инофрмация о функции и сама функция
        public class Function
        {
            public Func Func;
            public Purpose Purpose;

            public Function(Func func)
            {
                Func = func;
                Purpose = Purpose.No;
            }

            public Function(Func func, Purpose purpose)
            {
                Func = func;
                Purpose = purpose;
            }
        }
    }
}
