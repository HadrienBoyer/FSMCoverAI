using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.States
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/States/Reload")]
    public class ReloadState : FSMState
    {
        [SerializeField] private float reloadDuration = 1.5f;

        public override void OnEnter(AIController controller)
        {
            controller.EnterState(AICombatMode.Reloading);
            controller.Blackboard.IsReloading = true;
        }

        public override void OnTick(AIController controller, float deltaTime)
        {
            if (controller.Blackboard.CurrentTarget != null)
            {
                controller.FaceTargetFlat(controller.Blackboard.CurrentTarget.position);
            }

            if (controller.Blackboard.StateElapsedTime >= reloadDuration)
            {
                controller.Blackboard.IsReloading = false;
            }
        }

        public override void OnExit(AIController controller)
        {
            controller.Blackboard.IsReloading = false;
        }
    }
}
