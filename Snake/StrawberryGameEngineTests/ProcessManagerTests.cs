using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrawberryGameEngine.Core.Tests
{
    [TestClass()]
    public class ProcessManagerTests
    {
        [TestMethod()]
        public void PushTest()
        {
            try
            {
                Func func = delegate
        {
            int x = 0;
            int a = x;
            int b = a;
            int c = b;
            int d = c;
            x = c;
            if (x != 0)
            {
                Assert.Fail();
            }
        };
                ProcessManager manager = new ProcessManager();
                manager.Push(new Function(func));
                if (!(manager.First.ProcessID == 1 && manager.Last == null))
                {
                    Assert.Fail();
                }
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void PopTest()
        {
            try
            {
                Func func = delegate
        {
            int x = 0;
            int a = x;
            int b = a;
            int c = b;
            int d = c;
            x = c;
        };
                ProcessManager manager = new ProcessManager();
                manager.Push(new Function(func));
                manager.Pop(1);
                if (!manager.IsEmpty)
                {
                    Assert.Fail();
                }
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void PopAllTest()
        {
            try
            {
                Func func = delegate
        {
            int x = 0;
            int a = x;
            int b = a;
            int c = b;
            int d = c;
            x = c;
        };
                ProcessManager manager = new ProcessManager();
                manager.Push(new Function(func));
                manager.PopAll();
                if (!manager.IsEmpty)
                {
                    Assert.Fail();
                }
            }
            catch 
            {

                throw;
            }
        }

        [TestMethod()]
        public void ProcessTest()
        {
            try
            {
                Func func = delegate
        {
            int x = 0;
            int a = x;
            int b = a;
            int c = b;
            int d = c;
            x = c;
            if (x != 0)
            {
                Assert.Fail();
            }
        };
                StateManager manager = new StateManager();
                manager.Push(new Function(func));
                manager.Process();
            }
            catch 
            {

                throw;
            }
        }
    }
}