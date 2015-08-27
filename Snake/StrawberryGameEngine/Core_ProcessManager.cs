using System;

namespace StrawberryGameEngine
{
    namespace Core
    {
        /// <summary>
        /// Менеджер процессов
        /// </summary>
        public class ProcessManager
        {
            /// <summary>
            /// Первый процесс в списке
            /// </summary>
            public Process First;

            /// <summary>
            /// Последний процесс в списке
            /// </summary>
            public Process Last;

            /// <summary>
            /// Создаёт новый менеджер процессов
            /// </summary>
            public ProcessManager()
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
            /// Очищает список процессов
            /// </summary>
            private void Clear()
            {
                try
                {
                    First = null;
                    Last = null;
                }
                catch 
                {

                    throw;
                }
            }

            /// <summary>
            /// Проверяет пустой ли список
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    try
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
                    catch 
                    {

                        throw;
                    }
                }
            }

            /// <summary>
            /// Проверяет, существует ли процесс с указанным ID в списке
            /// </summary>
            /// <param name="ID">Искомый ID процесса</param>
            /// <returns>Существует указанный ID или нет</returns>
            public bool IDExist(int ID)
            {
                try
                {
                    Process Current = this.First;
                    while (Current != null)
                    {
                        if (Current.ProcessID == ID)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                catch 
                {

                    throw;
                }
            }

            /// <summary>
            /// Добавление процесса в конец списка
            /// </summary>
            /// <param name="CallerFunction">Вызываемая вынкция</param>
            /// <returns></returns>
            public bool Push(Function CallerFunction)
            {
                try
                {
                    if (CallerFunction==null)
                    {
                        throw new ArgumentNullException();
                    }
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
                catch 
                {

                    throw;
                }
            }

            /// <summary>
            /// Удаляет указанный процесс в списке
            /// </summary>
            /// <param name="id">ID процесса</param>
            /// <returns>возвращает указанный процесс в списке</returns>
            public Process Pop(int id)
            {
                try
                {
                    if (this.IsEmpty||!this.IDExist(id))
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

            /// <summary>
            /// Удаление всех процессов
            /// </summary>
            /// <returns>все процессы в списке в виде массива</returns>
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

            /// <summary>
            ///  Запускает все функции в списке по порядку, начиная с первой
            /// </summary>
            public void Run()
            {
                try
                {
                    Process CurrentProcess = First;
                    while (CurrentProcess != null)
                    {
                        CurrentProcess.function.func();
                        CurrentProcess = CurrentProcess.Next;
                    }
                }
                catch 
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Процесс и его описание
        /// </summary>
        public class Process
        {
            /// <summary>
            /// Следующий процесс в списке
            /// </summary>
            public Process Next = null;

            /// <summary>
            /// Предыдущий процесс в списке
            /// </summary>
            public Process Prev = null;

            /// <summary>
            /// ID Процесса
            /// </summary>
            public int ProcessID;

            /// <summary>
            /// Указатель на функцию
            /// </summary>
            public Function function;

            /// <summary>
            /// Создаёт новый процесс
            /// </summary>
            /// <param name="func">Исполняемая функция</param>
            /// <param name="ID">ID функции</param>
            public Process(Function func, int ID)
            {
                this.function = func;
                this.ProcessID = ID;
            }

            /// <summary>
            /// Присваивает function и ProcessID как у current, а Prev и Next как у prev и next соответственно
            /// </summary>
            /// <param name="old">Процесс, на основе которого необходимо создать новый процесс</param>
            /// <param name="prev">Предыдущий процесс в списке</param>
            /// <param name="next">Следующий процесс в списке</param>
            public Process(Process old, Process prev = null, Process next = null)
            {
                this.function = old.function;
                this.ProcessID = old.ProcessID;
                this.Prev = prev;
                this.Next = next;
            }
        }
    }
}
