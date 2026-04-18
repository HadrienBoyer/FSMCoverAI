using System.Collections.Generic;
using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Cover
{
    public static class CoverSelectionService
    {
        private static readonly List<CoverPoint> CoverPoints = new List<CoverPoint>();

        public static void Register(CoverPoint coverPoint)
        {
            if (coverPoint == null || CoverPoints.Contains(coverPoint))
            {
                return;
            }

            CoverPoints.Add(coverPoint);
        }

        public static void Unregister(CoverPoint coverPoint)
        {
            if (coverPoint == null)
            {
                return;
            }

            CoverPoints.Remove(coverPoint);
        }

        public static bool TryFindBestCover(
            Transform seeker,
            Transform threat,
            float maxDistance,
            LayerMask obstructionMask,
            out CoverQueryResult result)
        {
            result = CoverQueryResult.Invalid;

            if (seeker == null || threat == null)
            {
                return false;
            }

            float bestScore = float.NegativeInfinity;
            Vector3 threatPosition = threat.position;
            Vector3 seekerPosition = seeker.position;

            for (int i = 0; i < CoverPoints.Count; i++)
            {
                var point = CoverPoints[i];
                if (point == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(seekerPosition, point.transform.position);
                if (distance > maxDistance)
                {
                    continue;
                }

                Vector3 towardThreat = (threatPosition - point.transform.position).normalized;
                float coverFacingScore = Vector3.Dot(point.CoverDirection.normalized, towardThreat * -1f);
                if (coverFacingScore < 0.25f)
                {
                    continue;
                }

                bool blocked = Physics.Linecast(
                    point.transform.position + Vector3.up * 1.2f,
                    threatPosition + Vector3.up * 1.2f,
                    obstructionMask);

                if (!blocked)
                {
                    continue;
                }

                float score = coverFacingScore * 4f;
                score += Mathf.Clamp01(1f - (distance / Mathf.Max(0.01f, maxDistance))) * 2f;

                if (score > bestScore)
                {
                    bestScore = score;
                    result = point.ToQueryResult();
                }
            }

            return result.IsValid;
        }
    }
}
