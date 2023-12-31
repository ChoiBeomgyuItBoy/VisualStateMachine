using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class ActionState : State
    {
        [SerializeField] ActionData[] actionsData;

        [System.Serializable]
        class ActionData
        {
            public string actionID;
            public string[] parameters;
        }

        public override void Tick()
        {
            base.Tick();
            DoActions();
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
