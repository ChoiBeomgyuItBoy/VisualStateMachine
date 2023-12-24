using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [CreateAssetMenu(menuName = "New State Machine")]
    public class StateMachine : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] string entryStateID;
        [SerializeField] List<State> states = new();
        Dictionary<string, State> stateLookup = new();
        State currentState;

        public void Bind(StateMachineController controller)
        {
            foreach(var state in states)
            {
                state.Bind(controller);
            }
        }

        public State GetState(string stateID)
        {
            if(!stateLookup.ContainsKey(stateID))
            {
                return null;
            }

            return stateLookup[stateID];
        }

        public IEnumerable<State> GetStates()
        {
            return states;
        }

        public void Enter()
        {
            SwitchState(entryStateID);
        }

        public void Tick()
        {
            currentState.Tick();
        }

        public void SwitchState(string newStateID)
        {
            currentState = GetState(newStateID);
        }

#if UNITY_EDITOR
        public State CreateState(Type type)
        {
            State newState = CreateInstance(type) as State;
            newState.name = Guid.NewGuid().ToString();

            Undo.RegisterCreatedObjectUndo(newState, "State Created");
            Undo.RecordObject(this, "State Added");

            states.Add(newState);
            OnValidate();
            
            return newState;
        }

        public void RemoveState(State stateToRemove)
        {
            Undo.RecordObject(this, "State Removed");
            states.Remove(stateToRemove);
            OnValidate();
            Undo.DestroyObjectImmediate(stateToRemove);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if(AssetDatabase.GetAssetPath(this) != "")
            {
                foreach(var state in states)
                {
                    if(AssetDatabase.GetAssetPath(state) == "")
                    {
                        AssetDatabase.AddObjectToAsset(state, this);
                    }
                }
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }  
#endif
        
        void OnEnable()
        {
            currentState = null;
        }

        void Awake()
        {
            OnValidate();
        }

        void OnValidate()
        {
            stateLookup.Clear();

            foreach(var state in states)
            {
                if(state != null)
                {
                    stateLookup[state.name] = state;
                }
            }
        }
    }
}
