using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [CreateAssetMenu(menuName = "New State Machine")]
    public class StateMachine : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] EntryState entryState;
        [SerializeField] AnyState anyState;
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
            SwitchState(entryState.GetEntryStateID());
        }

        public void Tick()
        {
            currentState.Tick();
            anyState.Tick();
        }

        public void SwitchState(string newStateID)
        {
            currentState = GetState(newStateID);
        }

#if UNITY_EDITOR
        public State CreateState(Type type)
        {
            State newState = MakeState(type);
            Undo.RegisterCreatedObjectUndo(newState, "State Created");
            Undo.RecordObject(this, "State Added");
            AddState(newState);
            return newState;
        }

        public void RemoveState(State stateToRemove)
        {
            Undo.RecordObject(this, "State Removed");
            states.Remove(stateToRemove);
            OnValidate();
            Undo.DestroyObjectImmediate(stateToRemove);
        }

        State MakeState(Type type)
        {
            State newState = CreateInstance(type) as State;
            newState.name = Guid.NewGuid().ToString();
            return newState;
        }

        void AddState(State newState)
        {
            states.Add(newState);
            OnValidate();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if(entryState == null)
            {
                entryState = MakeState(typeof(EntryState)) as EntryState;
                AddState(entryState);
            }

            if(anyState == null)
            {
                anyState = MakeState(typeof(AnyState)) as AnyState;
                AddState(anyState);
            }
            
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
