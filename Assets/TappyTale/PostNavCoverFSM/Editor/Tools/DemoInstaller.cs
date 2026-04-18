#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
// Unity.AI.Navigation is an optional package that provides NavMeshSurface. Avoid importing it directly here.
using TappyTale.PostNavCoverFSM.Runtime.AI;
using TappyTale.PostNavCoverFSM.Runtime.Navigation;
using TappyTale.PostNavCoverFSM.Runtime.FSM;
using TappyTale.PostNavCoverFSM.Runtime.FSM.States;
using TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions;
using TappyTale.PostNavCoverFSM.Runtime.Cover;
using TappyTale.PostNavCoverFSM.Editor.FSMGraph;

namespace TappyTale.PostNavCoverFSM.Editor.Tools
{
    /// <summary>
    /// Provides editor menu items to generate demo assets and a playable scene for the PostNavCoverFSM package.
    /// </summary>
    public static class DemoInstaller
    {
        private const string DemoRootFolder = "Assets/TappyTale/PostNavCoverFSM/Demo";
        private const string DemoPrefabsFolder = DemoRootFolder + "/Prefabs";
        private const string DemoMaterialsFolder = DemoRootFolder + "/Materials";
        private const string DemoGraphsFolder = DemoRootFolder + "/Graphs";
        private const string DemoStatesFolder = DemoRootFolder + "/States";
        private const string DemoConditionsFolder = DemoRootFolder + "/Conditions";

        /// <summary>
        /// Generates demo prefabs, materials, states, conditions and graph assets. The generated assets are saved into the Demo folder.
        /// </summary>
        [MenuItem("Tools/TappyTale/Demo Install/Generate Demo Prefabs", priority = 100)]
        public static void GenerateDemoPrefabs()
        {
            // Ensure demo folders exist
            Directory.CreateDirectory(DemoRootFolder);
            Directory.CreateDirectory(DemoPrefabsFolder);
            Directory.CreateDirectory(DemoMaterialsFolder);
            Directory.CreateDirectory(DemoGraphsFolder);
            Directory.CreateDirectory(DemoStatesFolder);
            Directory.CreateDirectory(DemoConditionsFolder);

            // Create simple material for the enemy
            var enemyMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            enemyMaterial.color = new Color(0.8f, 0.1f, 0.1f, 1f);
            AssetDatabase.CreateAsset(enemyMaterial, $"{DemoMaterialsFolder}/EnemyMaterial.mat");

            // Create state assets
            IdleState idleState = ScriptableObject.CreateInstance<IdleState>();
            SeekCoverState seekState = ScriptableObject.CreateInstance<SeekCoverState>();
            MoveToCoverState moveState = ScriptableObject.CreateInstance<MoveToCoverState>();
            InCoverState inCoverState = ScriptableObject.CreateInstance<InCoverState>();

            AssetDatabase.CreateAsset(idleState, $"{DemoStatesFolder}/IdleState.asset");
            AssetDatabase.CreateAsset(seekState, $"{DemoStatesFolder}/SeekCoverState.asset");
            AssetDatabase.CreateAsset(moveState, $"{DemoStatesFolder}/MoveToCoverState.asset");
            AssetDatabase.CreateAsset(inCoverState, $"{DemoStatesFolder}/InCoverState.asset");

            // Create condition assets
            HasTargetCondition hasTarget = ScriptableObject.CreateInstance<HasTargetCondition>();
            HasAssignedCoverCondition hasCover = ScriptableObject.CreateInstance<HasAssignedCoverCondition>();
            ReachedDestinationCondition reachedDest = ScriptableObject.CreateInstance<ReachedDestinationCondition>();

            AssetDatabase.CreateAsset(hasTarget, $"{DemoConditionsFolder}/HasTargetCondition.asset");
            AssetDatabase.CreateAsset(hasCover, $"{DemoConditionsFolder}/HasAssignedCoverCondition.asset");
            AssetDatabase.CreateAsset(reachedDest, $"{DemoConditionsFolder}/ReachedDestinationCondition.asset");

            // Build graph asset
            var graph = ScriptableObject.CreateInstance<FsmGraphAsset>();
            // create node data
            FsmGraphNodeData idleNode = new FsmGraphNodeData
            {
                Guid = Guid.NewGuid().ToString("N"),
                Title = "Idle",
                Position = new Vector2(100f, 100f),
                State = idleState,
                Transitions = new List<FsmGraphTransitionData>()
            };
            FsmGraphNodeData seekNode = new FsmGraphNodeData
            {
                Guid = Guid.NewGuid().ToString("N"),
                Title = "SeekCover",
                Position = new Vector2(400f, 100f),
                State = seekState,
                Transitions = new List<FsmGraphTransitionData>()
            };
            FsmGraphNodeData moveNode = new FsmGraphNodeData
            {
                Guid = Guid.NewGuid().ToString("N"),
                Title = "MoveToCover",
                Position = new Vector2(700f, 100f),
                State = moveState,
                Transitions = new List<FsmGraphTransitionData>()
            };
            FsmGraphNodeData coverNode = new FsmGraphNodeData
            {
                Guid = Guid.NewGuid().ToString("N"),
                Title = "InCover",
                Position = new Vector2(1000f, 100f),
                State = inCoverState,
                Transitions = new List<FsmGraphTransitionData>()
            };
            // Set transitions
            idleNode.Transitions.Add(new FsmGraphTransitionData
            {
                TargetNodeGuid = seekNode.Guid,
                Condition = hasTarget
            });
            seekNode.Transitions.Add(new FsmGraphTransitionData
            {
                TargetNodeGuid = moveNode.Guid,
                Condition = hasCover
            });
            moveNode.Transitions.Add(new FsmGraphTransitionData
            {
                TargetNodeGuid = coverNode.Guid,
                Condition = reachedDest
            });
            // Add nodes to graph
            graph.EntryNodeGuid = idleNode.Guid;
            graph.Nodes = new List<FsmGraphNodeData> { idleNode, seekNode, moveNode, coverNode };

            string graphPath = $"{DemoGraphsFolder}/DemoGraph.asset";
            AssetDatabase.CreateAsset(graph, graphPath);

            // Create enemy prefab
            var enemyGO = new GameObject("DemoEnemyAI");
            // Add a visible body
            var meshObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            meshObject.name = "Model";
            meshObject.transform.SetParent(enemyGO.transform);
            meshObject.transform.localPosition = Vector3.zero;
            var renderer = meshObject.GetComponent<Renderer>();
            renderer.sharedMaterial = enemyMaterial;

            // Add navigation and AI components
            if (enemyGO.GetComponent<NavMeshAgent>() == null)
            {
                enemyGO.AddComponent<NavMeshAgent>();
            }
            if (enemyGO.GetComponent<NavMeshNavigationAgent>() == null)
            {
                enemyGO.AddComponent<NavMeshNavigationAgent>();
            }
            var smc = enemyGO.AddComponent<StateMachineController>();
            var aiController = enemyGO.AddComponent<AIController>();
            // Add cover post adapter if package is present
#if TAPPYTALE_POST_NAV_PRESENT
            enemyGO.AddComponent<TappyTale.PostNavCoverFSM.Runtime.Integration.PostNav.PnsPostAgentAdapter>();
#endif
            // Build runtime FSM from graph
            FsmGraphRebuilder.Rebuild(graph, smc);

            string prefabPath = $"{DemoPrefabsFolder}/DemoEnemyAI.prefab";
            PrefabUtility.SaveAsPrefabAsset(enemyGO, prefabPath);

            // Clean up temporary game object
            GameObject.DestroyImmediate(enemyGO);

            // Save and refresh
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("TappyTale Demo", "Demo prefabs, states, conditions and graph have been generated.", "OK");
        }

