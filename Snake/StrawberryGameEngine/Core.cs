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
            // Массив для хранения данных
            private State CurrentState;
            // Размер
            private int size;

            public StateManager()
            {
                this.size = 0;
                this.CurrentState = null;
            }

            private void Clear()
            {
                CurrentState = null;
                size = 0;
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
                try
                {
                    CurrentState = new State(CallerFunction);
                    if (CurrentState != null)
                    {
                        State temp;
                        temp = CurrentState;
                        CurrentState.Prev = temp;
                    }
                }
                catch
                {
                    throw;
                }
                // если состояние получилось установить
                return true;
            }

            // Возвращение к предыдущему состоянию
            public Function Pop()
            {
                try
                {
                    if (this.size == 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return this.CurrentState.function;
                } catch
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
                    if (this.IsEmpty())
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

        public class ProcessManager
        {

        }

        public class App
        {

        }
    }
}
