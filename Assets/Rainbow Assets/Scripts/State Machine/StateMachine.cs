using System.Collections.Generic;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [CreateAssetMenu(menuName = "New State Machine")]
    public class StateMachine : ScriptableObject
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
            currentState?.Exit();
            currentState = GetState(newStateID);
            currentState.Enter();
        }
        
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
