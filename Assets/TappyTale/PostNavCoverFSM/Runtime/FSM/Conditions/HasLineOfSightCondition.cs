using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Has Line Of Sight")]
    public class HasLineOfSightCondition : FSMCondition
    {
        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.Blackboard.HasLineOfSight;
        }
    }
}
