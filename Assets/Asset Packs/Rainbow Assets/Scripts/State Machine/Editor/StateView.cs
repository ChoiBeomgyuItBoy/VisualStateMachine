using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateView : Node
    {
        State state;

        public StateView(State state) : base(StateMachineEditor.path + "StateView.uxml")
        {
            this.state = state;

            style.left = state.GetPosition().x;
            style.top = state.GetPosition().y;

            SetTitle();
        }

        public State GetState()
        {
            return state;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            state.SetPosition(new Vector2(newPos.x, newPos.y));
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Selection.activeObject = state;
        }

        void SetTitle()
        {
            if(state is ActionState)
            {
                BindTitle();
            }
        }

        void BindTitle()
        {
            Label titleLabel = this.Q<Label>("title-label");
            titleLabel.bindingPath = "title";
            titleLabel.Bind(new SerializedObject(state));
        }
    }   
}
