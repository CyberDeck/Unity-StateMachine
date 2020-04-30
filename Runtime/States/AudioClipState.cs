using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Secretlab.StateMachine {

    public class AudioClipState : AState<IStateMachine<IState>> {
        private float delay { get; }
        private IState nextState { get; }
        private AudioClip clip { get; }
        private AudioSource audioSource { get; }
        private bool loop { get; }

        public AudioClipState(IStateMachine<IState> stateMachine, AudioSource audioSource, AudioClip clip, float delay = 0, bool loop = false, IState nextState = null) : base(stateMachine) {
            this.delay = delay;
            this.nextState = nextState;
            this.clip = clip;
            this.loop = loop;
            this.audioSource = audioSource;
        }
        public AudioClipState(IStateMachine<IState> stateMachine, AudioSource audioSource, AudioClip clip, bool delay, bool loop = false, IState nextState = null)
            : this(stateMachine, audioSource, clip, (delay && clip != null) ? clip.length : 0f, loop, nextState) {
        }

        public override float Enter() {
            //Debug.Log("ENTER "+this.GetType().Name);
            // Queue next state if one is set
            if (nextState != null) {
                parent.state = nextState;
            }
            if (clip == null) {
                // do not play a sound
                return 0;
            }
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
            return delay;
        }

        public override void Exit() {
            //Debug.Log("EXIT " + this.GetType().Name);
            audioSource.Stop();
        }
    }

}