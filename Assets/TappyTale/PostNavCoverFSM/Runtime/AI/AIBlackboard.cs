using System;
using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.Cover;

namespace TappyTale.PostNavCoverFSM.Runtime.AI
{
    [Serializable]
    public class AIBlackboard
    {
        public Transform CurrentTarget;
        public Vector3 LastKnownTargetPosition;
        public bool HasLineOfSight;
        public bool IsReloading;
        public float Health = 100f;
        public float MaxHealth = 100f;
        public float StateElapsedTime;
        public CoverQueryResult CurrentCover;
        public AICombatMode CombatMode;
    }
}
