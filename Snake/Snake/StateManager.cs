using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class StateManager
    {
        private Stack<Action> States;

        public StateManager()
        {
            States = new Stack<Action>();
        }

        public StateManager(params Action[] states)
            :this()
        {
            foreach (var state in states)
            {
                States.Push(state);
            }
        }

        public void RunLastAddedState()
        {
            States.Pop().Invoke();
        }

        public void RunAllStates()
        {
            while (States.Count>0) 
            {
                RunLastAddedState();
            }
        }

        public void AddState(Action newState)
        {
            States.Push(newState);
        }

        public void ClearStack()
        {
            States.Clear();
        }
    }
}
