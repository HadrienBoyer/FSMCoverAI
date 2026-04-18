using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM
{
    public abstract class FSMCondition : ScriptableObject
    {
        public abstract bool Evaluate(AIController controller);
    }
}
