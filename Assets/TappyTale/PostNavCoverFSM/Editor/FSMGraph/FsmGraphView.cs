#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using TappyTale.PostNavCoverFSM.Runtime.FSM;

namespace TappyTale.PostNavCoverFSM.Editor.FSMGraph
{
    public sealed class FsmGraphView : GraphView
    {
        public event Action<FsmGraphNodeView> NodeCreated;

        public FsmGraphView()
        {
            style.flexGrow = 1f;
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            graphViewChanged += OnGraphViewChanged;

            this.AddManipulator(new ContextualMenuManipulator(menu =>
            {
                menu.menu.AppendAction("Create State Node", _ => CreateNode(new Vector2(200f, 200f)));
            }));
        }

        public FsmGraphNodeView CreateNode(Vector2 position)
        {
            var data = new FsmGraphNodeData
            {
                Guid = System.Guid.NewGuid().ToString("N"),
                Title = "State",
                Position = position,
                State = null
            };

            var nodeView = new FsmGraphNodeView(data);
            AddElement(nodeView);
            NodeCreated?.Invoke(nodeView);
            return nodeView;
        }

        public void Load(FsmGraphAsset graph)
        {
            DeleteElements(graphElements.ToList());

            if (graph == null)
            {
                return;
            }

            var lookup = new Dictionary<string, FsmGraphNodeView>();
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                FsmGraphNodeData data = graph.Nodes[i];
                var view = new FsmGraphNodeView(data);
                lookup[data.Guid] = view;
                AddElement(view);
            }

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                FsmGraphNodeData data = graph.Nodes[i];
                if (!lookup.TryGetValue(data.Guid, out var fromView))
                {
                    continue;
                }

                for (int t = 0; t < data.Transitions.Count; t++)
                {
                    FsmGraphTransitionData transition = data.Transitions[t];
                    if (!lookup.TryGetValue(transition.TargetNodeGuid, out var toView))
                    {
                        continue;
                    }

                    var edge = fromView.OutputPort.ConnectTo(toView.InputPort);
                    edge.userData = transition.Condition;
                    AddElement(edge);
                }
            }
        }

        public void SaveTo(FsmGraphAsset graph)
        {
            graph.Nodes.Clear();

            var nodeViews = nodes.ToList().OfType<FsmGraphNodeView>().ToList();
            var nodeLookup = new Dictionary<string, FsmGraphNodeData>();

            for (int i = 0; i < nodeViews.Count; i++)
            {
                var nodeView = nodeViews[i];
                var data = new FsmGraphNodeData
                {
                    Guid = nodeView.Guid,
                    Title = nodeView.title,
                    Position = nodeView.GetPosition().position,
                    State = nodeView.StateField.value as FSMState
                };

                graph.Nodes.Add(data);
                nodeLookup[data.Guid] = data;
            }

            var edgesList = edges.ToList();
            for (int i = 0; i < edgesList.Count; i++)
            {
                var edge = edgesList[i];
                if (edge.output?.node is not FsmGraphNodeView fromNode || edge.input?.node is not FsmGraphNodeView toNode)
                {
                    continue;
                }

                if (!nodeLookup.TryGetValue(fromNode.Guid, out var fromData))
                {
                    continue;
                }

                fromData.Transitions.Add(new FsmGraphTransitionData
                {
                    TargetNodeGuid = toNode.Guid,
                    Condition = edge.userData as FSMCondition
                });
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            return change;
        }
    }
}
#endif
