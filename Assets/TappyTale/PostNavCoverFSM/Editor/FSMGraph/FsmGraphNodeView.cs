#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using TappyTale.PostNavCoverFSM.Runtime.FSM;

namespace TappyTale.PostNavCoverFSM.Editor.FSMGraph
{
    public sealed class FsmGraphNodeView : Node
    {
        public string Guid { get; }
        public Port InputPort { get; }
        public Port OutputPort { get; }
        public ObjectField StateField { get; }

        public FsmGraphNodeView(FsmGraphNodeData data)
        {
            Guid = data.Guid;
            title = string.IsNullOrWhiteSpace(data.Title) ? "State" : data.Title;
            viewDataKey = Guid;

            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "In";
            inputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            OutputPort.portName = "Out";
            outputContainer.Add(OutputPort);

            StateField = new ObjectField("State")
            {
                objectType = typeof(FSMState),
                value = data.State
            };
            StateField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is FSMState state)
                {
                    title = state.name;
                }
            });

            extensionContainer.Add(StateField);
            RefreshExpandedState();
            RefreshPorts();
            SetPosition(new Rect(data.Position, new Vector2(260f, 160f)));
        }
    }
}
#endif
