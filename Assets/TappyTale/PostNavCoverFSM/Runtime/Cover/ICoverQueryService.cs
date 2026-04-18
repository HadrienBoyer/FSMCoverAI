using UnityEngine;

namespace TappyTale.PostNavCoverFSM.Runtime.Cover
{
    public interface ICoverQueryService
    {
        bool TryFindBestCover(Transform seeker, Transform threat, out CoverQueryResult result);
    }
}
