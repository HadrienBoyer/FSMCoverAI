using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Has Target")]
    public class HasTargetCondition : FSMCondition
    {
        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.Blackboard.CurrentTarget != null;
        }
    }
}
