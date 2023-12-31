using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

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

                foreach(var state in stateMachine.GetStates())
                {
                    foreach(var transition in state.GetTransitions())
                    {
                        CreateTransitionEdge(transition);
                    }
                }
            }
        }

        public void UpdateStates()
        {
            foreach(var node in nodes)
            {
                StateView stateView = node as StateView;

                if(stateView != null)
                {
                    stateView.UpdateState();
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if(!Application.isPlaying)
            {
                base.BuildContextualMenu(evt);
                Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
                evt.menu.AppendAction($"Create State", a => CreateState(typeof(ActionState), mousePosition));
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            foreach(var endPort in ports)
            {
                if(endPort.direction == startPort.direction)
                {
                    continue;
                }

                if(AreConnected(startPort, endPort))
                {
                    continue;
                }

                compatiblePorts.Add(endPort);
            }

            return compatiblePorts;
        }

        bool AreConnected(Port startPort, Port endPort)
        {
            foreach(var connection in startPort.connections)
            {
                if(connection.input == endPort || connection.output == endPort)
                {
                    return true;
                }
            }

            return false;
        }

        void CreateStateView(State state)
        {
            StateView newStateView = new(state);
            AddElement(newStateView);
        }

        void CreateState(Type type, Vector2 mousePosition)
        {
            State newState = stateMachine.CreateState(type, mousePosition);
            CreateStateView(newState);
        }

        void RemoveState(StateView stateView)
        {
            stateMachine.RemoveState(stateView.GetState());
        }

        StateView GetStateView(string stateID)
        {
            return GetNodeByGuid(stateID) as StateView;
        }

        void CreateTransitionEdge(Transition transition)
        {
            StateView rootStateView = GetStateView(transition.GetRootStateID());
            StateView trueStateView = GetStateView(transition.GetTrueStateID());
            AddElement(rootStateView.ConnectTo(trueStateView));
        }

        void CreateTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.AddTransition(edge.input.node.viewDataKey);
        }

        void RemoveTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.RemoveTransition(edge.input.node.viewDataKey);
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            var edgesToCreate = graphViewChange.edgesToCreate;

            if(edgesToCreate != null)
            {
                foreach(var edge in edgesToCreate)
                {
                    CreateTransition(edge as TransitionEdge);
                }
            }

            var elementsToRemove = graphViewChange.elementsToRemove;

            if(elementsToRemove != null)
            {
                foreach(var element in elementsToRemove)
                {
                    StateView stateView = element as StateView;

                    if(stateView != null)
                    {
                        RemoveState(stateView);
                    }

                    TransitionEdge transitionEdge = element as TransitionEdge;

                    if(transitionEdge != null)
                    {
                        RemoveTransition(transitionEdge);
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