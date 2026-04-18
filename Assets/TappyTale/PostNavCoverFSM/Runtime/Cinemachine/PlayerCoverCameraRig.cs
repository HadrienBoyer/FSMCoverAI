using UnityEngine;
using Unity.Cinemachine;

namespace TappyTale.PostNavCoverFSM.Runtime.CinemachineIntegration
{
    [DisallowMultipleComponent]
    public class PlayerCoverCameraRig : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera normalCamera;
        [SerializeField] private CinemachineCamera coverCamera;
        [SerializeField] private CinemachinePositionComposer coverComposer;
        [SerializeField] private float rightSideScreenX = 0.65f;
        [SerializeField] private float leftSideScreenX = 0.35f;

        public void EnterCover(bool peekRight)
        {
            if (coverComposer != null)
            {
                coverComposer.Composition.ScreenPosition.x = peekRight ? rightSideScreenX : leftSideScreenX;
            }

            if (normalCamera != null)
            {
                normalCamera.Priority = 10;
            }

            if (coverCamera != null)
            {
                coverCamera.Priority = 20;
            }
        }

        public void ExitCover()
        {
            if (normalCamera != null)
            {
                normalCamera.Priority = 20;
            }

            if (coverCamera != null)
            {
                coverCamera.Priority = 10;
            }
        }
    }
}
