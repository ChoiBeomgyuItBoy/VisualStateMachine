using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [System.Serializable]
    public class Transition
    {
        [SerializeField] string rootStateID;
        [SerializeField] string trueStateID;
        [SerializeField] Condition condition;
        StateMachineController controller;

        public void Bind(StateMachineController controller)
        {
            this.controller = controller;
        }

        public string GetRootStateID()
        {
            return rootStateID;
        }

        public string GetTrueStateID()
        {
            return trueStateID;
        }

        public bool Check()
        {
            return condition.Check(controller.GetComponents<IPredicateEvaluator>());
        }
    }
}
