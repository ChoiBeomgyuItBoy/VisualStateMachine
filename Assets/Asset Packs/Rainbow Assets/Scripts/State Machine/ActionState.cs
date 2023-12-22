using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class ActionState : State
    {
        [SerializeField] ActionData[] actionsData;

        protected override void OnEnter() { }

        protected override void OnTick()
        {
            DoActions();
        }

        protected override void OnExit() { }

        [System.Serializable]
        class ActionData
        {
            public string actionID;
            public string[] parameters;
        }

        void DoActions()
        {
            foreach(var action in controller.GetComponents<IAction>())
            {
                foreach(var actionData in actionsData)
                {
                    action.DoAction(actionData.actionID, actionData.parameters);
                }
            }
        }
    }
}
