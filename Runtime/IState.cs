using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Secretlab.StateMachine {

    /// <summary>
    /// Basic interface of a single state for the StateMachine class.
    /// </summary>
    public interface IState<T> {
        /// <summary>
        /// Is called when the state is entered.
        /// </summary>
        /// <returns>
        /// Returns the minimum state time a state has to be active before exiting, i.e. switching to a next state
        /// </returns>
        float Enter();
        /// <summary>
        /// Is called at each frame update.
        /// </summary>
        /// Is called at each frame update. If the function returns a following state to be activated,  the following state will be activated regardless of the state delay.
        /// <returns>Null if the current state should be kept active or the next state to be activated.</returns>
        T Update();
        /// <summary>
        /// Is called at each fixed (physics) update.
        /// </summary>
        /// Is called at each physics update. If the function returns a following state to be activated,  the following state will be activated regardless of the state delay.
        /// <returns>Null if the current state should be kept active or the next state to be activated.</returns>
        T FixedUpdate();
        /// <summary>
        /// Is called when the state is exited.
        /// </summary>
        void Exit();
    }

    public interface IState : IState<IState> {
    }

    public class AState<T> : IState where T : class {
        protected T parent { get; }

        public AState(T parent) {
            this.parent = parent;
        }

        public virtual float Enter() { return 0; }
        public virtual void Exit() { }
        public virtual IState FixedUpdate() { return null; }
        public virtual IState Update() { return null; }
    }

}