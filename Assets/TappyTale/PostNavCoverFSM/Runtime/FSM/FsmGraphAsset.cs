using System.Collections.Generic;
using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/FSM Graph")]
    public class FsmGraphAsset : ScriptableObject
    {
        public string EntryNodeGuid;
        public List<FsmGraphNodeData> Nodes = new List<FsmGraphNodeData>();
    }
}
