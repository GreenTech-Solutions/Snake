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
                        if (First == null)
                        {
                            if (!(Last == null))
                            {
                                throw new Exception("Непредвиденная ошибка в работе менеджера процессов.");
                            }
                            return true;
                        }
                        return false;
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
            /// <param name="id">Искомый ID процесса</param>
            /// <returns>Существует указанный ID или нет</returns>
            public bool IdExist(int id)
            {
                try
                {
                    var current = First;
                    while (current != null)
                    {
                        if (current.ProcessId == id)
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
            /// <param name="callerFunction">Вызываемая вынкция</param>
            /// <returns></returns>
            public bool Push(Function callerFunction)
            {
                try
                {
                    if (callerFunction==null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (IsEmpty)
                    {
                        First = new Process(callerFunction, 1);
                        First = new Process(First, null, Last);
                        return true;
                    }
                    if (Last == null)
                    {
                        Last = new Process(callerFunction, 2);
                        Last = new Process(Last, First);
                        return true;
                    }
                    Last.Prev = Last;
                    Last.Function = callerFunction;
                    Last.ProcessId++;
                    return true;
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
                    if (IsEmpty||!IdExist(id))
                    {
                        return null;
                    }
                    var currentProcess = First;
                    while (currentProcess != null)
                    {
                        if (id==1)
                        {
                            return currentProcess;
                        }
                        if (currentProcess.ProcessId == id)
                        {
                            currentProcess.Prev.Next = currentProcess.Next;
                            currentProcess.Next.Prev = currentProcess.Prev;
                            return currentProcess;
                        }
                        else
                        {
                            currentProcess = currentProcess.Next;
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
                    var currentProcess = First.Next;
                    if (currentProcess == null)
                    {
                        Clear();
                    }
                    else
                    {
                        var ID = 1;
                        currentProcess.Prev.ProcessId = ID;
                        currentProcess = First;
                        while (currentProcess!=null)
                        {
                            currentProcess.Next.ProcessId = ++ID;
                            currentProcess = currentProcess.Next;
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
                    if (IsEmpty)
                    {
                        return null;
                    }
                    if (Last==null)
                    {
                        Process[] returnArray = { First };
                        return returnArray;
                    }
                    var ReturnArray = new Process[Last.ProcessId];
                    Process currentId;
                    for (var i = 0; i < Last.ProcessId; i++)
                    {
                        currentId = First;
                        ReturnArray[i] = currentId;
                        currentId = currentId.Next;
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
                    var currentProcess = First;
                    while (currentProcess != null)
                    {
                        currentProcess.Function.Func();
                        currentProcess = currentProcess.Next;
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
            public Process Next;

            /// <summary>
            /// Предыдущий процесс в списке
            /// </summary>
            public Process Prev;

            /// <summary>
            /// ID Процесса
            /// </summary>
            public int ProcessId;

            /// <summary>
            /// Указатель на функцию
            /// </summary>
            public Function Function;

            /// <summary>
            /// Создаёт новый процесс
            /// </summary>
            /// <param name="func">Исполняемая функция</param>
            /// <param name="id">ID функции</param>
            public Process(Function func, int id)
            {
                Function = func;
                ProcessId = id;
            }

            /// <summary>
            /// Присваивает function и ProcessID как у current, а Prev и Next как у prev и next соответственно
            /// </summary>
            /// <param name="old">Процесс, на основе которого необходимо создать новый процесс</param>
            /// <param name="prev">Предыдущий процесс в списке</param>
            /// <param name="next">Следующий процесс в списке</param>
            public Process(Process old, Process prev = null, Process next = null)
            {
                Function = old.Function;
                ProcessId = old.ProcessId;
                Prev = prev;
                Next = next;
            }
        }
    }
}
