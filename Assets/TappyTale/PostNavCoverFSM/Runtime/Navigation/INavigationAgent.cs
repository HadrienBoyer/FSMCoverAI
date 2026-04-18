using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Navigation
{
    public interface INavigationAgent
    {
        Vector3 Destination { get; }
        bool HasPath { get; }
        bool IsMoving { get; }
        void SetDestination(Vector3 destination);
        bool HasReachedDestination(float tolerance = 0.15f);
        void Stop();
    }
}
