# TappyTale Post Navigation Cover FSM

Unity 6 package for a cover-oriented FPS AI stack with:

- ScriptableObject FSM
- Runtime `StateMachineController`
- Cover system with local cover fallback
- Optional direct Post Navigation System integration
- FSM Graph Editor with save/load and controller rebuild
- CoverPoint generator from Post Navigation posts
- Optional Cinemachine 3 cover camera helper

## Folder import

Copy `Assets/TappyTale/PostNavCoverFSM` into your Unity project.

## Important symbols

This package includes an editor script that auto-adds and auto-removes the scripting define:

- `TAPPYTALE_POST_NAV_PRESENT`

It is added when the Post Navigation System assembly `kierancoppins.post-navigation.dll` is detected.

## Notes

- The core package compiles without Post Navigation System.
- The direct PNS bridge compiles only when the package is present.
- The Graph Editor targets Unity's GraphView editor API.
