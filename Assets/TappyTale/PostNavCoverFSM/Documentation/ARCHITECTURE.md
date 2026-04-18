# Architecture

## Runtime layers

- `TappyTale.PostNavCoverFSM.Runtime.FSM`
  - ScriptableObject states
  - ScriptableObject conditions
  - Runtime `StateMachineController`
  - Blackboard and graph/runtime conversion data

- `TappyTale.PostNavCoverFSM.Runtime.AI`
  - `AIController`
  - target sensing
  - state machine ticking
  - blackboard orchestration

- `TappyTale.PostNavCoverFSM.Runtime.Cover`
  - `CoverPoint`
  - `CoverSelectionService`
  - fallback cover search
  - cover query result contracts

- `TappyTale.PostNavCoverFSM.Runtime.Navigation`
  - `INavigationAgent`
  - NavMesh implementation

- `TappyTale.PostNavCoverFSM.Runtime.Integration.PostNav`
  - `PnsPostAgentAdapter`
  - direct PNS zone/post querying
  - occupancy handling
  - translation of `ICoverPost` into runtime cover results

## Authoring

- `FsmGraphAsset`
- `FSMGraphWindow`
- `FsmGraphRebuilder`
- menu tools

## Suggested flow

1. Create state assets and condition assets
2. Create `FsmGraphAsset`
3. Open graph window
4. Place nodes, assign state assets, wire transitions
5. Rebuild `StateMachineController`
6. Add `AIController` and `NavMeshNavigationAgent`
7. Optionally add `PnsPostAgentAdapter`
8. Generate `CoverPoint` proxies from PNS posts if needed
