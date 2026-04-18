using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Has Assigned Cover")]
    public class HasAssignedCoverCondition : FSMCondition
    {
        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.Blackboard.CurrentCover.IsValid;
        }
    }
}
