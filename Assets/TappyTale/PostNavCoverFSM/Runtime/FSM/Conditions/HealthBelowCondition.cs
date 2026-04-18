using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Health Below")]
    public class HealthBelowCondition : FSMCondition
    {
        [SerializeField] private float threshold = 25f;

        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.Blackboard.Health <= threshold;
        }
    }
}
