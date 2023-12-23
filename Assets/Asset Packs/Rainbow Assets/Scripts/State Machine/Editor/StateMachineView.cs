using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateMachineView : GraphView
    {
        new class UxmlFactory : UxmlFactory<StateMachineView, UxmlTraits> { }
        StateMachine stateMachine;

        public StateMachineView()
        {
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateMachineEditor.uss");
            styleSheets.Add(styleSheet);
            
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        public void Refresh(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);

            graphViewChanged += OnGraphViewChanged;

            if(stateMachine != null)
            {
                foreach(var state in stateMachine.GetStates())
                {
                    CreateStateView(state);
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Create State", a => CreateState(typeof(ActionState)));
        }

        void CreateStateView(State state)
        {
            StateView newStateView = new(state);
            AddElement(newStateView);
        }

        void CreateState(Type type)
        {
            State newState = stateMachine.CreateState(type);
            CreateStateView(newState);
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            var elementsToRemove = graphViewChange.elementsToRemove;

            if(elementsToRemove != null)
            {
                foreach(var element in elementsToRemove)
                {
                    StateView stateView = element as StateView;

                    if(stateView != null)
                    {
                        stateMachine.RemoveState(stateView.GetState());
                    }
                }
            }

            return graphViewChange;
        }

        void OnUndoRedo()
        {
            Refresh(stateMachine);
        }
    }
}