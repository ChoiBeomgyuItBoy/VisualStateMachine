using System.Collections.Generic;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public abstract class State : ScriptableObject
    {
        [SerializeField] List<Transition> transitions = new();
        protected StateMachineController controller;

        public void Bind(StateMachineController controller)
        {
            foreach(var transition in transitions)
            {
                transition.Bind(controller);
            }

            this.controller = controller;
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Tick()
        {
            CheckTransitions();
            OnTick();
        }

        public void Exit()
        {
            OnExit();
        }

        protected abstract void OnEnter();
        protected abstract void OnTick();
        protected abstract void OnExit();

        void CheckTransitions()
        {
            foreach(var transition in transitions)
            {
                bool success = transition.Check();

                if(success)
                {
                    string trueStateID = transition.GetTrueStateID();

                    controller.SwitchState(trueStateID);
                }
            }
        }
    }
}
