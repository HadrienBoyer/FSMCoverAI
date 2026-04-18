using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Reached Destination")]
    public class ReachedDestinationCondition : FSMCondition
    {
        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.ReachedDestination();
        }
    }
}
