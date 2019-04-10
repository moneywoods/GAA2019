using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
    public class State<T> where T : Enum
    {
        public T CurrentStateIndex { get; private set; }
        public StateContex<T> Context
        {
            get;
            private set;
        }
        public delegate void stateEnterEvent();
        public stateEnterEvent OnEnter;

        public delegate void stateExitEvent();
        public stateExitEvent OnExit;

        public delegate void stateUpdate();
        public stateUpdate update;

        public State(StateContex<T> stateContex)
        {
            Context = stateContex;
        }
    }

    public class StateContex<T> : MonoBehaviour where T : IEnumerable
    {
        public List<State<T>> StateList = new List<State<T>>();
        public State<T> CurrentState { get; private set; }

        object locker = new object();

        public void SetCurrentState(State<T> state)
        {
            if(state == null || !StateList.Contains(state))
            {
                return;
            }

            CurrentState = state;
        }

        public void AddState(State<T> state)
        {
            if(state == null || StateList.Contains(state))
            {
                return;
            }
            StateList.Add(state);
        }

        public void TransitState(State<T> target)
        {
            if(target == null || !StateList.Contains(target))
            {
                return;
            }

            lock(locker)
            {
                if(CurrentState.OnExit != null)
                {
                    CurrentState.OnExit();
                }
                CurrentState = target;
                if(CurrentState.OnEnter != null)
                {
                    CurrentState.OnEnter();
                }
            }
        }

        /* Unity 関数 override */
        protected virtual void Update()
        {
            if( CurrentState.update != null)
            {
                CurrentState.update(); // これを忘れない
            }
        }
    }
}

