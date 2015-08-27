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
                try
                {
                    Clear();
                }
                catch
                {

                    throw;
                }
            }

            /// <summary>
            /// Обнуляет поля
            /// </summary>
            public void Clear()
            {
                try
                {
                    _currentState = null;
                    _size = 0;
                }
                catch 
                {

                    throw;
                }
            }

            /// <summary>
            /// Проверяет пустой ли список
            /// </summary>
            public bool IsEmpty => _size == 0;

            /// <summary>
            /// Размер стека
            /// </summary>
            public virtual int Count => _size;

            /// <summary>
            /// Установка текущего состояния
            /// </summary>
            /// <param name="callerFunction">Функция определяющая состояние</param>
            /// <returns>Результат добавления состояния(Удалось или нет)</returns>
            public bool Push(Function callerFunction)
            {
                try
                {
                    if (callerFunction == null)
                    {
                        throw new ArgumentNullException();
                    }

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
                catch 
                {

                    throw;
                }
            }

            /// <summary>
            /// Возвращение к предыдущему состоянию
            /// </summary>
            /// <returns>Функция, которая была вверху стека</returns>
            public Function Pop()
            {
                try
                {
                    return IsEmpty ? null : _currentState.Function;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _currentState = _currentState.Prev;
                    _size--;
                }
            }

            /// <summary>
            /// Извлечени верхнего состояния из стека
            /// </summary>
            /// <param name="state">Ссылка на извлекаемое состояние</param>
            public void Pop(ref State state)
            {
                try
                {
                    if (state == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (_size == 0)
                    {
                        throw new InvalidOperationException();
                    }
                    state = _currentState;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    _currentState = _currentState.Prev;
                    _size--;
                }
            }

            /// <summary>
            /// Удаление всех состояний
            /// </summary>
            /// <returns>Массив с функциями, определяющими состояния в стеке</returns>
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
                catch
                {
                    throw;
                }
                finally
                {
                    Clear();
                }
            }

            /// <summary>
            /// Извлечение всех состояний
            /// </summary>
            /// <param name="states">Ссылка на массив с извлекаемыми состояниями</param>
            public void PopAll(ref State[] states)
            {
                try
                {
                    if (states == null)
                    {
                        throw  new ArgumentNullException();
                    }
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
                catch
                {
                    throw;
                }
                finally
                {
                    Clear();
                }
            }

            /// <summary>
            /// Вызов функции текущего состояния
            /// </summary>
            public void Process()
            {
                try
                {
                    var currentProcess = Pop();
                    currentProcess.Func();
                }
                catch 
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Назначение для функции
        /// </summary>
        public enum Purpose
        {
            Stop = 0,
            Init,
            Frame,
            No
        }

        /// <summary>
        /// Программное состояние
        /// </summary>
        public class State
        {
            /// <summary>
            /// Ссылка на предыдущее состояние
            /// </summary>
            public State Prev;

            /// <summary>
            /// Указатель на функцию
            /// </summary>
            public Function Function;

            /// <summary>
            /// Создаёт новое состояние с нулевыми значениями
            /// </summary>
            public State()
            {
                Function = null;
                Prev = null;
            }

            /// <summary>
            /// Создаёт новое состояние с указанной функцией
            /// </summary>
            /// <param name="func">Функция, определяющая состояние</param>
            public State(Function func)
            {
                if (func == null)
                {
                    throw  new ArgumentNullException();
                }
                Function = func;
                Prev = null;
            }
        }

        /// <summary>
        ///  Информация о функции и сама функция
        /// </summary>
        public class Function
        {
            /// <summary>
            /// Функция
            /// </summary>
            public Func Func;

            /// <summary>
            /// Назначение функции
            /// </summary>
            public Purpose Purpose;
            
            /// <summary>
            /// Создаёт новую функцию без назначения
            /// </summary>
            /// <param name="func">Используемая функция</param>
            public Function(Func func)
            {
                if (func == null)
                {
                    throw new ArgumentNullException();
                }
                Func = func;
                Purpose = Purpose.No;
            }
            
            /// <summary>
            /// Создаёт новую функцию с указанным назначением
            /// </summary>
            /// <param name="func">Функция</param>
            /// <param name="purpose">Назначение функции</param>
            public Function(Func func, Purpose purpose) : this(func)
            {
                Purpose = purpose == default(Purpose) ? Purpose.No : purpose;
            }
        }
    }
}
