# FishNet Rigidbody Test

The following project showcases a possible bug with a CSP Rigidbody2D movement implementation placed on multiple non-owned objects.

## How to Run

Clone the repository, open the project on Unity (2022.3.20f1), then run the game on two instances.

### Target Demo
By pressing Spacebar on the **Server**, it will spawn a circle that moves towards a target (in this case (0, 0)).

### Balls Demo
By pressing Spacebar on the **Server**, it will spawn a ball that falls with gravity and has a bouncy physics material. The code used to sync the rigidbody comes directly from the [FishNet docs]([url](https://fish-networking.gitbook.io/docs/manual/guides/prediction/version-2-experimental/getting-started)).

## The Bug

When spawning <= 32 objects, the system works just fine. After this threshold, the objects will desync heavily.
