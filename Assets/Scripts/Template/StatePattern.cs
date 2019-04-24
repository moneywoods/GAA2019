using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
    public class State
    {
        public string Name { get; protected set; }
        public StateContex Context
        {
            get;
            protected set;
        }

        // 必要そうなら中継ぎクラスを作る
        // StateContexWithNakatugi StateContex<T> : Monobehaviour where T : Class
        //public delegate void stateEnterEvent(Nakatsugi nakatsugi);
        //public stateEnterEvent OnEnter;

        public delegate void stateEnterEvent();
        public stateEnterEvent OnEnter;

        public delegate void stateExitEvent();
        public stateExitEvent OnExit;

        public delegate void stateUpdate();
        public stateUpdate update;

        public State(StateContex stateContex)
        {
            Context = stateContex;
        }
    }

    public class StateContex : MonoBehaviour
    {
        public List<State> StateList = new List<State>();
        public State CurrentState { get; protected set; }

        public StateContex()
        {
            CurrentState = null;
        }
        object locker = new object();

        public void SetCurrentState(State state)
        {
            if(state == null || !StateList.Contains(state))
            {
                return;
            }
            CurrentState = state;
            Debug.Log(gameObject.name + "is now State: " + CurrentState.Name);
        }

        public void SetCurrentState(string stateName)
        {
            if(stateName == null)
            {
                return; // これがいるかは不明.
            }
            var targetState = StateList.Find(s => s.Name == stateName);
            if( targetState == null ) 
            {
                Debug.Log(stateName + "instance has not been \"new\"ed.");
                return;
            }

            CurrentState = targetState;
            Debug.Log(gameObject.name + "is set to State: " + CurrentState.Name);
        }

        public void AddState(State state)
        {
            if(state == null || StateList.Contains(state))
            {
                return;
            }
            StateList.Add(state);
        }

        public void TransitState(State target) // 名前が微妙かも
        {
            if(target == null || !StateList.Contains(target))
            {
                return;
            }

            TransitTo(target);
        }

        public void TransitState(string stateName)
        {
            if(stateName == null)
            {
                return; // これがいるかは不明.
            }

            var targetState = StateList.Find(s => s.Name == stateName);

            if(targetState == null)
            {
                Debug.Log(stateName + "instance has not been \"new\"ed.");
                return;
            }

            TransitTo(targetState);
            Debug.Log(gameObject.name + "transited ot State: " + CurrentState.Name);
        }

        private void TransitTo(State target) // null検証はしてません. 
        {
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


    public class GenericStateContex<TBase> : MonoBehaviour where TBase : MonoBehaviour
    {
        public List<GenericState<TBase>> StateList = new List<GenericState<TBase>>();
        public GenericState<TBase> CurrentState { get; protected set; }

        public GenericStateContex()
        {
            CurrentState = null;
        }
        object locker = new object();

        public void SetCurrentState(GenericState<TBase> state)
        {
            if(state == null || !StateList.Contains(state))
            {
                return;
            }
            CurrentState = state;
            Debug.Log(gameObject.name + "is now State: " + CurrentState.Name);
        }

        public void SetCurrentState(string stateName)
        {
            if(stateName == null)
            {
                return; // これがいるかは不明.
            }
            var targetState = StateList.Find(s => s.Name == stateName);
            if(targetState == null)
            {
                Debug.Log(stateName + "instance has not been \"new\"ed.");
                return;
            }

            CurrentState = targetState;
            Debug.Log(gameObject.name + "is set to State: " + CurrentState.Name);
        }

        public void AddState(GenericState<TBase> state)
        {
            if(state == null || StateList.Contains(state))
            {
                return;
            }
            StateList.Add(state);
        }

        public void TransitState(GenericState<TBase> target) // 名前が微妙かも
        {
            if(target == null || !StateList.Contains(target))
            {
                return;
            }

            TransitTo(target);
        }

        public void TransitState(string stateName)
        {
            if(stateName == null)
            {
                return; // これがいるかは不明.
            }

            var targetState = StateList.Find(s => s.Name == stateName);

            if(targetState == null)
            {
                Debug.Log(stateName + "instance has not been \"new\"ed.");
                return;
            }

            TransitTo(targetState);
            Debug.Log(gameObject.name + "transited ot State: " + CurrentState.Name);
        }

        private void TransitTo(GenericState<TBase> target) // null検証はしてません. 
        {
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
            if(CurrentState.update != null)
            {
                CurrentState.update(); // これを忘れない
            }
        }

        public class GenericState<TBase> where TBase : MonoBehaviour
        {
            public string Name { get; protected set; }
            public GenericStateContex<TBase> Context
            {
                get;
                protected set;
            }

            // 必要そうなら中継ぎクラスを作る
            // StateContexWithNakatugi StateContex<T> : Monobehaviour where T : Class
            //public delegate void stateEnterEvent(Nakatsugi nakatsugi);
            //public stateEnterEvent OnEnter;

            public delegate void stateEnterEvent();
            public stateEnterEvent OnEnter;

            public delegate void stateExitEvent();
            public stateExitEvent OnExit;

            public delegate void stateUpdate();
            public stateUpdate update;

            public GenericState(GenericStateContex<TBase> stateContex)
            {
                Context = stateContex;
            }
        }

    }
}

