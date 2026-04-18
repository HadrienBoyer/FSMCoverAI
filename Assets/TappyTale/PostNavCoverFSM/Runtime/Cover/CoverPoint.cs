using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Cover
{
    [DisallowMultipleComponent]
    public class CoverPoint : MonoBehaviour
    {
        // Stores the type of cover using the internal AiCoverType to avoid conflicts with external packages.
        [field: SerializeField] public AiCoverType CoverType { get; private set; } = AiCoverType.High;
        [field: SerializeField] public Vector3 CoverDirection { get; private set; } = Vector3.forward;
        [field: SerializeField] public bool CanPeekLeft { get; private set; } = true;
        [field: SerializeField] public bool CanPeekRight { get; private set; } = true;

        private void OnEnable()
        {
            CoverSelectionService.Register(this);
        }

        private void OnDisable()
        {
            CoverSelectionService.Unregister(this);
        }

        public void Configure(AiCoverType coverType, Vector3 coverDirection, bool canPeekLeft, bool canPeekRight)
        {
            CoverType = coverType;
            CoverDirection = coverDirection.sqrMagnitude > 0.0001f ? coverDirection.normalized : Vector3.forward;
            CanPeekLeft = canPeekLeft;
            CanPeekRight = canPeekRight;
        }

        public CoverQueryResult ToQueryResult()
        {
            return new CoverQueryResult
            {
                IsValid = true,
                Position = transform.position,
                CoverDirection = CoverDirection,
                CanPeekLeft = CanPeekLeft,
                CanPeekRight = CanPeekRight,
                CoverType = CoverType,
                Source = this
            };
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, CoverDirection.normalized);
        }
#endif
    }
}
