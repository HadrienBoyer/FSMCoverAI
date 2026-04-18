using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Common
{
    public static class ComponentUtility
    {
        public static bool TryGetInterface<T>(Component source, out T result) where T : class
        {
            result = null;
            if (source == null)
            {
                return false;
            }

            var components = source.GetComponents<MonoBehaviour>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T typed)
                {
                    result = typed;
                    return true;
                }
            }

            return false;
        }
    }
}
