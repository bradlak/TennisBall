using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TennisBall.Infrastructure
{
    [Serializable]
    public abstract class StatesReminder<T>
    {
        protected int statesCapacity;
        public StatesReminder(int capacity = 5)
        {
            this.statesCapacity = capacity;
            this.StateId = 0;
        }
        protected static List<T> States { get; set; }
        protected int StateId { get; set; }
        public virtual bool CanGoBack { get; set; }
        public virtual bool CanGoForward { get; set; }

        public void GoBack()
        {

            if (CanGoBack)
            {
                SetPreviousState();
            }
        }

        protected virtual void SetPreviousState()
        {

        }

        public void GoForward()
        {
            if (CanGoForward)
            {
                SetNextState();
            }
        }

        protected virtual void SetNextState()
        {

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