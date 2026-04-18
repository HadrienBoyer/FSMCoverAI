using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/No Target")]
    public class NoTargetCondition : FSMCondition
    {
        public override bool Evaluate(AIController controller)
        {
            return controller == null || controller.Blackboard.CurrentTarget == null;
        }
    }
}
