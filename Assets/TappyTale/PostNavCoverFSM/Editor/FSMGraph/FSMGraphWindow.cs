#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using TappyTale.PostNavCoverFSM.Runtime.FSM;

namespace TappyTale.PostNavCoverFSM.Editor.FSMGraph
{
    public sealed class FSMGraphWindow : EditorWindow
    {
        private FsmGraphView graphView;
        private ObjectField graphField;
        private ObjectField controllerField;
        private FsmGraphAsset activeGraph;
        private StateMachineController activeController;

        [MenuItem("Tools/TappyTale/PostNav Cover/FSM Graph")]
        public static void Open()
        {
            FSMGraphWindow window = GetWindow<FSMGraphWindow>();
            window.titleContent = new GUIContent("FSM Graph");
            window.minSize = new Vector2(800f, 500f);
        }

        private void CreateGUI()
        {
            var toolbar = new Toolbar();

            graphField = new ObjectField("Graph")
            {
                objectType = typeof(FsmGraphAsset),
                value = activeGraph
            };
            graphField.RegisterValueChangedCallback(evt =>
            {
                activeGraph = evt.newValue as FsmGraphAsset;
                ReloadGraph();
            });
            toolbar.Add(graphField);

            controllerField = new ObjectField("Controller")
            {
                objectType = typeof(StateMachineController),
                value = activeController
            };
            controllerField.RegisterValueChangedCallback(evt =>
            {
                activeController = evt.newValue as StateMachineController;
            });
            toolbar.Add(controllerField);

            var saveButton = new ToolbarButton(SaveGraph) { text = "Save Graph" };
            toolbar.Add(saveButton);

            var rebuildButton = new ToolbarButton(RebuildController) { text = "Rebuild Controller" };
            toolbar.Add(rebuildButton);

            var entryButton = new ToolbarButton(SetSelectedAsEntry) { text = "Set Entry" };
            toolbar.Add(entryButton);

            rootVisualElement.Add(toolbar);

            graphView = new FsmGraphView();
            rootVisualElement.Add(graphView);
            ReloadGraph();
        }

        private void ReloadGraph()
        {
            if (graphView == null)
            {
                return;
            }

            graphView.Load(activeGraph);
        }

        private void SaveGraph()
        {
            if (activeGraph == null)
            {
                EditorUtility.DisplayDialog("FSM Graph", "Assign a FsmGraphAsset first.", "OK");
                return;
            }

            Undo.RecordObject(activeGraph, "Save FSM Graph");
            graphView.SaveTo(activeGraph);
            EditorUtility.SetDirty(activeGraph);
            AssetDatabase.SaveAssets();
        }

        private void RebuildController()
        {
            SaveGraph();

            if (activeGraph == null || activeController == null)
            {
                EditorUtility.DisplayDialog("FSM Graph", "Assign both a graph asset and a StateMachineController.", "OK");
                return;
            }

            FsmGraphRebuilder.Rebuild(activeGraph, activeController);
            AssetDatabase.SaveAssets();
        }

        private void SetSelectedAsEntry()
        {
            if (activeGraph == null || graphView == null)
            {
                return;
            }

            foreach (var item in graphView.selection)
            {
                if (item is FsmGraphNodeView nodeView)
                {
                    activeGraph.EntryNodeGuid = nodeView.Guid;
                    EditorUtility.SetDirty(activeGraph);
                    AssetDatabase.SaveAssets();
                    break;
                }
            }
        }
    }
}
#endif
