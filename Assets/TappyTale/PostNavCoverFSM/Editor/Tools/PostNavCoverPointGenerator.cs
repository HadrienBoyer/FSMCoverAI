#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TappyTale.PostNavCoverFSM.Runtime.Cover;
using AiCoverType = TappyTale.PostNavCoverFSM.Runtime.Cover.AiCoverType;

#if TAPPYTALE_POST_NAV_PRESENT
using KieranCoppins.PostNavigation;
#endif

namespace TappyTale.PostNavCoverFSM.Editor.Tools
{
    public static class PostNavCoverPointGenerator
    {
        [MenuItem("Tools/TappyTale/PostNav Cover/Generate CoverPoints From PNS Posts")]
        public static void Generate()
        {
#if TAPPYTALE_POST_NAV_PRESENT
            IPost[] posts = PostGenerators.GetPostFromSceneData();
            if (posts == null || posts.Length == 0)
            {
                EditorUtility.DisplayDialog("PNS Cover Generator", "No PNS scene posts were loaded. Generate PNS posts first.", "OK");
                return;
            }

            Scene activeScene = SceneManager.GetActiveScene();
            string rootName = $"GeneratedCoverPoints_{activeScene.name}";
            GameObject root = GameObject.Find(rootName);
            if (root == null)
            {
                root = new GameObject(rootName);
                Undo.RegisterCreatedObjectUndo(root, "Create CoverPoint Root");
            }

            int createdCount = 0;
            int index = 0;
            foreach (IPost post in posts)
            {
                if (post is not ICoverPost coverPost)
                {
                    continue;
                }

                GameObject coverObject = new GameObject($"CoverPoint_{index:D4}");
                Undo.RegisterCreatedObjectUndo(coverObject, "Create CoverPoint");
                coverObject.transform.SetParent(root.transform);
                coverObject.transform.position = post.Position;

                var point = coverObject.AddComponent<CoverPoint>();
                point.Configure(
                    ConvertCoverType(coverPost.CoverType),
                    coverPost.CoverDirection,
                    // Use the "peak" spellings from the PNS ICoverPost interface. They map to our internal "peek" booleans.
                    coverPost.CanPeakLeft,
                    coverPost.CanPeakRight);

                createdCount++;
                index++;
            }

            Selection.activeObject = root;
            EditorUtility.DisplayDialog("PNS Cover Generator", $"Generated {createdCount} CoverPoint objects from PNS scene posts.", "OK");
#else
            EditorUtility.DisplayDialog("PNS Cover Generator", "Post Navigation System is not detected. Import the package first.", "OK");
#endif
        }

#if TAPPYTALE_POST_NAV_PRESENT
        private static AiCoverType ConvertCoverType(KieranCoppins.PostNavigation.CoverType externalType)
        {
            string enumName = externalType.ToString();
            return enumName switch
            {
                "LowCover" => AiCoverType.Low,
                "Low" => AiCoverType.Low,
                "HighCover" => AiCoverType.High,
                "High" => AiCoverType.High,
                _ => AiCoverType.Unknown
            };
        }
#endif
    }
}
#endif
