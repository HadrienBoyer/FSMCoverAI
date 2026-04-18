using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Move To Cover")]
    public class MoveToCoverState : FSMState
    {
        public override void OnEnter(AIController controller)
        {
            controller.EnterState(AICombatMode.SeekingCover);

            if (controller.Blackboard.CurrentCover.IsValid)
            {
                controller.MoveTo(controller.Blackboard.CurrentCover.Position);
            }
        }

        public override void OnTick(AIController controller, float deltaTime)
        {
            if (controller.Blackboard.CurrentCover.IsValid)
            {
                Vector3 direction = controller.Blackboard.CurrentCover.CoverDirection;
                controller.FaceTargetFlat(controller.transform.position + direction);
            }
        }
    }
}
