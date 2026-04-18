using System;
using System.Collections.Generic;
using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM
{
    [Serializable]
    public class FSMTransitionDefinition
    {
        public FSMCondition Condition;
        public FSMState TargetState;
    }

    [Serializable]
    public class FSMStateDefinition
    {
        public FSMState State;
        public List<FSMTransitionDefinition> Transitions = new List<FSMTransitionDefinition>();
    }

    [Serializable]
    public class FsmGraphNodeData
    {
        public string Guid;
        public string Title;
        public Vector2 Position;
        public FSMState State;
        public List<FsmGraphTransitionData> Transitions = new List<FsmGraphTransitionData>();
    }

    [Serializable]
    public class FsmGraphTransitionData
    {
        public string TargetNodeGuid;
        public FSMCondition Condition;
    }
}
