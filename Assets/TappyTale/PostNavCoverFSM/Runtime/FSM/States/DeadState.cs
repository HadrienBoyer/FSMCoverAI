using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Dead")]
    public class DeadState : FSMState
    {
        public override void OnEnter(AIController controller)
        {
            controller.StopMoving();
            controller.EnterState(AICombatMode.Dead);
        }
    }
}
