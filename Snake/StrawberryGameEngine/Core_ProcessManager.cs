using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine
{
    namespace Core
    {
        // Набор процессов
        public class ProcessManager
        {
            // Первый и последний процессы в списке
            public Process First;
            public Process Last;

            public ProcessManager()
            {
                Clear();
            }

            // Обнуляет поля
            private void Clear()
            {
                First = null;
                Last = null;
            }

            // Проверка на пустоту + Выбрасывает ошибку, если Список "частично пуст"
            public bool IsEmpty
            {
                get
                {
                    if (this.First == null)
                    {
                        if (!(this.Last == null))
                        {
                            throw new Exception("Непредвиденная ошибка в работе менеджера процессов.");
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Добавление процесса в конец списка
            public bool Push(Function CallerFunction)
            {
                if (this.IsEmpty)
                {
                    First = new Process(CallerFunction, 1);
                    First = new Process(First, null, Last);
                    return true;
                }
                else
                {
                    if (Last == null)
                    {
                        Last = new Process(CallerFunction, 2);
                        Last = new Process(Last, First);
                        return true;
                    }
                    Last.Prev = Last;
                    Last.function = CallerFunction;
                    Last.ProcessID++;
                    return true;
                }
            }

            // Удаление процесса из конца списка + возвращает последний процесс в списке
            public Process Pop(int id)
            {
                try
                {
                    if (this.IsEmpty)
                    {
                        return null;
                    }
                    Process CurrentProcess = this.First;
                    while (CurrentProcess != null)
                    {
                        if (id==1)
                        {
                            return CurrentProcess;
                        }
                        if (CurrentProcess.ProcessID == id)
                        {
                            CurrentProcess.Prev.Next = CurrentProcess.Next;
                            CurrentProcess.Next.Prev = CurrentProcess.Prev;
                            return CurrentProcess;
                        }
                        else
                        {
                            CurrentProcess = CurrentProcess.Next;
                        }
                    }
                    return null;
                }
                catch
                {

                    throw;
                }
                finally
                {
                    Process CurrentProcess = First.Next;
                    if (CurrentProcess == null)
                    {
                        Clear();
                    }
                    else
                    {
                        int ID = 1;
                        CurrentProcess.Prev.ProcessID = ID;
                        CurrentProcess = First;
                        while (CurrentProcess!=null)
                        {
                            CurrentProcess.Next.ProcessID = ++ID;
                            CurrentProcess = CurrentProcess.Next;
                        }
                    }
                }
            }

            // Удаление всех процессов + возвращает все процессы в списке в виде массива
            public Process[] PopAll()
            {
                try
                {
                    if (this.IsEmpty)
                    {
                        return null;
                    }
                    if (this.Last==null)
                    {
                        Process[] returnArray = { First };
                        return returnArray;
                    }
                    Process[] ReturnArray = new Process[this.Last.ProcessID];
                    Process CurrentID;
                    for (int i = 0; i < this.Last.ProcessID; i++)
                    {
                        CurrentID = First;
                        ReturnArray[i] = CurrentID;
                        CurrentID = CurrentID.Next;
                    }
                    return ReturnArray;
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

            // Запускает все функции в списке по порядку, начиная с первой
            public void Process()
            {
                Process CurrentProcess = First;
                while (CurrentProcess != null)
                {
                    CurrentProcess.function.func();
                    CurrentProcess = CurrentProcess.Next;
                }
            }
        }

        public class Process
        {
            // Следующий и предыдущий процесс в списке
            public Process Next = null;
            public Process Prev = null;

            // ID процесса
            public int ProcessID;

            // Указатель на функцию
            public Function function;

            public Process(Function func, int ID)
            {
                this.function = func;
                this.ProcessID = ID;
            }

            // Присваивает function и ProcessID как у current, а Prev и Next как у prev и next соответственно
            public Process(Process current, Process prev = null, Process next = null)
            {
                this.function = current.function;
                this.ProcessID = current.ProcessID;
                this.Prev = prev;
                this.Next = next;
            }
        }
    }
}
