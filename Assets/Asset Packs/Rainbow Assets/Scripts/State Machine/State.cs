using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public abstract class State : ScriptableObject
    {
        [SerializeField] string title = "New State";
        [SerializeField] Vector2 position;
        [SerializeField] List<Transition> transitions = new();
        protected StateMachineController controller;

        public void Bind(StateMachineController controller)
        {
            this.controller = controller;

            foreach(var transition in transitions)
            {
                transition.Bind(controller);
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "State Moved");
            this.position = position;
            EditorUtility.SetDirty(this);
        }
#endif

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

        protected abstract void OnEnter();
        protected abstract void OnTick();
        protected abstract void OnExit();
    }
}
