using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrawberryGameEngine.Core.Tests
{
    [TestClass]
    public class StateManagerTests
    {
        [TestMethod]
        public void PushTest()
        {
            Func func = delegate
            {
                var x = 0;
                var a = x;
                var b = a;
                var c = b;
                var d = c;
                x = c;
                if (x!=0)
                {
                    Assert.Fail();
                }
            };
            var manager = new StateManager();
            manager.Push(new Function(func));
            if (manager.Count!=1)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void PopTest()
        {
            Func func = delegate
            {
                var x = 0;
                var a = x;
                var b = a;
                var c = b;
                var d = c;
                x = c;
            };
            var manager = new StateManager();
            manager.Push(new Function(func));
            manager.Pop();
            if (!manager.IsEmpty)
            {
                Assert.Fail();
            } 
        }

        [TestMethod]
        public void PopAllTest()
        {
            Func func = delegate
            {
                var x = 0;
                var a = x;
                var b = a;
                var c = b;
                var d = c;
                x = c;
            };
            var manager = new StateManager();
            manager.Push(new Function(func));
            manager.PopAll();
            if (!manager.IsEmpty)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ProcessTest()
        {
            Func func = delegate
            {
                var x = 0;
                var a = x;
                var b = a;
                var c = b;
                var d = c;
                x = c;
                if (x != 0)
                {
                    Assert.Fail();
                }
            };
            var manager = new StateManager();
            manager.Push(new Function(func));
            manager.Process();
        }
    }
}