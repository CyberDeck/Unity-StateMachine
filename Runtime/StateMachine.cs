using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Secretlab.StateMachine {

    public interface IStateMachine<T> where T : IState<T> {
        bool allowChange { get; }
        T state { set; get; }
        T forceState { set; }
    }
    public interface IStateMachine : IStateMachine<IState> {
    }

    /// <summary>
    /// A timed state machine. 
    /// </summary>
    /// A timed state machine. It uses the Time.time scaled time for delay calculation. 
    public class TimedStateMachine<T> : IStateMachine<T> where T : IState<T> {
        private T currentState;
        private T nextDelayedState;
        private float timeDelay;

        public float delay { set { timeDelay = Time.time + value; } get { return timeDelay - Time.time; } }
        public bool allowChange { get { return IsStateChangeAllowed(); } }
        public T state { set { ChangeStateDelayed(value); } get { return currentState; } }
        public T forceState { set { ForceChangeState(value); } }
        public T nextState { get { return nextDelayedState; } }


        public TimedStateMachine(T currentState = default(T)) {
            this.currentState = currentState;
            this.nextDelayedState = default(T);
            if (this.currentState != null) {
                timeDelay = float.MaxValue;
                this.delay = currentState.Enter();
            } else {
                this.delay = 0f;
            }
        }

        /// <summary>
        /// Check if a state change is allowed, i.e. the current state is not in delay.
        /// </summary>
        /// <returns>true if a state change is allowed or false otherwise.</returns>
        public bool IsStateChangeAllowed() {
            return Time.time >= timeDelay;
        }

        /// <summary>
        /// Forcefully change the state if the current state. If the current state is delayed this may lead to unexpected behaviour.
        /// </summary>
        /// <param name="nextState">The new state to switch the state machine to.</param>
        public void ForceChangeState(T nextState) {
            if (currentState != null) {
                // Exit current state
                currentState.Exit();
            }
            // Void the delayed next state
            nextDelayedState = default(T);
            // Change State
            currentState = nextState;
            timeDelay = float.MaxValue;
            delay = currentState.Enter();
        }

        /// <summary>
        /// Change the state if the current state is not delayed, i.e. allowing for a state change
        /// </summary>
        /// <param name="nextState">The new state to switch the state machine to.</param>
        /// <returns>true if the state change was performed. False if the state machine is currently disallowing a state change.</returns>
        public bool ChangeState(T nextState) {
            if (!IsStateChangeAllowed()) {
                return false;
            }
            ForceChangeState(nextState);
            return true;
        }

        /// <summary>
        /// Change the state if the current state is not delayed, i.e. allowing for a state change. Otherwise change the state after the delay.
        /// </summary>
        /// Change the state if the current state is not delayed, i.e. allowing for a state change. Otherwise change the state after the delay. If a state is registered for a delayed state change it is overwritten.
        /// <param name="nextState">The new state to switch the state machine to.</param>
        public void ChangeStateDelayed(T nextState) {
            if (!IsStateChangeAllowed()) {
                nextDelayedState = nextState;
            } else {
                ForceChangeState(nextState);
            }
        }

        /// <summary>
        /// Perform a delayed state change if a delayed state change should be performed.
        /// </summary>
        /// <returns>True if a state change was performed. Otherwise false. </returns>
        private bool DoDelayedStateChange() {
            if (nextDelayedState != null) {
                if (IsStateChangeAllowed()) {
                    ForceChangeState(nextDelayedState);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// To be called at each frame update.
        /// </summary>
        public void Update() {
            if (DoDelayedStateChange()) {
                return;
            }
            if (currentState != null) {
                // Update the state.
                T nextState = currentState.Update();
                if (nextState != null) {
                    // State requested a following state. Change it forcefully.
                    ForceChangeState(nextState);
                }
            }
        }

        /// <summary>
        /// To be called at each fixed update, i.e. physics update.
        /// </summary>
        public void FixedUpdate() {
            if (DoDelayedStateChange()) {
                return;
            }
            if (currentState != null) {
                // Update the state.
                T nextState = currentState.FixedUpdate();
                if (nextState != null) {
                    // State requested a following state. Change it forcefully.
                    ForceChangeState(nextState);
                }
            }
        }
    }

    public class TimedStateMachine : TimedStateMachine<IState> {
    }

}