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

        // Менеджер состояний                        ---Исправить класс используя State как элемент цепи
        public class StateManager
        {
            // Вершина стека состояний
            private State CurrentState;
            // Размер
            private int size;

            public StateManager()
            {
                Clear();
            }

            // Обнуляет поля
            private void Clear()
            {
                this.CurrentState = null;
                this.size = 0;
            }

            // Проверка на пустоту
            public bool IsEmpty
            {
                get
                {
                    return this.size == 0;
                }
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
                try
                {
                    if (this.IsEmpty)
                    {
                        CurrentState = new State(CallerFunction);
                        this.size++;
                        return true;
                    }
                    CurrentState.Prev = CurrentState;
                    CurrentState.function = CallerFunction;
                    return true;
                }
                catch
                {
                    throw;
                }
            }

            // Возвращение к предыдущему состоянию
            public Function Pop()
            {
                try
                {
                    if (this.IsEmpty)
                    {
                        return null;
                    }
                    else
                    {
                    return this.CurrentState.function;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.CurrentState = this.CurrentState.Prev;
                    this.size--;
                }
            }

            public void Pop(ref State state)
            {
                try
                {
                    if (this.size == 0)
                    {
                        throw new InvalidOperationException();
                    }
                    state = this.CurrentState;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.CurrentState = this.CurrentState.Prev;
                    this.size--;
                }
            }

            // Удаление всех состояний
            public Function[] PopAll()
            {
                try
                {
                    if (this.IsEmpty)
                    {
                        return null;
                    }
                    Function[] ReturnArray = new Function[this.size];
                    for (int i = 0; i < this.size; i++)
                    {
                        ReturnArray[i] = this.Pop();
                    }
                    return ReturnArray;
                }
                catch
                {

                    throw;
                } finally
                {
                    this.Clear();
                }
            }

            public void PopAll(ref State[] states)
            {
                try
                {
                    if (this.IsEmpty)
                    {
                        states = null;
                        return;
                    }
                    State[] ReturnArray = new State[this.size];
                    for (int i = 0; i < this.size; i++)
                    {
                        this.Pop(ref ReturnArray[i]);
                    }
                    return;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.Clear();
                }
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
            public State Prev;
            // указатель на функцию
            public Function function;

            public State()
            {
                function = null;
                Prev = null;
            }

            public State(Function func)
            {
                this.function = func;
                Prev = null;
            }
        }

        // Инофрмация о функции и сама функция
        public class Function
        {
            public Func func;
            public Purpose purpose;

            public Function(Func func)
            {
                this.func = func;
                purpose = Purpose.NO;
            }

            public Function(Func func, Purpose purpose)
            {
                this.func = func;
                this.purpose = purpose;
            }
        }

        public class App
        {

        }
    }
}
