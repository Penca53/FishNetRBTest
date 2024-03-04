# FishNet Rigidbody Test

The following project showcases a possible bug with a CSP Rigidbody2D movement implementation placed on multiple non-owned objects.

## How to Run

Clone the repository, open the project on Unity (2022.3.20f1), then run the game on two instances.
By pressing Spacebar on the **Server**, it will spawn an object that rotates in a circle.

## The Bug

When spawning <= 32 objects, the system works just fine. After this threshold, the only following objects will be heavily desynced.
