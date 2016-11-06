using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisBall.Infrastructure
{
    [Serializable]
    public abstract class StatesReminder<T>
    {
        private int statesCapacity;

        protected static List<T> States { get; set; }

        public StatesReminder(int capacity = 5)
        {
            this.statesCapacity = capacity;
            this.StateId = 0;
        }

        protected int StateId { get; set; }

        public virtual bool CanGoBack { get; set; }

        public virtual bool CanGoForward { get; set; }

        public abstract void SetNextState();

        public abstract void SetPreviousState();

        public void GoBack()
        {
            if (CanGoBack)
            {
                SetPreviousState();
            }
        }

        public void GoForward()
        {
            if (CanGoForward)
            {
                SetNextState();
            }
        }

        protected void SaveCurrentState(T data)
        {
            if (States.Count() == statesCapacity + 1)
            {
                bool removed = States.Remove(States.First());
            }
            States.Add(ObjectCloner.Clone<T>(data));
        }
    }
}