#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using TappyTale.PostNavCoverFSM.Runtime.FSM;

namespace TappyTale.PostNavCoverFSM.Editor.FSMGraph
{
    public static class FsmGraphRebuilder
    {
        public static void Rebuild(FsmGraphAsset graph, StateMachineController controller)
        {
            if (graph == null || controller == null)
            {
                return;
            }

            var guidToState = new Dictionary<string, FSMState>();
            var runtimeStates = new List<FSMStateDefinition>();
            FSMState initialState = null;

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                FsmGraphNodeData node = graph.Nodes[i];
                if (node.State == null)
                {
                    continue;
                }

                guidToState[node.Guid] = node.State;
            }

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                FsmGraphNodeData node = graph.Nodes[i];
                if (node.State == null)
                {
                    continue;
                }

                var definition = new FSMStateDefinition
                {
                    State = node.State,
                    Transitions = new List<FSMTransitionDefinition>()
                };

                for (int t = 0; t < node.Transitions.Count; t++)
                {
                    FsmGraphTransitionData transition = node.Transitions[t];
                    if (!guidToState.TryGetValue(transition.TargetNodeGuid, out FSMState targetState))
                    {
                        continue;
                    }

                    definition.Transitions.Add(new FSMTransitionDefinition
                    {
                        Condition = transition.Condition,
                        TargetState = targetState
                    });
                }

                if (initialState == null || graph.EntryNodeGuid == node.Guid)
                {
                    initialState = node.State;
                }

                runtimeStates.Add(definition);
            }

            Undo.RecordObject(controller, "Rebuild FSM Controller");
            controller.SetAuthoringData(initialState, runtimeStates);
            EditorUtility.SetDirty(controller);
        }
    }
}
#endif
