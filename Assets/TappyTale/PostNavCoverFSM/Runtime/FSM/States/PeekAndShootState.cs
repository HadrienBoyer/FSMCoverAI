using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Peek And Shoot")]
    public class PeekAndShootState : FSMState
    {
        [SerializeField] private float burstDuration = 1.25f;

        public override void OnEnter(AIController controller)
        {
            controller.EnterState(AICombatMode.Attacking);
        }

        public override void OnTick(AIController controller, float deltaTime)
        {
            if (controller.Blackboard.CurrentTarget != null)
            {
                controller.FaceTargetFlat(controller.Blackboard.CurrentTarget.position);
            }

            if (controller.Blackboard.StateElapsedTime >= burstDuration)
            {
                controller.Blackboard.IsReloading = true;
            }
        }

        public override void OnExit(AIController controller)
        {
            controller.Blackboard.IsReloading = false;
        }
    }
}
