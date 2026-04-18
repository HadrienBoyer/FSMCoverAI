using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.Common;
using TappyTale.PostNavCoverFSM.Runtime.Cover;
using TappyTale.PostNavCoverFSM.Runtime.FSM;
using TappyTale.PostNavCoverFSM.Runtime.Navigation;

namespace TappyTale.PostNavCoverFSM.Runtime.AI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StateMachineController))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private StateMachineController stateMachineController;
        [SerializeField] private LayerMask lineOfSightMask = ~0;
        [SerializeField] private float viewDistance = 25f;
        [SerializeField] private float coverSearchDistance = 30f;
        [SerializeField] private Transform aimPivot;

        public AIBlackboard Blackboard = new AIBlackboard();

        public INavigationAgent NavigationAgent { get; private set; }
        public ICoverQueryService CoverQueryService { get; private set; }
        public Transform AimPivot => aimPivot != null ? aimPivot : transform;

        private void Reset()
        {
            stateMachineController = GetComponent<StateMachineController>();
        }

        private void Awake()
        {
            if (stateMachineController == null)
            {
                stateMachineController = GetComponent<StateMachineController>();
            }

            ComponentUtility.TryGetInterface(this, out INavigationAgent navigationAgent);
            NavigationAgent = navigationAgent;

            ComponentUtility.TryGetInterface(this, out ICoverQueryService coverQueryService);
            CoverQueryService = coverQueryService;

            stateMachineController.Initialize(this);
        }

        private void Update()
        {
            UpdateSenses();
            Blackboard.StateElapsedTime += Time.deltaTime;
            stateMachineController.Tick(Time.deltaTime);
        }

        public void SetTarget(Transform target)
        {
            Blackboard.CurrentTarget = target;
            if (target != null)
            {
                Blackboard.LastKnownTargetPosition = target.position;
            }
        }

        public bool TryFindBestCover(out CoverQueryResult result)
        {
            if (Blackboard.CurrentTarget == null)
            {
                result = CoverQueryResult.Invalid;
                return false;
            }

            if (CoverQueryService != null &&
                CoverQueryService.TryFindBestCover(transform, Blackboard.CurrentTarget, out result))
            {
                return true;
            }

            return CoverSelectionService.TryFindBestCover(
                transform,
                Blackboard.CurrentTarget,
                coverSearchDistance,
                lineOfSightMask,
                out result);
        }

        public void MoveTo(Vector3 destination)
        {
            NavigationAgent?.SetDestination(destination);
        }

        public bool ReachedDestination()
        {
            return NavigationAgent == null || NavigationAgent.HasReachedDestination();
        }

        public void StopMoving()
        {
            NavigationAgent?.Stop();
        }

        public void FaceTargetFlat(Vector3 worldPosition, float rotateSpeed = 720f)
        {
            Vector3 direction = worldPosition - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        public void EnterState(AICombatMode combatMode)
        {
            Blackboard.CombatMode = combatMode;
            Blackboard.StateElapsedTime = 0f;
        }

        private void UpdateSenses()
        {
            Transform target = Blackboard.CurrentTarget;
            if (target == null)
            {
                Blackboard.HasLineOfSight = false;
                return;
            }

            Vector3 origin = AimPivot.position;
            Vector3 destination = target.position;
            Vector3 direction = destination - origin;

            if (direction.sqrMagnitude > viewDistance * viewDistance)
            {
                Blackboard.HasLineOfSight = false;
                return;
            }

            bool hit = Physics.Raycast(
                origin,
                direction.normalized,
                out RaycastHit hitInfo,
                direction.magnitude,
                lineOfSightMask);

            Blackboard.HasLineOfSight = !hit || hitInfo.transform == target || hitInfo.transform.IsChildOf(target);
            if (Blackboard.HasLineOfSight)
            {
                Blackboard.LastKnownTargetPosition = target.position;
            }
        }
    }
}
