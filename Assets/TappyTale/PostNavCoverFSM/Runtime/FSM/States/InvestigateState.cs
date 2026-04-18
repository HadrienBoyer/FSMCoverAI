using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Investigate")]
    public class InvestigateState : FSMState
    {
        public override void OnEnter(AIController controller)
        {
            controller.EnterState(AICombatMode.None);
            controller.MoveTo(controller.Blackboard.LastKnownTargetPosition);
        }
    }
}
