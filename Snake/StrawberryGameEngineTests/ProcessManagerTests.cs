using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrawberryGameEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine.Core.Tests
{
    [TestClass()]
    public class ProcessManagerTests
    {
        [TestMethod()]
        public void PushTest()
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
            if (!(manager.First.ProcessID ==  1 && manager.Last==null))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PopTest()
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

        [TestMethod()]
        public void PopAllTest()
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

        [TestMethod()]
        public void ProcessTest()
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
    }
}