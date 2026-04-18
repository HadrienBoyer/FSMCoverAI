using System;
using System.Collections.Generic;
using KieranCoppins.PostNavigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZoneableAgentImpl : MonoBehaviour, IPostAgent
{
    /////////////////////////////
    /// IPostAgent Interface ///
    public Vector3 Position => transform.position;

    public Action<Zone> OnAssignedZone { get; set; }
    /////////////////////////////

    public Transform Target;
    private NavMeshAgent agent;

    void Awake()
    {
        // Assign delegate
        OnAssignedZone += AssignedToZone;

        // Get the nav mesh agent
        agent = GetComponent<NavMeshAgent>();

        // Request the closest zone on awake
        Zone zone = ZoneManager.Instance.GetClosestZoneToAgent(this);
        ZoneManager.Instance.RequestZone(zone, this);
    }

    private void AssignedToZone(Zone zone)
    {
        // Use the post selector to get a post within the zone
        PostSelector postSelector = new PostSelector(
            new List<IPostRule>{
                new IsNotOccupied(this),
                new IsInCover(Target, 1f),
                new HasLineOfSight(Target, 1.5f, 5f, false),
                new DistanceToTarget(transform, 1f, true)
            },
            (origin) => ZoneManager.Instance.GetPostsForAgent(this)
        );

        var postSelectorScores = postSelector.Run(transform.position);

        // Get the best post from the post selector
        IPost bestPost = postSelectorScores.GetBestPost();

        // We are going to occupy the post so tell the post manager
        PostManager.Instance.OccupyPost(bestPost, this);

        // Use unity's nav mesh agent to move to the post
        agent.SetDestination(bestPost.Position);
    }
}