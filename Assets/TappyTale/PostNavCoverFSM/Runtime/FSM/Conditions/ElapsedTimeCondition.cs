using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM.Conditions
{
    [CreateAssetMenu(menuName = "TappyTale/PostNavCoverFSM/Conditions/Elapsed Time")]
    public class ElapsedTimeCondition : FSMCondition
    {
        [SerializeField] private float minimumDuration = 1f;

        public override bool Evaluate(AIController controller)
        {
            return controller != null && controller.Blackboard.StateElapsedTime >= minimumDuration;
        }
    }
}
