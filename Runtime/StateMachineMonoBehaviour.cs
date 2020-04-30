using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Secretlab.StateMachine {

    public abstract class TimedStateMachineMonoBehaviour<T> : MonoBehaviour where T : IState<T> {
        protected TimedStateMachine<T> stateMachine;

        public float editorStateDelay { get { return Mathf.Max(0, stateMachine.delay); } }
        public string editorStateName { get { return state.GetType().Name + (stateMachine.nextState != null ? " -> " + stateMachine.nextState.GetType().Name : ""); } }
        protected T state { set { stateMachine.state = value; } get { return stateMachine.state; } }
        protected T forceState { set { stateMachine.forceState = value; } get { return stateMachine.state; } }

        abstract protected T InitialState();

        virtual protected void Awake() {
            stateMachine = new TimedStateMachine<T>(InitialState());
        }

        virtual protected void Update() {
            stateMachine.Update();
        }

        virtual protected void FixedUpdate() {
            stateMachine.FixedUpdate();
        }
    }

    public class TimedStateMachineMonoBehaviour : TimedStateMachineMonoBehaviour<IState> {
        protected override IState InitialState() {
            return null;
        }
    }

}