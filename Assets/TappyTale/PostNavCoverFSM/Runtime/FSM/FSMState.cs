using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM
{
    public abstract class FSMState : ScriptableObject
    {
        public virtual void OnEnter(AIController controller) { }
        public virtual void OnTick(AIController controller, float deltaTime) { }
        public virtual void OnExit(AIController controller) { }
    }
}
