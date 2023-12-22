using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        [SerializeField] StateMachine stateMachine;

        public void SwitchState(string newStateID)
        {
            stateMachine.SwitchState(newStateID);
        }

        void Start()
        {
            stateMachine.Bind(this);
            stateMachine.Enter();
        }

        void Update()
        {
            stateMachine.Tick();
        }
    }
}
