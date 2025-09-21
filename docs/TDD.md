# Technical Design Document — Core Systems

## Goals
Camera-relative strafing controller with predictable animation sync, data-driven tuning, future‑proof hooks for combat and buffs.

## Structure
```
Assets/Code/
  Core/                (EventBus, ServiceLocator, Pool)
  Systems/
    Input/             (PlayerInputDriver)
    Camera/            (CameraRig, CameraCollision)
    Combat/            (Hitbox, Hurtbox, Damage, Block)
  Gameplay/
    Player/
      PlayerBrain.cs       (camera-relative movement)
      PlayerAnimatorDriver.cs
  Data/                (ScriptableObjects)
  Tests/
```

## Data (SOs)
- `PlayerConfig`: walkSpeed, sprintMultiplier, rotationSharpness, gravity, groundedGravity, baseDefense.
- `AttackConfig`: weaponClass(1H/2H), damage, windup, active, recovery, coneDeg, reach.
- `CameraConfig`: offsets, pitch range, zoom, smoothing, collisionRadius.

## Input
Actions: Move, Sprint, Attack, Orbit, Zoom. `PlayerInputDriver` produces intents.

## Movement
- `PlayerBrain` reads `PlayerInputDriver`, projects move axes onto camera forward/right, and feeds a `CharacterController` with constant-speed strafing (no acceleration curves).
- Base speed comes from `PlayerConfig.walkSpeed`; sprint/buff multipliers adjust target speed directly.
- `NormalizedSpeed` = `currentSpeed / baseSpeed` (>=0), driving animation speed so clips stay in sync as buffs scale.
- Optional gravity keeps the controller grounded; no jump/dash logic baked in.

## Hitboxes
Arc sector during `active`: test angle and distance vs Hurtboxes. One hit per target per swing.

## Camera
Follow with smoothing; orbit clamped; spherecast collision and pull‑in to avoid occlusion.

## Tests
- Movement vector math (camera projection, normalized speed ratio).  
- Hitbox arc math edge cases.  
- Camera occlusion ≤2 frames.  
- p99 frame ≤20 ms in test scene.
