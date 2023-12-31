using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public abstract class State : ScriptableObject
    {
        [SerializeField] string uniqueID;
        [SerializeField] string title = "New State";
        [SerializeField] Vector2 position;
        [SerializeField] List<Transition> transitions = new();
        Dictionary<string, Transition> transitionLookup = new();
        bool started = false;
        protected StateMachineController controller;

        public void Bind(StateMachineController controller)
        {
            this.controller = controller;

            foreach(var transition in transitions)
            {
                transition.Bind(controller);
            }
        }

        public State Clone()
        {
            return Instantiate(this);
        }

        public bool Started()
        {
            return started;
        }

        public string GetUniqueID()
        {
            return uniqueID;
        }

        public void SetUniqueID(string uniqueID)
        {
            this.uniqueID = uniqueID;
        }

        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public IEnumerable<Transition> GetTransitions()
        {
            return transitions;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "State Moved");
            this.position = position;
            EditorUtility.SetDirty(this);
        }

        public void AddTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Added");
            transitions.Add(new Transition(uniqueID, trueStateID));
            OnValidate();
            EditorUtility.SetDirty(this);
        }

        public void RemoveTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Removed");
            transitions.Remove(GetTransition(trueStateID));
            OnValidate();
            EditorUtility.SetDirty(this);
        }
#endif

        public virtual void Enter()
        {
            started = true;
        }

        public virtual void Tick()
        {
            CheckTransitions();
        }

        public virtual void Exit()
        {
            started = false;
        }

        void Awake()
        {
            OnValidate();
        }

        void OnValidate()
        {
            transitionLookup.Clear();

            foreach(var transition in transitions)
            {
                if(transition != null)
                {
                    transitionLookup[transition.GetTrueStateID()] = transition;
                }
            }
        }

        Transition GetTransition(string trueStateID)
        {
            if(!transitionLookup.ContainsKey(trueStateID))
            {
                return null;
            }

            return transitionLookup[trueStateID];
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
    }
}
