using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Cover
{
    public struct CoverQueryResult
    {
        public bool IsValid;
        public Vector3 Position;
        public Vector3 CoverDirection;
        public bool CanPeekLeft;
        public bool CanPeekRight;
        public AiCoverType CoverType;
        public Object Source;

        public static CoverQueryResult Invalid => new CoverQueryResult
        {
            IsValid = false,
            Position = Vector3.zero,
            CoverDirection = Vector3.forward,
            CanPeekLeft = false,
            CanPeekRight = false,
            CoverType = AiCoverType.Unknown,
            Source = null
        };
    }
}