        /// <summary>
        /// Creates a demo scene containing ground, light, a target capsule and an instance of the demo enemy prefab.
        /// </summary>
        [MenuItem("Tools/TappyTale/Demo Install/Create Demo Scene", priority = 101)]
        public static void CreateDemoScene()
        {
            // Ensure demo prefabs exist
            string enemyPrefabPath = $"{DemoPrefabsFolder}/DemoEnemyAI.prefab";
            GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(enemyPrefabPath);
            if (enemyPrefab == null)
            {
                EditorUtility.DisplayDialog("TappyTale Demo", "Please generate demo prefabs first.", "OK");
                return;
            }

            // Create new scene
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Ground plane
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(10f, 1f, 10f);
            // Optionally build a navigation mesh here if your project includes NavMeshSurface.
            // To avoid compile errors when the Unity.AI.Navigation package is absent, we do not reference NavMeshSurface directly.
            // You can add a NavMeshSurface component to the ground manually and bake the navmesh after generation if needed.

            // Directional light
            var lightGO = new GameObject("Directional Light");
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = Color.white;
            light.intensity = 1.2f;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Create a target for the AI to chase
            var target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            target.name = "DemoPlayer";
            target.transform.position = new Vector3(5f, 0f, 5f);

            // Instantiate the enemy AI from prefab
            var enemyInstance = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);
            enemyInstance.transform.position = new Vector3(-5f, 0f, -5f);

            // Assign target to AI controller
            var aiController = enemyInstance.GetComponent<AIController>();
            if (aiController != null)
            {
                // Use SetTarget to assign the current target on the blackboard correctly.
                aiController.SetTarget(target.transform);
            }

            // Save scene
            string demoScenePath = $"{DemoRootFolder}/DemoScene.unity";
            EditorSceneManager.SaveScene(newScene, demoScenePath);

            EditorUtility.DisplayDialog("TappyTale Demo", $"Demo scene created at {demoScenePath}", "OK");
        }
    }
}
#endif