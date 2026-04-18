using System;
using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;
using TappyTale.PostNavCoverFSM.Runtime.Cover;
using AiCoverType = TappyTale.PostNavCoverFSM.Runtime.Cover.AiCoverType;

#if TAPPYTALE_POST_NAV_PRESENT
using System.Collections.Generic;
using KieranCoppins.PostNavigation;
// Alias external cover type only when the Post Navigation package is present.
using PnsCoverType = KieranCoppins.PostNavigation.CoverType;
#endif

namespace TappyTale.PostNavCoverFSM.Runtime.Integration.PostNav
{
#if TAPPYTALE_POST_NAV_PRESENT
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AIController))]
    public class PnsPostAgentAdapter : MonoBehaviour, IPostAgent, ICoverQueryService
    {
        [SerializeField] private bool autoRequestClosestZoneOnEnable = true;
        [SerializeField] private float isInCoverDistance = 2f;
        [SerializeField] private float lineOfSightHeightOffset = 1.5f;
        [SerializeField] private float distanceWeight = 1f;

        private AIController controller;
        private Zone currentZone;
        private IPost currentPost;

        public Vector3 Position => transform.position;
        public Action<Zone> OnAssignedZone { get; set; }

        private void Awake()
        {
            controller = GetComponent<AIController>();
            OnAssignedZone += HandleAssignedZone;
        }

        private void OnEnable()
        {
            if (autoRequestClosestZoneOnEnable && ZoneManager.Instance != null)
            {
                RequestClosestZone();
            }
        }

        public void RequestClosestZone()
        {
            if (ZoneManager.Instance == null)
            {
                return;
            }

            Zone zone = ZoneManager.Instance.GetClosestZoneToAgent(this);
            if (zone != null)
            {
                ZoneManager.Instance.RequestZone(zone, this);
            }
        }

        public bool TryFindBestCover(Transform seeker, Transform threat, out CoverQueryResult result)
        {
            result = CoverQueryResult.Invalid;

            if (threat == null)
            {
                return false;
            }

            var selector = new PostSelector(
                new List<IPostRule>
                {
                    new IsNotOccupied(this),
                    new IsInCover(threat, isInCoverDistance, 1f, true),
                    new HasLineOfSight(threat, lineOfSightHeightOffset, 1f, false),
                    new DistanceToTarget(seeker, distanceWeight, true)
                },
                origin => ZoneManager.Instance != null
                    ? ZoneManager.Instance.GetPostsForAgent(this)
                    : Array.Empty<IPost>());

            var scores = selector.Run(transform.position);
            // The selector can return an empty score dictionary; in that case, no candidate posts are available.
            if (scores == null || scores.Count == 0)
            {
                return false;
            }
            IPost bestPost = scores.GetBestPost();
            if (bestPost == null)
            {
                return false;
            }

            currentPost = bestPost;
            if (PostManager.Instance != null)
            {
                PostManager.Instance.OccupyPost(bestPost, this);
            }

            Vector3 coverDirection = Vector3.forward;
            bool canPeekLeft = true;
            bool canPeekRight = true;
            AiCoverType coverType = AiCoverType.Unknown;

            if (bestPost is ICoverPost coverPost)
            {
                coverDirection = coverPost.CoverDirection;
                // Use the "peak" spellings from the Post Navigation System's ICoverPost interface. They map to our internal "peek" booleans.
                canPeekLeft = coverPost.CanPeakLeft;
                canPeekRight = coverPost.CanPeakRight;
                coverType = ConvertCoverType(coverPost.CoverType);
            }

            result = new CoverQueryResult
            {
                IsValid = true,
                Position = bestPost.Position,
                CoverDirection = coverDirection.sqrMagnitude > 0.0001f ? coverDirection.normalized : Vector3.forward,
                CanPeekLeft = canPeekLeft,
                CanPeekRight = canPeekRight,
                CoverType = coverType,
                Source = this
            };

            return true;
        }

        private void HandleAssignedZone(Zone zone)
        {
            currentZone = zone;
        }

        private static AiCoverType ConvertCoverType(PnsCoverType externalType)
        {
            string enumName = externalType.ToString();
            return enumName switch
            {
                "LowCover" => AiCoverType.Low,
                "Low" => AiCoverType.Low,
                "HighCover" => AiCoverType.High,
                "High" => AiCoverType.High,
                _ => AiCoverType.Unknown
            };
        }
    }
#else
    [DisallowMultipleComponent]
    public class PnsPostAgentAdapter : MonoBehaviour, ICoverQueryService
    {
        public bool TryFindBestCover(Transform seeker, Transform threat, out CoverQueryResult result)
        {
            result = CoverQueryResult.Invalid;
            return false;
        }
    }
#endif
}
