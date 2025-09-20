# Technical Design Document — Core Systems

## Goals
Layered FSM (Movement || Combat), data‑driven configs, clean input→intent→state flow, multiplayer‑friendly later.

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
      FSM/
        Movement/      (Idle, Run, Sprint)
        Combat/        (Idle, LightSwing1H, HeavySwing2H)
      PlayerBrain.cs
  Data/                (ScriptableObjects)
  Tests/
```

## Data (SOs)
- `PlayerConfig`: speeds, accel/decel, baseDefense.
- `AttackConfig`: weaponClass(1H/2H), damage, windup, active, recovery, coneDeg, reach.
- `CameraConfig`: offsets, pitch range, zoom, smoothing, collisionRadius.

## Input
Actions: Move, Sprint, Attack, Orbit, Zoom. `PlayerInputDriver` produces intents.

## FSMs
Interfaces: `IState`, `StateMachine`.  
Movement: Idle(0), Run(1), Sprint(2).  
Combat: Idle(0), Swings(2).  
Equal priority → allow Combat to proceed while Movement continues.

## Hitboxes
Arc sector during `active`: test angle and distance vs Hurtboxes. One hit per target per swing.

## Camera
Follow with smoothing; orbit clamped; spherecast collision and pull‑in to avoid occlusion.

## Tests
- Guards and transitions.  
- Hitbox arc math edge cases.  
- Camera occlusion ≤2 frames.  
- p99 frame ≤20 ms in test scene.
