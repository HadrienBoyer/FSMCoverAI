using UnityEngine;
using UnityEngine.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.Navigation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshNavigationAgent : MonoBehaviour, INavigationAgent
    {
        [SerializeField] private NavMeshAgent agent;

        public Vector3 Destination { get; private set; }
        public bool HasPath => agent != null && agent.hasPath;
        public bool IsMoving => agent != null && agent.velocity.sqrMagnitude > 0.001f;

        private void Reset()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Awake()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
        }

        public void SetDestination(Vector3 destination)
        {
            if (agent == null)
            {
                return;
            }

            Destination = destination;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public bool HasReachedDestination(float tolerance = 0.15f)
        {
            if (agent == null)
            {
                return true;
            }

            if (agent.pathPending)
            {
                return false;
            }

            if (agent.remainingDistance > agent.stoppingDistance + tolerance)
            {
                return false;
            }

            return !agent.hasPath || agent.velocity.sqrMagnitude <= 0.001f;
        }

        public void Stop()
        {
            if (agent == null)
            {
                return;
            }

            agent.isStopped = true;
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
        }
    }
}
