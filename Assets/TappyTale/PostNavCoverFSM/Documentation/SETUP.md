# Setup

## Base setup

1. Import the package under `Assets/TappyTale/PostNavCoverFSM`
2. Add a `NavMeshAgent`
3. Add `NavMeshNavigationAgent`
4. Add `AIController`
5. Add `StateMachineController`
6. Create graph/state assets
7. Rebuild the controller from the graph

## With Post Navigation System

1. Import Post Navigation System
2. Wait for domain reload
3. Confirm the editor has added `TAPPYTALE_POST_NAV_PRESENT`
4. Add `PnsPostAgentAdapter` on the AI agent
5. Add `ZoneManager` in scene if you use zones
6. Generate navmesh posts using the PNS workflow
7. Optionally run `Tools/TappyTale/PostNav Cover/Generate CoverPoints From PNS Posts`

## Recommended FSM chain

- Idle
- SeekCover
- MoveToCover
- InCover
- PeekAndShoot
- Reload
- Dead
