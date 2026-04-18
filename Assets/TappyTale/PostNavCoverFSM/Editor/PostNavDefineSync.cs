#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace TappyTale.PostNavCoverFSM.Editor
{
    [InitializeOnLoad]
    public static class PostNavDefineSync
    {
        private const string Define = "TAPPYTALE_POST_NAV_PRESENT";
        private const string TargetAssemblyQualifiedType = "KieranCoppins.PostNavigation.IPostAgent, kierancoppins.post-navigation";

        static PostNavDefineSync()
        {
            SyncDefines();
            EditorApplication.delayCall += SyncDefines;
        }

        private static void SyncDefines()
        {
            bool packagePresent = System.Type.GetType(TargetAssemblyQualifiedType) != null;
            NamedBuildTarget target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string defineString = PlayerSettings.GetScriptingDefineSymbols(target);
            var defines = defineString
                .Split(';')
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToList();

            bool hasDefine = defines.Contains(Define);

            if (packagePresent && !hasDefine)
            {
                defines.Add(Define);
                PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
            }
            else if (!packagePresent && hasDefine)
            {
                defines.RemoveAll(d => d == Define);
                PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
            }
        }
    }

    public sealed class PostNavDefineSyncBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Touch the type to ensure the static constructor runs and synchronises scripting defines.
            var _ = typeof(PostNavDefineSync);
        }
    }
}
#endif
