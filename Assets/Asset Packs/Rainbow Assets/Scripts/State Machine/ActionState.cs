using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class ActionState : State
    {
        [SerializeField] ActionData[] onEnterActions;
        [SerializeField] ActionData[] onTickActions;
        [SerializeField] ActionData[] onExitActions;

        [System.Serializable]
        class ActionData
        {
            public string actionID;
            public string[] parameters;
        }

        public override void Enter()
        {
            base.Enter();
            DoActions(onEnterActions);
        }

        public override void Tick()
        {
            base.Tick();
            DoActions(onTickActions);
        }

        public override void Exit()
        {
            base.Exit();
            DoActions(onExitActions);
        }

        void DoActions(ActionData[] actions)
        {
            foreach(var action in controller.GetComponents<IAction>())
            {
                foreach(var actionData in actions)
                {
                    action.DoAction(actionData.actionID, actionData.parameters);
                }
            }
        }
    }
}
