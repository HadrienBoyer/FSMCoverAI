#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;
using TappyTale.PostNavCoverFSM.Runtime.FSM;
using TappyTale.PostNavCoverFSM.Runtime.Navigation;

namespace TappyTale.PostNavCoverFSM.Editor.Tools
{
    public static class PostNavCoverSetupMenu
    {
        [MenuItem("Tools/TappyTale/PostNav Cover/Add Core AI Stack To Selected")]
        public static void AddCoreStack()
        {
            if (Selection.activeGameObject == null)
            {
                EditorUtility.DisplayDialog("Core AI Stack", "Select a GameObject first.", "OK");
                return;
            }

            GameObject target = Selection.activeGameObject;

            if (target.GetComponent<UnityEngine.AI.NavMeshAgent>() == null)
            {
                Undo.AddComponent<UnityEngine.AI.NavMeshAgent>(target);
            }

            if (target.GetComponent<NavMeshNavigationAgent>() == null)
            {
                Undo.AddComponent<NavMeshNavigationAgent>(target);
            }

            if (target.GetComponent<StateMachineController>() == null)
            {
                Undo.AddComponent<StateMachineController>(target);
            }

            if (target.GetComponent<AIController>() == null)
            {
                Undo.AddComponent<AIController>(target);
            }

            EditorUtility.DisplayDialog("Core AI Stack", "Core AI components added to selected GameObject.", "OK");
        }
    }
}
#endif
