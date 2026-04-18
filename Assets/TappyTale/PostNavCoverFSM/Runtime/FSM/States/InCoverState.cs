using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/In Cover")]
    public class InCoverState : FSMState
    {
        public override void OnEnter(AIController controller)
        {
            controller.StopMoving();
            controller.EnterState(AICombatMode.InCover);
        }

        public override void OnTick(AIController controller, float deltaTime)
        {
            if (controller.Blackboard.CurrentTarget != null)
            {
                controller.FaceTargetFlat(controller.Blackboard.CurrentTarget.position);
            }
        }
    }
}
