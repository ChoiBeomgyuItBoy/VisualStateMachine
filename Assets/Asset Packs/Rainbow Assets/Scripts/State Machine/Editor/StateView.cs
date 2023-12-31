using System;
using System.Collections.Generic;
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
        Port inputPort;
        Port outputPort;

        public StateView(State state) : base(StateMachineEditor.path + "StateView.uxml")
        {
            this.state = state;

            viewDataKey = state.name;

            style.left = state.GetPosition().x;
            style.top = state.GetPosition().y;

            CreatePorts();
            SetTitle();
            SetStyle();
            SetCapabilites();
        }

        public State GetState()
        {
            return state;
        }

        public TransitionEdge ConnectTo(StateView stateView)
        {
            return outputPort.ConnectTo<TransitionEdge>(stateView.inputPort);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            state.SetPosition(new Vector2(newPos.x, newPos.y));
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if(state is not EntryState)
            {
                Selection.activeObject = state;
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Transition", a => DragTransitionEdge());
        }

        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);

            if(inputPort != null)
            {
                foreach(var connection in inputPort.connections)
                {
                    collectedElementSet.Add(connection);
                }
            }

            if(outputPort != null)
            {
                foreach(var connection in outputPort.connections)
                {
                    collectedElementSet.Add(connection);
                }
            }
        }

        void CreatePorts()
        {
            if(state is ActionState)
            {
                inputPort = CreatePort(Direction.Input, Port.Capacity.Multi);
                outputPort = CreatePort(Direction.Output, Port.Capacity.Multi);
            }

            if(state is EntryState)
            {
                outputPort = CreatePort(Direction.Output, Port.Capacity.Single);
            }

            if(state is AnyState)
            {
                outputPort = CreatePort(Direction.Output, Port.Capacity.Multi);
            }
        }

        Port CreatePort(Direction direction, Port.Capacity capacity)
        {
            Port port = Port.Create<TransitionEdge>(Orientation.Vertical, direction, capacity, typeof(bool));
            Insert(0, port);
            return port;
        }

        void SetTitle()
        {
            if(state is ActionState)
            {
                BindTitle();
            }
            else
            {
                title = state.GetTitle();
            }
        }

        void BindTitle()
        {
            Label titleLabel = this.Q<Label>("title-label");
            titleLabel.bindingPath = "title";
            titleLabel.Bind(new SerializedObject(state));
        }

        void SetStyle()
        {
            VisualElement root = this.Q<VisualElement>("node-border");

            if(state is ActionState)
            {
                root.AddToClassList("actionState");
            }

            if(state is EntryState)
            {
                root.AddToClassList("entryState");
            }

            if(state is AnyState)
            {
                root.AddToClassList("anyState");
            }
        }

        void SetCapabilites()
        {
            if(state is EntryState || state is AnyState)
            {
                capabilities -= Capabilities.Deletable;
            }
        }

        class DragEvent : MouseDownEvent
        {
            public DragEvent(Vector2 mousePosition, VisualElement target)
            {
                this.mousePosition = mousePosition;
                this.target = target;
            }
        }

        void DragTransitionEdge()
        {
            outputPort.SendEvent(new DragEvent(outputPort.GetGlobalCenter(), outputPort));
        }
    }   
}
