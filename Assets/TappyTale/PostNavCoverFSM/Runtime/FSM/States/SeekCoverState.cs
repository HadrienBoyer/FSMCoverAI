using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Seek Cover")]
    public class SeekCoverState : FSMState
    {
        public override void OnEnter(AIController controller)
        {
            controller.EnterState(AICombatMode.SeekingCover);

            if (controller.TryFindBestCover(out var cover))
            {
                controller.Blackboard.CurrentCover = cover;
                controller.MoveTo(cover.Position);
                return;
            }

            controller.Blackboard.CurrentCover = Runtime.Cover.CoverQueryResult.Invalid;
            if (controller.Blackboard.CurrentTarget != null)
            {
                controller.MoveTo(controller.Blackboard.LastKnownTargetPosition);
            }
        }
    }
}
